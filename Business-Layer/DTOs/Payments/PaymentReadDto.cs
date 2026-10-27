using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Payments
{
    public class PaymentReadDto
    {
        public int PaymentID { get; set; }

        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;

        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}