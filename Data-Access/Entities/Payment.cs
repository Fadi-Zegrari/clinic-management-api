using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }

        public int AppointmentID { get; set; }

        public int PaymentMethodID { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        // Navigation Properties
        public Appointment Appointment { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
