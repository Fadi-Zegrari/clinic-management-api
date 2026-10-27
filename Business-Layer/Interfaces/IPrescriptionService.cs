using Business_Layer.DTOs.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionReadDto>> GetAllAsync();

        Task<PrescriptionReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(PrescriptionCreateDto dto);

        Task UpdateAsync(int id, PrescriptionUpdateDto dto);

        Task DeleteAsync(int id);
    }
}