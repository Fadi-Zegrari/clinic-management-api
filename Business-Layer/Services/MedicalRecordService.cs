using Business_Layer.DTOs.MedicalRecords;
using Business_Layer.Interfaces;
using Data_Access.Entities;
using Data_Access.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MedicalRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MedicalRecordReadDto>> GetAllAsync()
        {
            var medicalRecords = await _unitOfWork
                .Repository<MedicalRecord>()
                    .GetAllAsync();

            var result = new List<MedicalRecordReadDto>();

            foreach (var record in medicalRecords)
            {
                var dto = await BuildReadDtoAsync(record);

                if (dto is not null)
                    result.Add(dto);
            }

            return result;
        }

        public async Task<MedicalRecordReadDto?> GetByIdAsync(int id)
        {
            var record = await _unitOfWork
                .Repository<MedicalRecord>()
                    .GetByIdAsync(id);

            if (record is null)
                 return null;

            return await BuildReadDtoAsync(record);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<MedicalRecord>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(MedicalRecordCreateDto dto)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                     .GetByIdAsync(dto.AppointmentID);

            if (appointment is null)
                throw new ArgumentException("Appointment not found.");

            var recordAlreadyExists = await _unitOfWork
                .Repository<MedicalRecord>()
                    .AnyAsync(r => r.AppointmentID == dto.AppointmentID);

            if (recordAlreadyExists)
                throw new InvalidOperationException(
                      "A medical record already exists for this appointment.");

            var medicalRecord = new MedicalRecord
            {
                AppointmentID = dto.AppointmentID,
                Diagnosis = dto.Diagnosis,
                Treatment = dto.Treatment,
                Notes = dto.Notes,
                RecordDate = dto.RecordDate
            };

            await _unitOfWork
                .Repository<MedicalRecord>()
                    .AddAsync(medicalRecord);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MedicalRecordUpdateDto dto)
        {
            var medicalRecord = await _unitOfWork
                .Repository<MedicalRecord>()
                    .GetByIdAsync(id);

            if (medicalRecord is null)
                throw new ArgumentException("Medical record not found.");

            medicalRecord.Diagnosis = dto.Diagnosis;
            medicalRecord.Treatment = dto.Treatment;
            medicalRecord.Notes = dto.Notes;
            medicalRecord.RecordDate = dto.RecordDate;

            _unitOfWork
                .Repository<MedicalRecord>()
                    .Update(medicalRecord);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medicalRecord = await _unitOfWork
                .Repository<MedicalRecord>()
                    .GetByIdAsync(id);

            if (medicalRecord is null)
                throw new ArgumentException("Medical record not found.");

            var hasPrescriptions = await _unitOfWork
                .Repository<Prescription>()
                    .AnyAsync(p => p.MedicalRecordID == id);

            if (hasPrescriptions)
                throw new InvalidOperationException(
                    "Cannot delete a medical record that has prescriptions.");

            _unitOfWork
                .Repository<MedicalRecord>()
                    .Delete(medicalRecord);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<MedicalRecordReadDto?> BuildReadDtoAsync(MedicalRecord record)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                    .GetByIdAsync(record.AppointmentID);

            if (appointment is null)
                return null;

            var patient = await _unitOfWork
                .Repository<Patient>()
                    .GetByIdAsync(appointment.PatientID);

            var doctor = await _unitOfWork
                .Repository<Doctor>()
                    .GetByIdAsync(appointment.DoctorID);

            if (patient is null || doctor is null)
                return null;

            var patientPerson = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(patient.PersonID);

            var doctorPerson = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(doctor.PersonID);

            if (patientPerson is null || doctorPerson is null)
                return null;

            return new MedicalRecordReadDto
            {
                MedicalRecordID = record.MedicalRecordID,
                AppointmentID = record.AppointmentID,
                AppointmentDate = appointment.AppointmentDate,
                PatientName = patientPerson.Name,
                DoctorName = doctorPerson.Name,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                Notes = record.Notes,
                RecordDate = record.RecordDate
            };
        }
    }
}