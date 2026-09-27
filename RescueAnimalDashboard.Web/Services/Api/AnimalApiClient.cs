using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Web.Services.Api
{
    public class AnimalApiClient(HttpClient httpClient) : IAnimalApiClient
    {
        public async Task<List<Animal>> GetAnimalsAsync(CancellationToken ct = default)
        {
            return await httpClient.GetFromJsonAsync<List<Animal>>("animals", ct) ?? [];
        }
    }
}
