using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }

        public int PersonID { get; set; }

        public int SpecializationID { get; set; }

        // Navigation Properties
        public Person Person { get; set; } = null!;
        public Specialization Specialization { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; }
               = new List<Appointment>();
    }
}