using RescueAnimalDashboard.Domain.ValueObjects;
using RescueAnimalDashboard.Infrastructure.Persistence.Entities;
using RescueAnimalDashboard.Infrastructure.Utilities;

namespace RescueAnimalDashboard.Infrastructure.Persistence.Seeding
{
    internal static class AnimalSeedData
    {
        public static List<AnimalRecord> Load(string csvPath) =>
            BuildRecords(CsvLoader.LoadAnimalsFromCsv(csvPath));

        private static List<AnimalRecord> BuildRecords(List<Animal> animals)
        {
            // Create case-insensitive dictionaries of lookup table values to avoid duplicates
            var types = new Dictionary<string, AnimalTypeRecord>(StringComparer.OrdinalIgnoreCase);
            var sexes = new Dictionary<string, SexUponOutcomeRecord>(StringComparer.OrdinalIgnoreCase);
            var outcomes = new Dictionary<string, OutcomeTypeRecord>(StringComparer.OrdinalIgnoreCase);

            return [..animals.Select(a => new AnimalRecord
            {
                AnimalId = a.AnimalId,
                AnimalType = GetOrCreate(types, a.Type, n => new AnimalTypeRecord { Name = n })!,
                SexUponOutcome = GetOrCreate(sexes, a.SexUponOutcome, n => new SexUponOutcomeRecord { Name = n }),
                OutcomeType = GetOrCreate(outcomes, a.OutcomeType, n => new OutcomeTypeRecord { Name = n }),
                Breed = a.Breed,
                Color = a.Color,
                DateOfBirth = a.DateOfBirth,
                DateTime = a.DateTime,
                Name = a.Name,
                OutcomeSubtype = a.OutcomeSubtype,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                AgeUponOutcome = a.AgeUponOutcome,
                AgeUponOutcomeInWeeks = a.AgeUponOutcomeInWeeks
            })];
        }

        private static T? GetOrCreate<T>(Dictionary<string, T> cache, string? name, Func<string, T> create)
            where T : LookupRecord
        {
            if (name is null)
                return null;

            // If name already exists in cache, return the record so that the appropriate lookup table entry is used.
            if (!cache.TryGetValue(name, out var record))
            {
                record = create(name);
                cache[name] = record;
            }

            return record;
        }
    }
}
