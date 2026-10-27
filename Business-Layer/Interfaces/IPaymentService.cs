using Business_Layer.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentReadDto>> GetAllAsync();

        Task<PaymentReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(PaymentCreateDto dto);
    }
}
