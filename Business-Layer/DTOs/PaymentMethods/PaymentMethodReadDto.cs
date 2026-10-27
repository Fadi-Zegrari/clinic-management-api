using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.PaymentMethods
{
    public class PaymentMethodReadDto
    {
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; } = string.Empty;
    }
}