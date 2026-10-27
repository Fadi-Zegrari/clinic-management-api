using Data_Access.Data;
using Data_Access.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess
               (this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ClinicConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
