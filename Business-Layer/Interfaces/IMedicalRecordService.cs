using Business_Layer.DTOs.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordReadDto>> GetAllAsync();

        Task<MedicalRecordReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(MedicalRecordCreateDto dto);

        Task UpdateAsync(int id, MedicalRecordUpdateDto dto);

        Task DeleteAsync(int id);
    }
}