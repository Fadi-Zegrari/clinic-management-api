using Business_Layer.DTOs.Patients;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientReadDto>> GetAllAsync();

        Task<PatientReadDto?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(PatientCreateDto patientDto);

        Task UpdateAsync(int id, PatientUpdateDto patientDto);

        Task DeleteAsync(int id);
    }
}