using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RescueAnimalDashboard.Application.Interfaces;
using RescueAnimalDashboard.Infrastructure.Persistence.Context;
using RescueAnimalDashboard.Infrastructure.Repositories;
using Sorting;

namespace RescueAnimalDashboard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<RescueAnimalDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());   // PostgreSQL standard naming convention

            // Use either Csv or Ef repository
            //services.AddScoped<IAnimalRepository, CsvAnimalRepository>();
            services.AddScoped<IAnimalRepository, EfAnimalRepository>();

            services.AddScoped<ISorter, MergeSorter>();

            return services;
        }
    }
}
