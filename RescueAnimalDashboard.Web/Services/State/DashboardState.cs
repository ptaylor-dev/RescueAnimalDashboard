using MudBlazor;
using RescueAnimalDashboard.Application.Services;
using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Web.Services.Api;
using Sorting;
using System.Diagnostics;

namespace RescueAnimalDashboard.Web.Services.State
{
    public sealed class DashboardState
    {
        private readonly IAnimalApiClient _apiClient;
        private readonly MergeSorter _sorter = new();   // TODO - switch to ISorter and use dependency injection

        private List<Animal> _animals = [];
        private List<Animal> _visibleAnimals = [];

        // Uses reference based lookup
        private Dictionary<Animal, string> _searchText = new(ReferenceEqualityComparer.Instance);
        public IReadOnlyList<Animal> Animals => _animals;
        public IReadOnlyList<Animal> VisibleAnimals => _visibleAnimals;

        // Shared load task to eliminate duplicate API calls
        private Task? _loadTask;


        // Creating set methods for these properties allows for updating subscribers of change and additional logic
        public Animal? SelectedAnimal { get; private set; }
        public RescueCategory RescueCategory { get; private set; } = RescueCategory.None;
        public string SearchString { get; private set; } = String.Empty;
        public AnimalSortOption SortOption { get; private set; } = AnimalSortOption.AnimalId;
        public bool SortAscending { get; private set; } = true;
        public event Action? OnChange;

        public DashboardState(IAnimalApiClient apiClient)
        {
            _apiClient = apiClient;
        }



        public async Task LoadAsync() => _loadTask ??= LoadCoreAsync();

        private async Task LoadCoreAsync()
        {
            try
            {
                var animals = await _apiClient.GetAnimalsAsync();

                // Create a string of all searchable fields in each animal and add to a dictionary (hashtable)
                _searchText = new Dictionary<Animal, string>(animals.Count, ReferenceEqualityComparer.Instance);
                foreach (var animal in animals)
                {
                    _searchText[animal] = animal.ToString().ToLowerInvariant();
                }

                _animals = ApplySort(animals);
                ApplyFilters();
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                _loadTask = null;   // Setting to null allows a retry from another caller
                throw;
            }
        }

        public void SetSelectedAnimal(Animal? animal)
        {
            // Check to see that selection has changed first to avoid unnecessary renders
            if (SelectedAnimal == animal)
                return;

            SelectedAnimal = animal;
            NotifyStateChanged();
        }

        public void SetRescueCategory(RescueCategory category)
        {
            RescueCategory = category;
            ApplyFilters();
            NotifyStateChanged();
        }

        public void SetSearchString(string value)
        {
            SearchString = value?.Trim() ?? String.Empty;
            ApplyFilters();
            NotifyStateChanged();
        }

        public void SetSort(AnimalSortOption sortOption, bool ascending)
        {
            // If sort settings have not changed, don't change them
            if (SortOption == sortOption && SortAscending == ascending)
            {
                return;
            }

            SortOption = sortOption;
            SortAscending = ascending;

            // Sort master list once to preserve order with filter changes
            _animals = ApplySort(_animals);
            ApplyFilters();
        }

        public void ApplyFilters()
        {
            // Filter by search text and rescue criteria
            _visibleAnimals = [.. _animals
                .Where(ApplyRescueCriteria)
                .Where(ApplySearch)];
        }

        private bool ApplySearch(Animal animal)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;

            var animalIsValid = _searchText.TryGetValue(animal, out var text);

            return animalIsValid && text is not null && text.Contains(SearchString, StringComparison.OrdinalIgnoreCase);
        }

        private List<Animal> ApplySort(List<Animal> animals)
        {
            return SortOption switch
            {
                AnimalSortOption.AnimalId => _sorter.Sort(animals, a => a.AnimalId, SortAscending),
                AnimalSortOption.Type => _sorter.Sort(animals, a => a.Type, SortAscending),
                AnimalSortOption.Breed => _sorter.Sort(animals, a => a.Breed, SortAscending),
                AnimalSortOption.Color => _sorter.Sort(animals, a => a.Color, SortAscending),
                AnimalSortOption.DateOfBirth => _sorter.Sort(animals, a => a.DateOfBirth, SortAscending),
                AnimalSortOption.DateTime => _sorter.Sort(animals, a => a.DateTime, SortAscending),
                AnimalSortOption.Name => _sorter.Sort(animals, a => a.Name, SortAscending),
                AnimalSortOption.OutcomeType => _sorter.Sort(animals, a => a.OutcomeType, SortAscending),
                AnimalSortOption.OutcomeSubtype => _sorter.Sort(animals, a => a.OutcomeSubtype, SortAscending),
                AnimalSortOption.SexUponOutcome => _sorter.Sort(animals, a => a.SexUponOutcome, SortAscending),
                AnimalSortOption.AgeUponOutcomeInWeeks => _sorter.Sort(animals, a => a.AgeUponOutcomeInWeeks, SortAscending),
                _ => _sorter.Sort(animals, a => a.AnimalId, SortAscending)
            };
        }

        private bool ApplyRescueCriteria(Animal animal)
        {
            return RescueCategory switch
            {
                RescueCategory.Water => RescueClassifier.IsAWaterRescueAnimal(animal),
                RescueCategory.Mountain => RescueClassifier.IsAMountainRescueAnimal(animal),
                RescueCategory.Disaster => RescueClassifier.IsADisasterRescueAnimal(animal),
                _ => true
            };
        }

        // Notify subscribed objects of state change
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
