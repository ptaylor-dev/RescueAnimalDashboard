using RescueAnimalDashboard.Application.Models;

namespace RescueAnimalDashboard.Web.Services.Api
{
    public interface IAnimalApiClient
    {
        Task<AnimalResponse> GetAnimalsAsync(
            AnimalQuery query,
            CancellationToken ct = default);
    }
}
