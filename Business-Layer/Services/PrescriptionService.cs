using Business_Layer.DTOs.Prescriptions;
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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PrescriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PrescriptionReadDto>> GetAllAsync()
        {
            var prescriptions = await _unitOfWork
                .Repository<Prescription>()
                    .GetAllAsync();

            return prescriptions.Select(MapToReadDto);
        }

        public async Task<PrescriptionReadDto?> GetByIdAsync(int id)
        {
            var prescription = await _unitOfWork
                .Repository<Prescription>()
                    .GetByIdAsync(id);

            if (prescription is null)
                return null;

            return MapToReadDto(prescription);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<Prescription>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(PrescriptionCreateDto dto)
        {
            var medicalRecordExists = await _unitOfWork
                .Repository<MedicalRecord>()
                    .ExistsAsync(dto.MedicalRecordID);

            if (!medicalRecordExists)
                throw new ArgumentException("Medical record not found.");

            var prescription = new Prescription
            {
                MedicalRecordID = dto.MedicalRecordID,
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                Instructions = dto.Instructions
            };

            await _unitOfWork
                .Repository<Prescription>()
                    .AddAsync(prescription);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PrescriptionUpdateDto dto)
        {
            var prescription = await _unitOfWork
                .Repository<Prescription>()
                    .GetByIdAsync(id);

            if (prescription is null)
                throw new ArgumentException("Prescription not found.");

            prescription.MedicationName = dto.MedicationName;
            prescription.Dosage = dto.Dosage;
            prescription.Instructions = dto.Instructions;

            _unitOfWork
                .Repository<Prescription>()
                    .Update(prescription);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prescription = await _unitOfWork
                .Repository<Prescription>()
                    .GetByIdAsync(id);

            if (prescription is null)
                throw new ArgumentException("Prescription not found.");

            _unitOfWork
                .Repository<Prescription>()
                    .Delete(prescription);

            await _unitOfWork.SaveChangesAsync();
        }

        private static PrescriptionReadDto MapToReadDto(Prescription prescription)
        {
            return new PrescriptionReadDto
            {
                PrescriptionID = prescription.PrescriptionID,
                MedicalRecordID = prescription.MedicalRecordID,
                MedicationName = prescription.MedicationName,
                Dosage = prescription.Dosage,
                Instructions = prescription.Instructions
            };
        }
    }
}