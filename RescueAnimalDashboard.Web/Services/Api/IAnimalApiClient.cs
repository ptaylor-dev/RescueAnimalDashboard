using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Web.Services.Api
{
    public interface IAnimalApiClient
    {
        Task<IReadOnlyList<Animal>> GetAnimalsAsync(CancellationToken ct);
    }
}
