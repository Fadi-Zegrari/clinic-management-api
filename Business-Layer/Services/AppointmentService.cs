using Business_Layer.DTOs.Appointments;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppointmentReadDto>> GetAllAsync()
        {
            var appointments = await _unitOfWork
                .Repository<Appointment>()
                    .GetAllAsync();

            var result = new List<AppointmentReadDto>();

            foreach (var appointment in appointments)
            {
                var dto = await MapToReadDtoAsync(appointment);

                if (dto is not null)
                    result.Add(dto);
            }

            return result;
        }

        public async Task<AppointmentReadDto?> GetByIdAsync(int id)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                    .GetByIdAsync(id);

            if (appointment is null)
                return null;

            return await MapToReadDtoAsync(appointment);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<Appointment>()
                    .ExistsAsync(id);
        }

        public async Task AddAsync(AppointmentCreateDto dto)
        {
            ValidateAppointmentDate(dto.AppointmentDate);

            await ValidateReferencesAsync
            (
                dto.DoctorID,
                dto.PatientID,
                dto.AppointmentStatusID
            );

            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID,
                AppointmentStatusID = dto.AppointmentStatusID
            };

            await _unitOfWork
                .Repository<Appointment>()
                    .AddAsync(appointment);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, AppointmentUpdateDto dto)
        {
            ValidateAppointmentDate(dto.AppointmentDate);

            var appointment = await _unitOfWork
                .Repository<Appointment>()
                    .GetByIdAsync(id);

            if (appointment is null)
                throw new KeyNotFoundException(
                    $"Appointment with ID {id} was not found.");

            await ValidateReferencesAsync
            (
                dto.DoctorID,
                dto.PatientID,
                dto.AppointmentStatusID
            );

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.DoctorID = dto.DoctorID;
            appointment.PatientID = dto.PatientID;
            appointment.AppointmentStatusID = dto.AppointmentStatusID;

            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelAsync(int id)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                    .GetByIdAsync(id);

            if (appointment is null)
                throw new KeyNotFoundException(
                    $"Appointment with ID {id} was not found.");

            var canceledStatus = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetByIdAsync(4);

            if (canceledStatus is null)
                throw new InvalidOperationException(
                    "Canceled status was not found.");

            appointment.AppointmentStatusID = canceledStatus.AppointmentStatusID;

            _unitOfWork.Repository<Appointment>().Update(appointment);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ValidateReferencesAsync(int doctorId,int patientId,int statusId)
        {
            var doctorExists = await _unitOfWork
                .Repository<Doctor>()
                    .ExistsAsync(doctorId);

            if (!doctorExists)
                throw new ArgumentException(
                    $"Doctor with ID {doctorId} was not found.");

            var patientExists = await _unitOfWork
                .Repository<Patient>()
                    .ExistsAsync(patientId);

            if (!patientExists)
                throw new ArgumentException(
                    $"Patient with ID {patientId} was not found.");

            var statusExists = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .ExistsAsync(statusId);

            if (!statusExists)
                throw new ArgumentException(
                    $"Appointment status with ID {statusId} was not found.");
        }

        private static void ValidateAppointmentDate(DateTime date)
        {
            if (date == default)
                throw new ArgumentException(
                    "Appointment date must be a valid date.");

            if (date < DateTime.Now)
                throw new ArgumentException(
                    "Appointment date cannot be in the past.");
        }

        private async Task<AppointmentReadDto?> MapToReadDtoAsync(Appointment appointment)
        {
            var doctor = await _unitOfWork
                .Repository<Doctor>()
                    .GetByIdAsync(appointment.DoctorID);

            var patient = await _unitOfWork
                .Repository<Patient>()
                    .GetByIdAsync(appointment.PatientID);

            var status = await _unitOfWork
                .Repository<AppointmentStatus>()
                    .GetByIdAsync(appointment.AppointmentStatusID);

            if (doctor is null || patient is null || status is null)
                return null;

            var doctorPerson = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(doctor.PersonID);

            var patientPerson = await _unitOfWork
                .Repository<Person>()
                    .GetByIdAsync(patient.PersonID);

            if (doctorPerson is null || patientPerson is null)
                return null;

            return new AppointmentReadDto
            {
                AppointmentID = appointment.AppointmentID,
                AppointmentDate = appointment.AppointmentDate,

                DoctorID = doctor.DoctorID,
                DoctorName = doctorPerson.Name,

                PatientID = patient.PatientID,
                PatientName = patientPerson.Name,

                AppointmentStatusID = status.AppointmentStatusID,
                StatusName = status.StatusName
            };
        }
    }
}