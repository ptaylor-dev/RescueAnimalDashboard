using RescueAnimalDashboard.Domain.Models;

namespace RescueAnimalDashboard.Application.Models;

public sealed class AnimalResponse(
    IReadOnlyList<Animal> animals,
    int pageIndex,
    int pageSize,
    int totalCount,
    int pageCount,
    List<BreedCount> breedCounts)
{
    public IReadOnlyList<Animal> Animals { get; } = animals;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
    public int PageCount { get; } = pageCount;
    public List<BreedCount> BreedCounts { get; } = breedCounts;
}