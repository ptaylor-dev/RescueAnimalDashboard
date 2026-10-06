using Microsoft.EntityFrameworkCore;
using RescueAnimalDashboard.Application.Interfaces;
using RescueAnimalDashboard.Application.Models;
using RescueAnimalDashboard.Domain.Models;
using RescueAnimalDashboard.Domain.Services;
using RescueAnimalDashboard.Infrastructure.Persistence.Context;
using RescueAnimalDashboard.Infrastructure.Persistence.Entities;

namespace RescueAnimalDashboard.Infrastructure.Repositories
{
    internal class EfAnimalRepository(RescueAnimalDbContext db) : IAnimalRepository
    {
        public async Task<AnimalResponse> GetAnimalsAsync(
            AnimalQuery aQuery, CancellationToken ct = default)
        {
            var query = db.Animals.AsNoTracking();


            query = ApplyRescueCriteria(query, aQuery.RescueType);

            // Escape characters that could alter the search terms
            if (!string.IsNullOrEmpty(aQuery.SearchText))
            {
                var searchString = aQuery.SearchText.Trim()
                    .Replace("\\", "\\\\")
                    .Replace("%", "\\%")
                    .Replace("_", "\\_");

                var pattern = $"%{searchString}%";

                // Compare search string to each property
                query = query.Where(a =>
                    (a.AnimalId != null
                        && EF.Functions.ILike(a.AnimalId, pattern, "\\"))

                    || (a.AnimalType != null
                        && EF.Functions.ILike(a.AnimalType.Name, pattern, "\\"))

                    || (a.Breed != null
                        && EF.Functions.ILike(a.Breed, pattern, "\\"))
                    
                    || (a.Color != null
                        && EF.Functions.ILike(a.Color, pattern, "\\"))
                    
                    || (a.Name != null
                        && EF.Functions.ILike(a.Name, pattern, "\\"))
                   
                    || (a.OutcomeType != null
                        && EF.Functions.ILike(a.OutcomeType.Name, pattern, "\\"))
                    
                    || (a.OutcomeSubtype != null
                        && EF.Functions.ILike(a.OutcomeSubtype, pattern, "\\"))
                    
                    || (a.SexUponOutcome != null
                        && EF.Functions.ILike(a.SexUponOutcome.Name, pattern, "\\"))
                    
                    || (a.AgeUponOutcome != null
                        && EF.Functions.ILike(a.AgeUponOutcome, pattern, "\\")));
            }

            var totalCount = await query.CountAsync(ct);

            // Get a sorted list of top breeds for the chart
            var chartBreeds = await GetBreedCountsAsync(query, aQuery.BreedCountLimit, ct);

            // Sort the results of the main query
            query = ApplySorting(query, aQuery.SortBy, aQuery.SortBySecondary, aQuery.SortAscending);

            // Generate the resulting page of Animal instances
            var animals = await query
                .Skip(aQuery.PageIndex * aQuery.PageSize)
                .Take(aQuery.PageSize)
                .Select(a => new Animal(
                    a.AnimalId,
                    a.AnimalType.Name,
                    a.Breed,
                    a.Color,
                    a.DateOfBirth,
                    a.DateTime,
                    a.Name,
                    a.OutcomeType != null ? a.OutcomeType.Name : null,
                    a.OutcomeSubtype,
                    a.SexUponOutcome != null ? a.SexUponOutcome.Name : null,
                    a.Latitude,
                    a.Longitude,
                    a.AgeUponOutcome,
                    a.AgeUponOutcomeInWeeks))
                .ToListAsync(ct);

            var pageCount = (int)Math.Ceiling(totalCount / (double)aQuery.PageSize);

            return new AnimalResponse(
                animals,
                aQuery.PageIndex,
                aQuery.PageSize,
                totalCount,
                pageCount,
                chartBreeds);
        }

        private static async Task<List<BreedCount>> GetBreedCountsAsync(
            IQueryable<AnimalRecord> query,
            int breedCountLimit,
            CancellationToken ct)
        {
            var breedCounts = await query
                .GroupBy(a => a.Breed)
                .Select(g => new
                {
                    Breed = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(group => group.Count)
                .ThenBy(group => group.Breed)
                .ToListAsync(ct);

            // Create list of structs from grouped query results
            return [.. breedCounts
                .Select(g => new BreedCount(
                    g.Breed ?? "Unknown",   // Unknown if breed label is null
                    g.Count))
                .Take(breedCountLimit)];
        }

        private static IQueryable<AnimalRecord> ApplySorting(
            IQueryable<AnimalRecord> query,
            AnimalSortOption? primarySort,
            AnimalSortOption? secondarySort,
            bool ascending)
        {
            // Default when sort option was not used in request.
            var primary = primarySort ?? AnimalSortOption.AnimalId;

            IOrderedQueryable<AnimalRecord> orderedQuery = primary switch
            {
                AnimalSortOption.AnimalId => ascending
                    ? query.OrderBy(a => a.AnimalId)
                    : query.OrderByDescending(a => a.AnimalId),

                AnimalSortOption.Name => ascending
                    ? query.OrderBy(a => a.Name)
                    : query.OrderByDescending(a => a.Name),

                AnimalSortOption.Type => ascending
                    ? query.OrderBy(a => a.AnimalType.Name)
                    : query.OrderByDescending(a => a.AnimalType.Name),

                AnimalSortOption.Breed => ascending
                    ? query.OrderBy(a => a.Breed)
                    : query.OrderByDescending(a => a.Breed),

                AnimalSortOption.Color => ascending
                    ? query.OrderBy(a => a.Color)
                    : query.OrderByDescending(a => a.Color),

                AnimalSortOption.OutcomeType => ascending
                    ? query.OrderBy(a => a.OutcomeType!.Name)
                    : query.OrderByDescending(a => a.OutcomeType!.Name),

                AnimalSortOption.AgeUponOutcomeInWeeks => ascending
                    ? query.OrderBy(a => a.AgeUponOutcomeInWeeks)
                    : query.OrderByDescending(a => a.AgeUponOutcomeInWeeks),

                AnimalSortOption.DateOfBirth => ascending
                    ? query.OrderBy(a => a.DateOfBirth)
                    : query.OrderByDescending(a => a.DateOfBirth),

                _ => ascending
                    ? query.OrderBy(a => a.AnimalId)
                    : query.OrderByDescending(a => a.AnimalId)
            };

            if (secondarySort is not null && secondarySort != primary)
            {
                orderedQuery = secondarySort.Value switch
                {
                    AnimalSortOption.AnimalId => ascending
                        ? orderedQuery.ThenBy(a => a.AnimalId)
                        : orderedQuery.ThenByDescending(a => a.AnimalId),

                    AnimalSortOption.Name => ascending
                        ? orderedQuery.ThenBy(a => a.Name)
                        : orderedQuery.ThenByDescending(a => a.Name),

                    AnimalSortOption.Type => ascending
                        ? orderedQuery.ThenBy(a => a.AnimalType.Name)
                        : orderedQuery.ThenByDescending(a => a.AnimalType.Name),

                    AnimalSortOption.Breed => ascending
                        ? orderedQuery.ThenBy(a => a.Breed)
                        : orderedQuery.ThenByDescending(a => a.Breed),

                    AnimalSortOption.Color => ascending
                        ? orderedQuery.ThenBy(a => a.Color)
                        : orderedQuery.ThenByDescending(a => a.Color),

                    AnimalSortOption.OutcomeType => ascending
                        ? orderedQuery.ThenBy(a => a.OutcomeType!.Name)
                        : orderedQuery.ThenByDescending(a => a.OutcomeType!.Name),

                    AnimalSortOption.AgeUponOutcomeInWeeks => ascending
                        ? orderedQuery.ThenBy(a => a.AgeUponOutcomeInWeeks)
                        : orderedQuery.ThenByDescending(a => a.AgeUponOutcomeInWeeks),

                    AnimalSortOption.DateOfBirth => ascending
                        ? orderedQuery.ThenBy(a => a.DateOfBirth)
                        : orderedQuery.ThenByDescending(a => a.DateOfBirth),

                    _ => ascending
                        ? orderedQuery.ThenBy(a => a.AnimalId)
                        : orderedQuery.ThenByDescending(a => a.AnimalId)
                };
            }

            return ascending
                ? orderedQuery.ThenBy(a => a.AnimalId)
                : orderedQuery.ThenByDescending(a => a.AnimalId
                );
        }

        private static IQueryable<AnimalRecord> ApplyRescueCriteria(
            IQueryable<AnimalRecord> query,
            RescueCategory? rescueCategory)
        {
            var criteria = RescueClassifier.GetCriteria(rescueCategory);

            if (criteria is null)
            {
                return query;
            }

            var type = criteria.TYPE;
            var sex = criteria.SEX;
            var breeds = criteria.BreedList.ToArray();
            var minAgeInWeeks = criteria.MinAgeInWeeks;
            var maxAgeInWeeks = criteria.MaxAgeInWeeks;

            return query.Where(a =>
                a.AnimalType != null &&
                EF.Functions.ILike(a.AnimalType.Name, type) &&

                a.SexUponOutcome != null &&
                EF.Functions.ILike(a.SexUponOutcome.Name, sex) &&

                a.Breed != null &&
                breeds.Any(breed =>
                    EF.Functions.ILike(a.Breed, breed)) &&

                a.AgeUponOutcomeInWeeks >= minAgeInWeeks &&
                a.AgeUponOutcomeInWeeks <= maxAgeInWeeks);
        }
    }
}
