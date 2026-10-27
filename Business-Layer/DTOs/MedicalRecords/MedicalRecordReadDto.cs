using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.MedicalRecords
{
    public class MedicalRecordReadDto
    {
        public int MedicalRecordID { get; set; }

        public int AppointmentID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Treatment { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public DateTime RecordDate { get; set; }
    }
}