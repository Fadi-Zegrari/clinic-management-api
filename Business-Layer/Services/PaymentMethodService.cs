using Business_Layer.DTOs.PaymentMethods;
using Business_Layer.Interfaces;
using Data_Access.Entities;
using Data_Access.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentMethodReadDto>> GetAllAsync()
        {
            var paymentMethods = await _unitOfWork
                .Repository<PaymentMethod>()
                    .GetAllAsync();

            return paymentMethods.Select(MapToReadDto);
        }

        public async Task<PaymentMethodReadDto?> GetByIdAsync(int id)
        {
            var paymentMethod = await _unitOfWork
                .Repository<PaymentMethod>()
                    .GetByIdAsync(id);

            return paymentMethod is null? null : MapToReadDto(paymentMethod);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<PaymentMethod>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(PaymentMethodCreateDto dto)
        {
            var paymentMethod = new PaymentMethod
            {
                MethodName = dto.MethodName.Trim()
            };

            await _unitOfWork
                .Repository<PaymentMethod>()
                    .AddAsync(paymentMethod);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PaymentMethodUpdateDto dto)
        {
            var paymentMethod = await _unitOfWork
                .Repository<PaymentMethod>()
                    .GetByIdAsync(id);

            if (paymentMethod is null)
                throw new KeyNotFoundException(
                    $"Payment method with ID {id} was not found.");

            paymentMethod.MethodName = dto.MethodName.Trim();

            _unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var paymentMethod = await _unitOfWork
                .Repository<PaymentMethod>()
                    .GetByIdAsync(id);

            if (paymentMethod is null)
                throw new KeyNotFoundException
                    ($"Payment method with ID {id} was not found.");

            var hasPayments = await _unitOfWork
                .Repository<Payment>()
                    .AnyAsync(p => p.PaymentMethodID == id);

            if (hasPayments)
                throw new InvalidOperationException(
                    "Cannot delete a payment method that is used by existing payments.");

            _unitOfWork.Repository<PaymentMethod>().Delete(paymentMethod);

            await _unitOfWork.SaveChangesAsync();
        }

        private static PaymentMethodReadDto MapToReadDto(PaymentMethod paymentMethod)
        {
            return new PaymentMethodReadDto
            {
                PaymentMethodID = paymentMethod.PaymentMethodID,
                MethodName = paymentMethod.MethodName
            };
        }
    }
}