using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
