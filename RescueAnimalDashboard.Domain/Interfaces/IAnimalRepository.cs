using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Domain.Interfaces
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAnimalsAsync(CancellationToken ct);
    }
}
