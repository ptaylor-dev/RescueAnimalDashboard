using RescueAnimalDashboard.Domain.Interfaces;
using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Infrastructure.Utilities;

namespace RescueAnimalDashboard.Infrastructure.Repositories
{
    public class CsvAnimalRepository : IAnimalRepository
    {
        private const string CSV_PATH = "Resources\\aac_shelter_outcomes.csv";
        private readonly List<Animal> _animals = [];

        public CsvAnimalRepository()
        {
            var csvPath = Path.Combine(AppContext.BaseDirectory, CSV_PATH);
            _animals = CsvLoader.LoadAnimalsFromCsv(csvPath);
        }

        public async Task<List<Animal>> GetAllAnimalsAsync(CancellationToken ct = default)
        {
            return _animals;
        }
    }
}
