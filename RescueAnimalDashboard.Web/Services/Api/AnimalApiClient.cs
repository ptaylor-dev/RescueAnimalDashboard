using RescueAnimalDashboard.Domain.ValueObjects;
using System.Net.Http.Json;

namespace RescueAnimalDashboard.Web.Services.Api
{
    public class AnimalApiClient(HttpClient httpClient) : IAnimalApiClient
    {
        public async Task<List<Animal>> GetAnimalsAsync(CancellationToken ct = default)
        {
            return await httpClient.GetFromJsonAsync("animals", AppJsonContext.Default.ListAnimal, ct) ?? [];
        }
    }
}
