using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.AppointmentStatuses
{
    public class AppointmentStatusReadDto
    {
        public int AppointmentStatusID { get; set; }

        public string StatusName { get; set; } = string.Empty;
    }
}