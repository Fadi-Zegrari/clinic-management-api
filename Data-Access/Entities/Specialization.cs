using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Specialization
    {
        public int SpecializationID { get; set; }

        public string SpecializationName { get; set; } = string.Empty;
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}