using Microsoft.EntityFrameworkCore;
using RescueAnimalDashboard.Domain.Interfaces;
using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Infrastructure.Persistence.Context;

namespace RescueAnimalDashboard.Infrastructure.Repositories
{
    internal class EfAnimalRepository(RescueAnimalDbContext db) : IAnimalRepository
    {
        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken ct = default) =>
            await db.Animals
                .AsNoTracking()
                .Select(a => new Animal(
                    a.AnimalId,
                    a.AnimalType.Name,
                    a.Breed,
                    a.Color,
                    a.DateOfBirth,
                    a.DateTime,
                    a.Name,
                    a.OutcomeType != null ? a.OutcomeType.Name : null,
                    a.OutcomeSubtype,
                    a.SexUponOutcome != null ? a.SexUponOutcome.Name : null,
                    a.Latitude,
                    a.Longitude,
                    a.AgeUponOutcome,
                    a.AgeUponOutcomeInWeeks))
                .ToListAsync(ct);
    }
}
