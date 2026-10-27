using Business_Layer.DTOs.Specializations;
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
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SpecializationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SpecializationReadDto>> GetAllAsync()
        {
            var specializations = await _unitOfWork
                .Repository<Specialization>()
                    .GetAllAsync();

            return specializations.Select(s => MapToReadDto(s));
        }

        public async Task<SpecializationReadDto?> GetByIdAsync(int id)
        {
            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(id);

            return specialization is null ? null : MapToReadDto(specialization);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<Specialization>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(SpecializationCreateDto dto)
        {
            var specialization = new Specialization
            {
                SpecializationName = dto.SpecializationName
            };

            await _unitOfWork.Repository<Specialization>()
                    .AddAsync(specialization);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, SpecializationUpdateDto dto)
        {
            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(id);

            if (specialization is null)
                throw new KeyNotFoundException
                    ($"Specialization with ID {id} was not found.");

            specialization.SpecializationName = dto.SpecializationName;

            _unitOfWork.Repository<Specialization>()
                .Update(specialization);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(id);

            if (specialization is null)
                throw new KeyNotFoundException(
                    $"Specialization with ID {id} was not found.");

            _unitOfWork.Repository<Specialization>()
                .Delete(specialization);

            await _unitOfWork.SaveChangesAsync();
        }

        private static SpecializationReadDto MapToReadDto(Specialization specialization)
        {
            return new SpecializationReadDto
            {
                SpecializationID = specialization.SpecializationID,
                SpecializationName = specialization.SpecializationName
            };
        }
    }
}