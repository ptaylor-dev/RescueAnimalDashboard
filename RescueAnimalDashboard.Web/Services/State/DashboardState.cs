using RescueAnimalDashboard.Application.Models;
using RescueAnimalDashboard.Domain.Models;

namespace RescueAnimalDashboard.Web.Services.State;

public sealed class DashboardState
{
    public const int ChartBreedCountLimit = 25;

    public RescueCategory RescueCategory { get; private set; }
        = RescueCategory.None;

    public string SearchString { get; private set; } = string.Empty;

    public Animal? SelectedAnimal { get; private set; }

    // These are only the currently loaded page, not all matching animals.
    public IReadOnlyList<Animal> CurrentPageAnimals { get; private set; }
        = [];

    public int TotalCount { get; private set; }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; } = 10;

    public AnimalSortOption? SortOption { get; private set; }
        = AnimalSortOption.AnimalId;

    public AnimalSortOption? SecondarySortOption { get; private set; }

    public bool SortAscending { get; private set; } = true;
    public List<BreedCount> BreedCounts { get; private set; } = [];


    // Changes requiring the table to refresh data.
    public event Func<Task>? FiltersChanged;

    // Changes for map/chart/other UI subscribers.
    public event Action? OnChange;

    public async Task SetSearchStringAsync(string? value)
    {
        var search = value?.Trim() ?? string.Empty;

        if (SearchString == search)
            return;

        SearchString = search;

        OnChange?.Invoke();
        await NotifyFiltersChangedAsync();
    }

    public async Task SetRescueCategoryAsync(RescueCategory category)
    {
        if (RescueCategory == category)
            return;

        RescueCategory = category;

        OnChange?.Invoke();
        await NotifyFiltersChangedAsync();
    }

    public async Task SetSecondarySortAsync(AnimalSortOption? option)
    {
        if (SecondarySortOption == option)
            return;

        SecondarySortOption = option;

        OnChange?.Invoke();
        await NotifyFiltersChangedAsync();
    }

    public void SetSelectedAnimal(Animal? animal)
    {
        if (ReferenceEquals(SelectedAnimal, animal))
            return;

        SelectedAnimal = animal;
        OnChange?.Invoke();
    }

    public void UpdatePage(
        AnimalQuery query,
        AnimalResponse response)
    {
        CurrentPageAnimals = response.Animals;
        TotalCount = response.TotalCount;

        PageIndex = query.PageIndex;
        PageSize = query.PageSize;

        SortOption = query.SortBy;
        SecondarySortOption = query.SortBySecondary;
        SortAscending = query.SortAscending;

        BreedCounts = response.BreedCounts;

        // Notify the map/chart, but do not initiate another API request.
        OnChange?.Invoke();
    }

    private async Task NotifyFiltersChangedAsync()
    {
        var handlers = FiltersChanged;

        if (handlers is null)
            return;

        foreach (var handler in handlers.GetInvocationList())
        {
            await ((Func<Task>)handler)();
        }
    }
}