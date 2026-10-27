using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Payments
{
    public class PaymentCreateDto
    {
        [Range(1, int.MaxValue)]
        public int AppointmentID { get; set; }

        [Range(1, int.MaxValue)]
        public int PaymentMethodID { get; set; }

        [Range(typeof(decimal), "0.01", "10000000")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}