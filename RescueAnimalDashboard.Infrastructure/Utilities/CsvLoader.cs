using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using RescueAnimalDashboard.Domain.Models;
using System.Globalization;

namespace RescueAnimalDashboard.Infrastructure.Utilities
{
    public static class CsvLoader
    {
        public static List<Animal> LoadAnimalsFromCsv(string csvPath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);

            // Convert any empty string fields to null
            csv.Context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);

            return [.. csv.GetRecords<AnimalCsvRow>().Select(ToAnimal)];
        }

        private static Animal ToAnimal(AnimalCsvRow row) => new(
            row.AnimalId ?? throw new InvalidDataException("AnimalId is required."),
            row.AnimalType ?? throw new InvalidDataException($"AnimalType is required ({row.AnimalId})."),
            row.Breed,
            row.Color,
            row.DateOfBirth,
            row.DateTime,
            row.Name,
            row.OutcomeType,
            row.OutcomeSubtype,
            row.SexUponOutcome,
            row.Latitude,
            row.Longitude,
            row.AgeUponOutcome,
            row.AgeUponOutcomeInWeeks);

        private class AnimalCsvRow
        {
            [Name("age_upon_outcome")] public string? AgeUponOutcome { get; set; }
            [Name("animal_id")] public string? AnimalId { get; set; }
            [Name("animal_type")] public string? AnimalType { get; set; }
            [Name("breed")] public string? Breed { get; set; }
            [Name("color")] public string? Color { get; set; }
            [Name("date_of_birth")] public DateOnly? DateOfBirth { get; set; }
            [Name("datetime")] public DateTime? DateTime { get; set; }
            [Name("name")] public string? Name { get; set; }
            [Name("outcome_subtype")] public string? OutcomeSubtype { get; set; }
            [Name("outcome_type")] public string? OutcomeType { get; set; }
            [Name("sex_upon_outcome")] public string? SexUponOutcome { get; set; }
            [Name("location_lat")] public double Latitude { get; set; }
            [Name("location_long")] public double Longitude { get; set; }
            [Name("age_upon_outcome_in_weeks")] public double AgeUponOutcomeInWeeks { get; set; }
        }
    }
}

