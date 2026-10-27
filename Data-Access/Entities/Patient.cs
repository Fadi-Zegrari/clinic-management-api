using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Patient
    {
        public int PatientID { get; set; }

        public int PersonID { get; set; }

        // Navigation Property
        public Person Person { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}