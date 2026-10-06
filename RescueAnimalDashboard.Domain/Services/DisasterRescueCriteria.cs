using RescueAnimalDashboard.Domain.Interfaces;

namespace RescueAnimalDashboard.Domain.Services
{
    public readonly struct DisasterRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Male"; }
        public readonly List<string> BreedList
        {
            get =>
            [
                "doberman pinscher",
                "german shepherd",
                "golden retriever",
                "bloodhound",
                "rottweiler"
            ];
        }
        public readonly int MinAgeInWeeks { get => 20; }
        public readonly int MaxAgeInWeeks { get => 300; }
    }
}
