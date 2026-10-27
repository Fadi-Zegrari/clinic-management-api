using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Prescriptions
{
    public class PrescriptionReadDto
    {
        public int PrescriptionID { get; set; }

        public int MedicalRecordID { get; set; }

        public string MedicationName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;
    }
}
