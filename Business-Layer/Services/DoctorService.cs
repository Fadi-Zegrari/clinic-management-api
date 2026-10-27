using Business_Layer.DTOs.Doctors;
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
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorReadDto>> GetAllAsync()
        {
            var doctors = await _unitOfWork
                .Repository<Doctor>()
                    .GetAllAsync();

            var result = new List<DoctorReadDto>();

            foreach (var doctor in doctors)
            {
                var person = await _unitOfWork
                    .Repository<Person>()
                        .GetByIdAsync(doctor.PersonID);

                var specialization = await _unitOfWork
                    .Repository<Specialization>()
                        .GetByIdAsync(doctor.SpecializationID);

                if (person is null || specialization is null)
                    continue;

                result.Add(MapToReadDto(doctor, person, specialization));
            }

            return result;
        }

        public async Task<DoctorReadDto?> GetByIdAsync(int id)
        {
            var doctor = await _unitOfWork
                .Repository<Doctor>()
                    .GetByIdAsync(id);

            if (doctor is null)
                return null;

            var person = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(doctor.PersonID);

            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(doctor.SpecializationID);

            if (person is null || specialization is null)
                return null;

            return MapToReadDto(doctor, person, specialization);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<Doctor>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(DoctorCreateDto dto)
        {
            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(dto.SpecializationID);

            if (specialization is null)
                throw new InvalidOperationException(
                    "The specified specialization does not exist.");

            var person = new Person
            {
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Address = dto.Address,
                Email = dto.Email
            };

            await _unitOfWork
                .Repository<Person>()
                    .AddAsync(person);

            await _unitOfWork.SaveChangesAsync();

            var doctor = new Doctor
            {
                PersonID = person.PersonID,
                SpecializationID = dto.SpecializationID
            };

            await _unitOfWork
                .Repository<Doctor>()
                    .AddAsync(doctor);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, DoctorUpdateDto dto)
        {
            var doctor = await _unitOfWork
                .Repository<Doctor>()
                    .GetByIdAsync(id);

            if (doctor is null)
                throw new KeyNotFoundException(
                    $"Doctor with ID {id} was not found.");

            var person = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(doctor.PersonID);

            if (person is null)
                throw new InvalidOperationException(
                    "The doctor's person record was not found.");

            var specialization = await _unitOfWork
                .Repository<Specialization>()
                    .GetByIdAsync(dto.SpecializationID);

            if (specialization is null)
                throw new InvalidOperationException(
                    "The specified specialization does not exist.");

            person.Name = dto.Name;
            person.DateOfBirth = dto.DateOfBirth;
            person.Gender = dto.Gender;
            person.Phone = dto.Phone;
            person.Address = dto.Address;
            person.Email = dto.Email;

            doctor.SpecializationID = dto.SpecializationID;

            _unitOfWork.Repository<Person>().Update(person);
            _unitOfWork.Repository<Doctor>().Update(doctor);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _unitOfWork
                .Repository<Doctor>()
                    .GetByIdAsync(id);

            if (doctor is null)
                throw new KeyNotFoundException(
                    $"Doctor with ID {id} was not found.");

            var hasAppointments = await _unitOfWork
                .Repository<Appointment>()
                    .AnyAsync(a => a.DoctorID == id);

            if (hasAppointments)
                throw new InvalidOperationException(
                    "Cannot delete a doctor who has appointments.");

            var person = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(doctor.PersonID);

            if (person is null)
                throw new InvalidOperationException(
                    "The doctor's person record was not found.");

            _unitOfWork.Repository<Doctor>().Delete(doctor);
            _unitOfWork.Repository<Person>().Delete(person);

            await _unitOfWork.SaveChangesAsync();
        }

        private static DoctorReadDto MapToReadDto(Doctor doctor, Person person, Specialization specialization)
        {
            return new DoctorReadDto
            {
                DoctorID = doctor.DoctorID,
                Name = person.Name,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Phone = person.Phone,
                Address = person.Address,
                Email = person.Email,
                SpecializationID = specialization.SpecializationID,
                SpecializationName = specialization.SpecializationName
            };
        }
    }
}