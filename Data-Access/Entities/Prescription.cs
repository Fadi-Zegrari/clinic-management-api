using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }

        public int MedicalRecordID { get; set; }

        public string MedicationName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;

        // Navigation Property
        public MedicalRecord MedicalRecord { get; set; } = null!;
    }
}
