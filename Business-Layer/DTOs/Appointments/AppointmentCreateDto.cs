using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Appointments
{
    public class AppointmentCreateDto
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Range(1, int.MaxValue)]
        public int DoctorID { get; set; }

        [Range(1, int.MaxValue)]
        public int PatientID { get; set; }

        [Range(1, int.MaxValue)]
        public int AppointmentStatusID { get; set; }
    }
}