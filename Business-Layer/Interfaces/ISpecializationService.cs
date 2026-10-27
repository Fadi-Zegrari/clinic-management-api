using Business_Layer.DTOs.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface ISpecializationService
    {
        Task<IEnumerable<SpecializationReadDto>> GetAllAsync();
        Task<SpecializationReadDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(SpecializationCreateDto dto);
        Task UpdateAsync(int id, SpecializationUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
