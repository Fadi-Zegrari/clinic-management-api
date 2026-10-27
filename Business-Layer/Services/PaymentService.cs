using Business_Layer.DTOs.Payments;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PaymentReadDto>> GetAllAsync()
        {
            var payments = await _unitOfWork
                .Repository<Payment>()
                    .GetAllAsync();

            var result = new List<PaymentReadDto>();

            foreach (var payment in payments)
            {
                var appointment = await _unitOfWork
                    .Repository<Appointment>()
                        .GetByIdAsync(payment.AppointmentID);

                var paymentMethod = await _unitOfWork
                    .Repository<PaymentMethod>()
                        .GetByIdAsync(payment.PaymentMethodID);

                if (appointment is null || paymentMethod is null)
                    continue;

                var patient = await _unitOfWork
                    .Repository<Patient>()
                        .GetByIdAsync(appointment.PatientID);

                var doctor = await _unitOfWork
                    .Repository<Doctor>()
                        .GetByIdAsync(appointment.DoctorID);

                if (patient is null || doctor is null)
                    continue;

                var patientPerson = await _unitOfWork
                    .Repository<Person>()
                        .GetByIdAsync(patient.PersonID);

                var doctorPerson = await _unitOfWork
                    .Repository<Person>()
                        .GetByIdAsync(doctor.PersonID);

                if (patientPerson is null || doctorPerson is null)
                    continue;

                result.Add(MapToReadDto
                    (
                    payment,
                    appointment,
                    patientPerson,
                    doctorPerson,
                    paymentMethod
                    ));
            }

            return result;
        }

        public async Task<PaymentReadDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork
                .Repository<Payment>()
                .GetByIdAsync(id);

            if (payment is null)
                return null;

            var appointment = await _unitOfWork
                .Repository<Appointment>()
                .GetByIdAsync(payment.AppointmentID);

            var paymentMethod = await _unitOfWork
                .Repository<PaymentMethod>()
                .GetByIdAsync(payment.PaymentMethodID);

            if (appointment is null || paymentMethod is null)
                return null;

            var patient = await _unitOfWork
                .Repository<Patient>()
                .GetByIdAsync(appointment.PatientID);

            var doctor = await _unitOfWork
                .Repository<Doctor>()
                .GetByIdAsync(appointment.DoctorID);

            if (patient is null || doctor is null)
                return null;

            var patientPerson = await _unitOfWork
                .Repository<Person>()
                .GetByIdAsync(patient.PersonID);

            var doctorPerson = await _unitOfWork
                .Repository<Person>()
                .GetByIdAsync(doctor.PersonID);

            if (patientPerson is null || doctorPerson is null)
                return null;

            return MapToReadDto(
                payment,
                appointment,
                patientPerson,
                doctorPerson,
                paymentMethod);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork
                .Repository<Payment>()
                .ExistsAsync(id);
        }

        public async Task AddAsync(PaymentCreateDto dto)
        {
            var appointment = await _unitOfWork
                .Repository<Appointment>()
                .GetByIdAsync(dto.AppointmentID);

            if (appointment is null)
                throw new ArgumentException("Appointment not found.");

            if (appointment.AppointmentStatusID == 4)
                throw new InvalidOperationException(
                    "Cannot register a payment for a canceled appointment.");

            var paymentMethodExists = await _unitOfWork
                .Repository<PaymentMethod>()
                .ExistsAsync(dto.PaymentMethodID);

            if (!paymentMethodExists)
                throw new ArgumentException("Payment method not found.");

            var paymentAlreadyExists = await _unitOfWork
                .Repository<Payment>()
                .AnyAsync(p => p.AppointmentID == dto.AppointmentID);

            if (paymentAlreadyExists)
                throw new InvalidOperationException(
                    "A payment has already been registered for this appointment.");

            var payment = new Payment
            {
                AppointmentID = dto.AppointmentID,
                PaymentMethodID = dto.PaymentMethodID,
                Amount = dto.Amount,
                PaymentDate = dto.PaymentDate
            };

            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        private static PaymentReadDto MapToReadDto
            (Payment payment, Appointment appointment, Person patientPerson, Person doctorPerson, PaymentMethod paymentMethod)
        {
            return new PaymentReadDto
            {
                PaymentID = payment.PaymentID,
                AppointmentID = payment.AppointmentID,
                AppointmentDate = appointment.AppointmentDate,
                PatientName = patientPerson.Name,
                DoctorName = doctorPerson.Name,
                PaymentMethodID = payment.PaymentMethodID,
                PaymentMethodName = paymentMethod.MethodName,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}