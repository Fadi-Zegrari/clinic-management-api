using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Specializations
{
    public class SpecializationCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string SpecializationName { get; set; } = string.Empty;
    }
}
