using RescueAnimalDashboard.Domain.Interfaces;

namespace RescueAnimalDashboard.Domain.Services
{
    public readonly struct MountainRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Male"; }
        public readonly List<string> BreedList
        {
            get =>
            [
                "german shepherd",
                "alaskan malamute",
                "old english sheepdog",
                "siberian husky"
            ];
        }
        public readonly int MinAgeInWeeks { get => 26; }
        public readonly int MaxAgeInWeeks { get => 156; }
    }
}
