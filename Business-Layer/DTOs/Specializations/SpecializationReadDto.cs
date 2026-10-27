using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Specializations
{
    public class SpecializationReadDto
    {
        public int SpecializationID { get; set; }
        public string SpecializationName { get; set; } = string.Empty;
    }
}
