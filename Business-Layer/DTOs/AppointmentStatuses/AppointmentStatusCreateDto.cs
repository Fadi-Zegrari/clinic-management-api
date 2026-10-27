using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.AppointmentStatuses
{
    public class AppointmentStatusCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string StatusName { get; set; } = string.Empty;
    }
}
