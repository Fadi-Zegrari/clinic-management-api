using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
            : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ==================================================
            // Configure Appointment Entity
            // ==================================================
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                // Primary Key
                entity.HasKey(e => e.AppointmentID);

                // Column configurations
                entity.Property(e => e.AppointmentDate)
                      .HasColumnType("datetime2");

                // Relationships with exact database constraint names
                entity.HasOne(d => d.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(d => d.PatientID)
                      .HasConstraintName("FK_Appointment_Patient");

                entity.HasOne(d => d.Doctor)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(d => d.DoctorID)
                      .HasConstraintName("FK_Appointment_Doctor");

                entity.HasOne(d => d.AppointmentStatus)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(d => d.AppointmentStatusID)
                      .HasConstraintName("FK_Appointment_Status");
            });


            // ==================================================
            // Configure AppointmentStatus Entity
            // ==================================================
            modelBuilder.Entity<AppointmentStatus>(entity =>
            {
                entity.ToTable("AppointmentStatus");

                entity.HasKey(e => e.AppointmentStatusID);

                entity.Property(e => e.StatusName)
                      .IsRequired()
                      .HasMaxLength(50);
            });


            // ==================================================
            // Configure Doctor Entity
            // ==================================================
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctor");

                // Primary Key
                entity.HasKey(e => e.DoctorID);

                // Auto increment
                entity.Property(e => e.DoctorID)
                      .ValueGeneratedOnAdd();

                // Required columns
                entity.Property(e => e.PersonID)
                      .IsRequired();

                entity.Property(e => e.SpecializationID)
                      .IsRequired();

                // Unique constraint: one Person = one Doctor
                // مطابق لـ UQ_Doctor_... في قاعدة البيانات
                entity.HasIndex(e => e.PersonID)
                      .IsUnique()
                      .HasDatabaseName("UQ_Doctor_PersonID");

                // Relationship with Person (One-to-One)
                entity.HasOne(d => d.Person)
                      .WithOne()                // ← بدون navigation
                      .HasForeignKey<Doctor>(d => d.PersonID)
                      .HasConstraintName("FK_Doctor_Person");

                // Relationship with Specialization (Many-to-One)
                entity.HasOne(d => d.Specialization)
                      .WithMany(s => s.Doctors)
                      .HasForeignKey(d => d.SpecializationID)
                      .HasConstraintName("FK_Doctor_Specialization");
            });


            // ==================================================
            // Configure MedicalRecord Entity
            // ==================================================
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.ToTable("MedicalRecord");


                // Primary Key
                entity.HasKey(e => e.MedicalRecordID);

                // Auto increment
                entity.Property(e => e.MedicalRecordID)
                      .ValueGeneratedOnAdd();

                // Required columns
                entity.Property(e => e.AppointmentID)
                      .IsRequired();

                entity.Property(e => e.Diagnosis)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.Treatment)
                      .HasMaxLength(500);

                entity.Property(e => e.Notes)
                      .HasMaxLength(1000);

                entity.Property(e => e.RecordDate)
                      .HasColumnType("datetime2");

                // Relationship: Many MedicalRecords → One Appointment
                entity.HasOne(d => d.Appointment)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(d => d.AppointmentID)
                      .HasConstraintName("FK_MedicalRecord_Appointment");
            });


            // ==================================================
            // Configure Patient Entity
            // ==================================================
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                // Primary Key
                entity.HasKey(e => e.PatientID);

                // Auto increment
                entity.Property(e => e.PatientID)
                      .ValueGeneratedOnAdd();

                // Required columns
                entity.Property(e => e.PersonID)
                      .IsRequired();

                // Unique constraint: one Person = one Patient
                // مطابق لـ UQ_Patient_... في DB
                entity.HasIndex(e => e.PersonID)
                      .IsUnique()
                      .HasDatabaseName("UQ_Patient_PersonID");

                // Relationship with Person (One-to-One)
                entity.HasOne(d => d.Person)
                      .WithOne()
                      .HasForeignKey<Patient>(d => d.PersonID)
                      .HasConstraintName("FK_Patient_Person");
            });


            // ==================================================
            // Configure Payment Entity
            // ==================================================
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");


                // Primary Key
                entity.HasKey(e => e.PaymentID);

                // Auto increment
                entity.Property(e => e.PaymentID)
                      .ValueGeneratedOnAdd();

                // Required columns
                entity.Property(e => e.AppointmentID)
                      .IsRequired();

                entity.Property(e => e.PaymentMethodID)
                      .IsRequired();

                // Decimal precision
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(10, 2)")
                      .IsRequired();

                // Date
                entity.Property(e => e.PaymentDate)
                      .HasColumnType("datetime2")
                      .IsRequired();

                // Relationship: Many Payments → One Appointment
                entity.HasOne(d => d.Appointment)
                      .WithMany(p => p.Payments)
                      .HasForeignKey(d => d.AppointmentID)
                      .HasConstraintName("FK_Payment_Appointment");

                // Relationship: Many Payments → One PaymentMethod
                entity.HasOne(d => d.PaymentMethod)
                      .WithMany(p => p.Payments)
                      .HasForeignKey(d => d.PaymentMethodID)
                      .HasConstraintName("FK_Payment_PaymentMethod");
            });


            // ==================================================
            // Configure PaymentMethod Entity
            // ==================================================
            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("PaymentMethod");


                // Primary Key
                entity.HasKey(e => e.PaymentMethodID);

                // Auto increment
                entity.Property(e => e.PaymentMethodID)
                      .ValueGeneratedOnAdd();

                // MethodName: required + max length
                entity.Property(e => e.MethodName)
                      .IsRequired()
                      .HasMaxLength(50);

                // Unique: MethodName لا يتكرر
                // مطابق لـ UQ_PaymentM_... في DB
                entity.HasIndex(e => e.MethodName)
                      .IsUnique()
                      .HasDatabaseName("UQ_PaymentMethod_MethodName");
            });


            // ==================================================
            // Configure Person Entity
            // ==================================================
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");


                // Primary Key
                entity.HasKey(e => e.PersonID);

                // Auto increment
                entity.Property(e => e.PersonID)
                      .ValueGeneratedOnAdd();

                // Name: required + max length
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                // DateOfBirth: required, نوع date
                entity.Property(e => e.DateOfBirth)
                      .HasColumnType("date")
                      .IsRequired();

                // Gender: required + max length
                entity.Property(e => e.Gender)
                      .IsRequired()
                      .HasMaxLength(10);

                // Phone: required + max length
                entity.Property(e => e.Phone)
                      .IsRequired()
                      .HasMaxLength(20);

                // Email: required + max length + unique
                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                // Unique: Email لا يتكرر
                // مطابق لـ UQ_Person_... في DB
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("UQ_Person_Email");

                // Address: optional + max length
                entity.Property(e => e.Address)
                      .HasMaxLength(200);
            });


            // ==================================================
            // Configure Prescription Entity
            // ==================================================
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.ToTable("Prescription");


                // Primary Key
                entity.HasKey(e => e.PrescriptionID);

                // Auto increment
                entity.Property(e => e.PrescriptionID)
                      .ValueGeneratedOnAdd();

                // Required columns
                entity.Property(e => e.MedicalRecordID)
                      .IsRequired();

                entity.Property(e => e.MedicationName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Dosage)
                      .IsRequired()
                      .HasMaxLength(100);

                // Optional
                entity.Property(e => e.Instructions)
                      .HasMaxLength(500);

                // Relationship: Many Prescriptions → One MedicalRecord
                entity.HasOne(d => d.MedicalRecord)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(d => d.MedicalRecordID)
                      .HasConstraintName("FK_Prescription_MedicalRecord");
            });


            // ==================================================
            // Configure Specialization Entity
            // ==================================================
            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization");


                // Primary Key
                entity.HasKey(e => e.SpecializationID);

                // Auto increment
                entity.Property(e => e.SpecializationID)
                      .ValueGeneratedOnAdd();

                // Required + max length
                entity.Property(e => e.SpecializationName)
                      .IsRequired()
                      .HasMaxLength(100);

                // Unique: SpecializationName لا يتكرر
                // مطابق لـ UQ_Speciali_... في DB
                entity.HasIndex(e => e.SpecializationName)
                      .IsUnique()
                      .HasDatabaseName("UQ_Specialization_Name");
            });

        }
    }
}