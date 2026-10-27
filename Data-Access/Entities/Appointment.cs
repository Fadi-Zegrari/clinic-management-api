using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{

    public class Appointment
    {
        public int AppointmentID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int DoctorID { get; set; }

        public int PatientID { get; set; }

        public int AppointmentStatusID { get; set; }

        // Navigation Properties
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public AppointmentStatus AppointmentStatus { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    }
}
