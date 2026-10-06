using RescueAnimalDashboard.Application.Interfaces;
using RescueAnimalDashboard.Application.Models;
using RescueAnimalDashboard.Domain.Models;
using RescueAnimalDashboard.Domain.Services;
using RescueAnimalDashboard.Infrastructure.Utilities;
using Sorting;

namespace RescueAnimalDashboard.Infrastructure.Repositories
{
    public class CsvAnimalRepository : IAnimalRepository
    {
        private const string CSV_PATH = "Resources\\aac_shelter_outcomes.csv";
        private readonly ISorter _sorter;

        private readonly List<Animal> _animals = [];

        // Uses reference based lookup
        private readonly Dictionary<Animal, string> _searchText = new(ReferenceEqualityComparer.Instance);


        public CsvAnimalRepository(ISorter sorter)
        {
            _sorter = sorter;

            var csvPath = Path.Combine(AppContext.BaseDirectory, CSV_PATH);
            _animals = CsvLoader.LoadAnimalsFromCsv(csvPath);

            // Create a string of all searchable fields in each animal and add to a dictionary (hashtable)
            _searchText = new Dictionary<Animal, string>(_animals.Count, ReferenceEqualityComparer.Instance);
            foreach (var animal in _animals)
            {
                _searchText[animal] = animal.ToString().ToLowerInvariant();
            }
        }

        public async Task<AnimalResponse> GetAnimalsAsync(
            AnimalQuery aQuery, CancellationToken ct = default)
        {
            var searchString = aQuery.SearchText ?? String.Empty;
            var rescueCategory = aQuery.RescueType ?? RescueCategory.None;

            IEnumerable<Animal> query = _animals;

            // Filter by category and search text first
            List<Animal> filteredAnimals = [.. query
                .Where(animal => ApplyRescueCriteria(animal, rescueCategory))
                .Where(animal => ApplySearch(animal, searchString))];

            // Sort filtered animals
            var sortedAnimals = ApplySorting(filteredAnimals, aQuery.SortBy, aQuery.SortBySecondary, aQuery.SortAscending);

            var totalCount = sortedAnimals.Count;

            var chartBreeds = GetBreedCounts(filteredAnimals, aQuery.BreedCountLimit);

            var animalPage = sortedAnimals
                .Skip(aQuery.PageIndex * aQuery.PageSize)
                .Take(aQuery.PageSize)
                .ToList();

            // Ceiling divisions accounts for page that is less than full
            int pageCount = (int)Math.Ceiling(totalCount / (double)aQuery.PageSize);

            var response = new AnimalResponse(
                animalPage,
                aQuery.PageIndex,
                aQuery.PageSize,
                totalCount,
                pageCount,
                chartBreeds);

            return response;
        }

        private bool ApplySearch(Animal animal, string? searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            var animalIsValid = _searchText.TryGetValue(animal, out var text);

            return animalIsValid && text is not null && text.Contains(searchString, StringComparison.OrdinalIgnoreCase);
        }

        private List<Animal> ApplySorting(
            List<Animal> animals,
            AnimalSortOption? sortBy,
            AnimalSortOption? sortBySecondary,
            bool sortAscending = true)
        {
            var primarySort = sortBy ?? AnimalSortOption.AnimalId;

            // Use a new copy of the list for sorting
            var sorted = animals.ToList();

            // Sort by secondary firste
            if (sortBySecondary is not null && sortBySecondary != sortBy)
            {
                sorted = ApplySort(sorted, sortBySecondary, sortAscending);
            }

            // Apply primary sort
            return ApplySort(sorted, primarySort, sortAscending);
        }

        private List<Animal> ApplySort(List<Animal> animals, AnimalSortOption? sortOption, bool sortAscending)
        {
            // Default sort option if null
            sortOption ??= AnimalSortOption.AnimalId;

            return sortOption switch
            {
                AnimalSortOption.AnimalId => _sorter.Sort(animals, a => a.AnimalId, sortAscending),
                AnimalSortOption.Type => _sorter.Sort(animals, a => a.Type, sortAscending),
                AnimalSortOption.Breed => _sorter.Sort(animals, a => a.Breed, sortAscending),
                AnimalSortOption.Color => _sorter.Sort(animals, a => a.Color, sortAscending),
                AnimalSortOption.DateOfBirth => _sorter.Sort(animals, a => a.DateOfBirth, sortAscending),
                AnimalSortOption.DateTime => _sorter.Sort(animals, a => a.DateTime, sortAscending),
                AnimalSortOption.Name => _sorter.Sort(animals, a => a.Name, sortAscending),
                AnimalSortOption.OutcomeType => _sorter.Sort(animals, a => a.OutcomeType, sortAscending),
                AnimalSortOption.OutcomeSubtype => _sorter.Sort(animals, a => a.OutcomeSubtype, sortAscending),
                AnimalSortOption.SexUponOutcome => _sorter.Sort(animals, a => a.SexUponOutcome, sortAscending),
                AnimalSortOption.AgeUponOutcomeInWeeks => _sorter.Sort(animals, a => a.AgeUponOutcomeInWeeks, sortAscending),
                _ => _sorter.Sort(animals, a => a.AnimalId, sortAscending)
            };
        }

        private static List<BreedCount> GetBreedCounts(List<Animal> animals, int breedCountLimit)
        {
            var breedCounts = animals
                .GroupBy(a => a.Breed)
                .Select(g => new
                {
                    Breed = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(group => group.Count)
                .ThenBy(group => group.Breed)
                .ToList();

            // Create list of structs from grouped results
            return [.. breedCounts
                .Select(g => new BreedCount(
                    g.Breed ?? "Unknown",   // Unknown if breed label is null
                    g.Count))
                .Take(breedCountLimit)];
        }

        private static bool ApplyRescueCriteria(Animal animal, RescueCategory rescueCategory)
        {
            return RescueClassifier.IsRescueAnimal(animal, rescueCategory);
        }
    }
}
