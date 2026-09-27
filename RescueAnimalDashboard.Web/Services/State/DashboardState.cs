using MudBlazor;
using RescueAnimalDashboard.Application.Services;
using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Web.Services.Api;
using Sorting;

namespace RescueAnimalDashboard.Web.Services.State
{
    public sealed class DashboardState
    {
        private readonly IAnimalApiClient _apiClient;
        private readonly MergeSorter _sorter = new();   // TODO - switch to ISorter and use dependency injection

        private List<Animal> _animals = [];
        private List<Animal> _visibleAnimals = [];
        public IReadOnlyList<Animal> Animals => _animals;
        public IReadOnlyList<Animal> VisibleAnimals => _visibleAnimals;


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

        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (Animals.Count > 0)
            {
                return;
            }

            _animals = await _apiClient.GetAnimalsAsync(ct);

            UpdateVisableAnimals();
            NotifyStateChanged();
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
            UpdateVisableAnimals();
            NotifyStateChanged();
        }

        public void SetSearchString(string value)
        {
            SearchString = value?.Trim() ?? String.Empty;
            UpdateVisableAnimals();
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
        }

        public void UpdateVisableAnimals()
        {
            // Sort master list of animals. Sorting before filtering will maintain cascaded sort ordering regardless of filtering.
            _animals = ApplySort(_animals);

            // Filter by search text and rescue criteria
            _visibleAnimals = [.. _animals
                .Where(ApplyRescueCriteria)
                .Where(ApplySearch)];
        }

        private bool ApplySearch(Animal animal)
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;

            if (animal.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
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
