namespace RescueAnimalDashboard.Domain.Interfaces
{
    // Defines the criteria for successful rescue training
    public interface IRescueCriteria
    {
        string TYPE { get; }
        string SEX { get; }
        List<string> BreedList { get; }
        int MinAgeInWeeks { get; }
        int MaxAgeInWeeks { get; }
    }
}
