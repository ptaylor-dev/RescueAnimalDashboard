using RescueAnimalDashboard.Application.Models;

namespace RescueAnimalDashboard.Application.Interfaces
{
    public interface IAnimalRepository
    {
        Task<AnimalResponse> GetAnimalsAsync(AnimalQuery animalQuery, CancellationToken ct = default);
    }
}
