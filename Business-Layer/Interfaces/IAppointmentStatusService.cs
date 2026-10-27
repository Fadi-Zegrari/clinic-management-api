using Business_Layer.DTOs.AppointmentStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IAppointmentStatusService
    {
        Task<IEnumerable<AppointmentStatusReadDto>> GetAllAsync();

        Task<AppointmentStatusReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(AppointmentStatusCreateDto dto);

        Task UpdateAsync(int id, AppointmentStatusUpdateDto dto);

        Task DeleteAsync(int id);
    }
}