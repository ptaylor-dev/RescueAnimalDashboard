using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Web.Services.Api;

namespace RescueAnimalDashboard.Web.Services.State
{
    public sealed class AppState
    {
        private readonly IAnimalApiClient _apiClient;
        private IReadOnlyList<Animal> _animals = [];

        public IReadOnlyList<Animal> FilteredAnimals = [];
        public Animal? SelectedAnimal { get; set; }
        public string SearchString = "";

        public AppState(IAnimalApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (_animals.Count > 0)
            {
                return;
            }

            try
            {
                _animals = await _apiClient.GetAnimalsAsync(ct);
                FilteredAnimals = _animals;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load animals from API server");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
