using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RescueAnimalDashboard.Domain.Interfaces;
using RescueAnimalDashboard.Infrastructure.Persistence.Context;
using RescueAnimalDashboard.Infrastructure.Repositories;

namespace RescueAnimalDashboard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RescueAnimalDbContext>(options =>
                options.UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());   // PostgreSQL standard naming convention

            services.AddScoped<IAnimalRepository, CsvAnimalRepository>();

            return services;
        }
    }
}
