namespace RescueAnimalDashboard.Domain.ValueObjects
{
    public class Animal(
        string animalId,
        string type,
        string? breed,
        string? color,
        DateOnly? dateOfBirth,
        DateTime? dateTime,
        string? name,
        string? outcomeType,
        string? outcomeSubtype,
        string? sexUponOutcome,
        double latitude,
        double longitude,
        string? ageUponOutcome,
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
        public double Latitude { get; init; } = latitude;
        public double Longitude { get; init; } = longitude;
        public string? AgeUponOutcome { get; init; } = ageUponOutcome;
        public double AgeUponOutcomeInWeeks { get; init; } = ageUponOutcomeInWeeks;
        public bool HasValidCoordinates =>
            Latitude is >= -90 and <= 90 && Longitude is >= -180 and <= 180;


        public override string ToString() =>
            $"""
            {AnimalId}
            {Type}
            {Breed}
            {Color}
            {DateOfBirth}
            {DateTime}
            {Name}
            {OutcomeType}
            {OutcomeSubtype}
            {SexUponOutcome}
            {AgeUponOutcome}
            """;
    }
}
