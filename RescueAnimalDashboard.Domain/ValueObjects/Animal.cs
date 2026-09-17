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
        string? outcomeType,
        string outcomeSubtype,
        string sexUponOutcome,
        GeoCoordinates geoCoordinates,
        string ageUponOutcome,
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
        public string AgeUponOutcome { get; init; } = ageUponOutcome;
        public double AgeUponOutcomeInWeeks { get; init; } = ageUponOutcomeInWeeks;


        public string ToString()
        {
            return $"{AnimalId}\n" +
                $"{Type}\n" +
                $"{Breed}\n" +
                $"{Color}\n" +
                $"{DateOfBirth}\n" +
                $"{DateTime}\n" +
                $"{Name}\n" +
                $"{OutcomeType}\n" +
                $"{OutcomeSubtype}\n" +
                $"{AgeUponOutcome}\n" +
                $"{AgeUponOutcomeInWeeks}\n";
        }
    }
}
