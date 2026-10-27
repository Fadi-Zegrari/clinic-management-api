using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Appointments
{
    public class AppointmentReadDto
    {
        public int AppointmentID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public int AppointmentStatusID { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
