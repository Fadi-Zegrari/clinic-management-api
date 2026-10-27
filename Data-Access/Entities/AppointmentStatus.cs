using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class AppointmentStatus
    {
        public int AppointmentStatusID { get; set; }

        public string StatusName { get; set; } = string.Empty;
        public ICollection<Appointment> Appointments { get; set; }
               = new List<Appointment>();
    }
}
