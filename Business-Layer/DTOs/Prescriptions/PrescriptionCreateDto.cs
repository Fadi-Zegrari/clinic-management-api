using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Prescriptions
{
    public class PrescriptionCreateDto
    {
        [Range(1, int.MaxValue)]
        public int MedicalRecordID { get; set; }

        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Dosage { get; set; } = string.Empty;

        [StringLength(500)]
        public string Instructions { get; set; } = string.Empty;
    }
}
