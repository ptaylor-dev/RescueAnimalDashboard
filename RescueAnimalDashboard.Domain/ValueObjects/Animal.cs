namespace RescueAnimalDashboard.Domain.ValueObjects
{
    public class Animal(
        string animalId,
        string type,
        string breed,
        string color,
        DateOnly? dateOfBirth,
        DateTime? dateTime,
        string? name,
        string? outcomeType,                // TODO - consider using enum
        string outcomeSubtype,
        string sexUponOutcome,                     // TDOD - consider using enum
        GeoCoordinates geoCoordinates,
        double ageUponOutcomeInWeeks)
    {
        public string AnimalId { get; init; } = animalId;
        public string Type { get; init; } = type;
        public string? Breed { get; init; } = breed;
        public string? Color { get; init; } = color;
        public DateOnly? DateOfBirth { get; init; } = dateOfBirth;
        public DateTime? DateTime { get; init; } = dateTime;
        public string? Name { get; init; } = name;
        public string? OutcomeType { get; init; } = outcomeType;
        public string? OutcomeSubtype { get; init; } = outcomeSubtype;
        public string? SexUponOutcome { get; init; } = sexUponOutcome;
        public GeoCoordinates? GeoCoordinates { get; init; } = geoCoordinates;
        public double AgeUponOutcomeInWeeks { get; init; } = ageUponOutcomeInWeeks;
    }
}
