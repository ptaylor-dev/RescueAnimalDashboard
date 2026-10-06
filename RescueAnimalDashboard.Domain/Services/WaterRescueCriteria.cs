using RescueAnimalDashboard.Domain.Interfaces;

namespace RescueAnimalDashboard.Domain.Services
{
    public readonly struct WaterRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Female"; }
        public readonly List<string> BreedList
        {
            get =>
            [
                "labrador retriever",
                "chesapeake bay retriever",
                "newfoundland"
            ];
        }
        public readonly int MinAgeInWeeks { get => 26; }
        public readonly int MaxAgeInWeeks { get => 156; }
    }
}
