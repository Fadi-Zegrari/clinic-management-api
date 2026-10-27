using Business_Layer.DTOs.Patients;
using Business_Layer.Interfaces;
using Data_Access.Entities;
using Data_Access.Repositories;
using System.Runtime.CompilerServices;
namespace Business_Layer.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<PatientReadDto>> GetAllAsync()
        {
            var patients = await _unitOfWork
                .Repository<Patient>()
                .GetAllAsync();

            var result = new List<PatientReadDto>();

            foreach (var patient in patients)
            {
                var person = await _unitOfWork
                    .Repository<Person>()
                    .GetByIdAsync(patient.PersonID);

                if (person is null)
                    continue;

                result.Add(MapToReadDto(patient, person));
            }

            return result;
        }

        public async Task<PatientReadDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
               throw new ArgumentException(
                      "Patient ID must be greater than zero.", nameof(id));

            var patient = await _unitOfWork
                .Repository<Patient>()
                .GetByIdAsync(id);

            if (patient is null)
                return null;

            var person = await _unitOfWork
                .Repository<Person>()
                .GetByIdAsync(patient.PersonID);

            if (person is null)
                return null;

            return MapToReadDto(patient, person);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _unitOfWork
                .Repository<Patient>()
                .ExistsAsync(id);
        }

        public async Task AddAsync(PatientCreateDto patientDto)
        {
            ArgumentNullException.ThrowIfNull(patientDto);

            var person = new Person
            {
                Name = patientDto.Name,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                Phone = patientDto.Phone,
                Address = patientDto.Address,
                Email = patientDto.Email
            };

            await _unitOfWork.Repository<Person>().AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            var patient = new Patient
            {
                PersonID = person.PersonID
            };

            await _unitOfWork.Repository<Patient>().AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, PatientUpdateDto patientDto)
        {
            ArgumentNullException.ThrowIfNull(patientDto);

            if (id <= 0)
                throw new ArgumentException(
                    "Patient ID must be greater than zero.", nameof(id));

            var patient = await _unitOfWork
                .Repository<Patient>()
                .GetByIdAsync(id);

            if (patient is null)
                throw new KeyNotFoundException(
                    $"Patient with ID {id} was not found.");

            var person = await _unitOfWork
                .Repository<Person>()
                .GetByIdAsync(patient.PersonID);

            if (person is null)
                throw new KeyNotFoundException(
                    $"Person with ID {patient.PersonID} was not found.");

            person.Name = patientDto.Name;
            person.DateOfBirth = patientDto.DateOfBirth;
            person.Gender = patientDto.Gender;
            person.Phone = patientDto.Phone;
            person.Address = patientDto.Address;
            person.Email = patientDto.Email;

            _unitOfWork.Repository<Person>().Update(person);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Patient ID must be greater than zero.",
                         nameof(id));

            var patient = await _unitOfWork
                .Repository<Patient>()
                    .GetByIdAsync(id);

            if (patient is null)
                throw new KeyNotFoundException(
                    $"Patient with ID {id} was not found.");

            var hasAppointments = await _unitOfWork
                .Repository<Appointment>()
                    .AnyAsync(a => a.PatientID == id);

            if (hasAppointments)
                throw new InvalidOperationException(
                    "Cannot delete this patient because they have appointments.");

            var person = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(patient.PersonID);

            if (person is null)
                throw new KeyNotFoundException(
                    $"Person with ID {patient.PersonID} was not found.");

            _unitOfWork.Repository<Patient>().Delete(patient);
            _unitOfWork.Repository<Person>().Delete(person);

            await _unitOfWork.SaveChangesAsync();
        }

        private static PatientReadDto MapToReadDto(
            Patient patient, Person person)
        {
            return new PatientReadDto
            {
                PatientID = patient.PatientID,
                Name = person.Name,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Phone = person.Phone,
                Address = person.Address,
                Email = person.Email
            };
        }
    }
}