using Business_Layer.DTOs.AppointmentStatuses;
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
    public class AppointmentStatusService : IAppointmentStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppointmentStatusReadDto>> GetAllAsync()
        {
            var statuses = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetAllAsync();

            return statuses.Select(MapToReadDto);
        }

        public async Task<AppointmentStatusReadDto?> GetByIdAsync(int id)
        {
            var status = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetByIdAsync(id);

            return status is null ? null : MapToReadDto(status);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<AppointmentStatus>()
                     .ExistsAsync(id);
        }

        public async Task AddAsync(AppointmentStatusCreateDto dto)
        {
            var status = new AppointmentStatus
            {
                StatusName = dto.StatusName
            };

            await _unitOfWork
                .Repository<AppointmentStatus>()
                    .AddAsync(status);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id,AppointmentStatusUpdateDto dto)
        {
            var status = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetByIdAsync(id);

            if (status is null)
                throw new KeyNotFoundException(
                    $"Appointment status with ID {id} was not found.");

            status.StatusName = dto.StatusName;

            _unitOfWork.Repository<AppointmentStatus>().Update(status);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var status = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetByIdAsync(id);

            if (status is null)
                throw new KeyNotFoundException(
                    $"Appointment status with ID {id} was not found.");

            var hasAppointments = await _unitOfWork
                .Repository<Appointment>()
                    .AnyAsync(a => a.AppointmentStatusID == id);

            if (hasAppointments)
                throw new InvalidOperationException(
                    "Cannot delete an appointment status that is in use.");

            _unitOfWork.Repository<AppointmentStatus>().Delete(status);

            await _unitOfWork.SaveChangesAsync();
        }

        private static AppointmentStatusReadDto MapToReadDto(AppointmentStatus status)
        {
            return new AppointmentStatusReadDto
            {
                AppointmentStatusID = status.AppointmentStatusID,
                StatusName = status.StatusName
            };
        }
    }
}
