using Business_Layer.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorReadDto>> GetAllAsync();

        Task<DoctorReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(DoctorCreateDto dto);

        Task UpdateAsync(int id, DoctorUpdateDto dto);

        Task DeleteAsync(int id);
    }
}
