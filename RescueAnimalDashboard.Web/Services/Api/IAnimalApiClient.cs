using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Web.Services.Api
{
    public interface IAnimalApiClient
    {
        Task<List<Animal>> GetAnimalsAsync(CancellationToken ct = default);
    }
}
