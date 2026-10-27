using Business_Layer.DTOs.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Business_Layer.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentReadDto>> GetAllAsync();

        Task<AppointmentReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(AppointmentCreateDto dto);

        Task UpdateAsync(int id, AppointmentUpdateDto dto);

        Task CancelAsync(int id);
    }
}