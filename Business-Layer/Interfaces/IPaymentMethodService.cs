using Business_Layer.DTOs.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodReadDto>> GetAllAsync();

        Task<PaymentMethodReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(PaymentMethodCreateDto dto);

        Task UpdateAsync(int id, PaymentMethodUpdateDto dto);

        Task DeleteAsync(int id);
    }
}