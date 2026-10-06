using RescueAnimalDashboard.Domain.Models;

namespace RescueAnimalDashboard.Application.Models
{
    public sealed class AnimalQuery(
        RescueCategory? rescueType,
        string? searchText,
        AnimalSortOption? sortBy,
        AnimalSortOption? sortBySecondary,
        bool? sortAscending,
        int? pageIndex,
        int? pageSize,
        int? breedCountLimit)
    {
        private const int MaxPageSize = 100;
        private const int MaxBreedCountLimit = 100;

        public RescueCategory? RescueType { get; init; } = rescueType;
        public string? SearchText { get; init; } = searchText;
        public AnimalSortOption? SortBy { get; init; } = sortBy;
        public AnimalSortOption? SortBySecondary { get; init; } = sortBySecondary;
        public bool SortAscending { get; init; } = sortAscending ?? true;
        public int PageIndex { get; init; } = Math.Max(pageIndex ?? 0, 0);
        public int PageSize { get; init; } = Math.Clamp(pageSize ?? 10, 1, MaxPageSize);
        public int BreedCountLimit { get; init; } = Math.Clamp(
            breedCountLimit ?? 25, 1, MaxBreedCountLimit);
    }
}
