using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.PaymentMethods
{
    public class PaymentMethodUpdateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string MethodName { get; set; } = string.Empty;
    }
}