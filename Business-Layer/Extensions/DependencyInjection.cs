using Business_Layer.Interfaces;
using Business_Layer.Services;
using Data_Access.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer
               (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);

            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentStatusService, AppointmentStatusService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();

            return services;
        }
    }
}
