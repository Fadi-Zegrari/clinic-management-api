using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class MedicalRecord
    {
        public int MedicalRecordID { get; set; }

        public int AppointmentID { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Treatment { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public DateTime RecordDate { get; set; }

        // Navigation Property
        public Appointment Appointment { get; set; } = null!;
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    }
}
