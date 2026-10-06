namespace RescueAnimalDashboard.Infrastructure.Persistence.Entities
{
    public abstract class LookupRecord
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class AnimalTypeRecord : LookupRecord { }

    public class SexUponOutcomeRecord : LookupRecord { }

    public class OutcomeTypeRecord : LookupRecord { }

    public class AnimalRecord
    {
        public int Id { get; set; }
        public required string AnimalId { get; set; }

        public int AnimalTypeId { get; set; }
        public AnimalTypeRecord AnimalType { get; set; } = null!;

        public int? SexUponOutcomeId { get; set; }
        public SexUponOutcomeRecord? SexUponOutcome { get; set; }

        public int? OutcomeTypeId { get; set; }
        public OutcomeTypeRecord? OutcomeType { get; set; }

        public string? Breed { get; set; }
        public string? Color { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime? DateTime { get; init; }
        public string? Name { get; init; }
        public string? OutcomeSubtype { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string? AgeUponOutcome { get; init; }
        public double AgeUponOutcomeInWeeks { get; init; }
    }
}
