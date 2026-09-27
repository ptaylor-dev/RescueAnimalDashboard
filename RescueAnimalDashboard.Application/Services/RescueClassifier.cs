using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Application.Services
{
    public enum RescueCategory
    {
        None,
        Water,
        Mountain,
        Disaster
    }

    // Defines the criteria for successful rescue training
    public interface IRescueCriteria
    {
        string TYPE { get; }
        string SEX { get; }
        List<string> BREED_LIST { get; }
        int MIN_AGE_IN_WEEKS { get; }
        int MAX_AGE_IN_WEEKS { get; }
    }

    public readonly struct WaterRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Female"; }
        public readonly List<string> BREED_LIST
        {
            get =>
            [
                "labrador retriever",
                "chesapeake bay retriever",
                "newfoundland"
            ];
        }
        public readonly int MIN_AGE_IN_WEEKS { get => 26; }
        public readonly int MAX_AGE_IN_WEEKS { get => 156; }
    }

    public readonly struct MountainRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Male"; }
        public readonly List<string> BREED_LIST
        {
            get =>
            [
                "german shepherd",
                "alaskan malamute",
                "old english sheepdog",
                "siberian husky"
            ];
        }
        public readonly int MIN_AGE_IN_WEEKS { get => 26; }
        public readonly int MAX_AGE_IN_WEEKS { get => 156; }
    }

    public readonly struct DisasterRescueCriteria() : IRescueCriteria
    {
        public readonly string TYPE { get => "Dog"; }
        public readonly string SEX { get => "Intact Male"; }
        public readonly List<string> BREED_LIST
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
        public readonly int MIN_AGE_IN_WEEKS { get => 20; }
        public readonly int MAX_AGE_IN_WEEKS { get => 300; }
    }


    public class RescueClassifier
    {

        public static bool IsAWaterRescueAnimal(Animal animal)
        {
            return VerifyRescueCriteria(animal, new WaterRescueCriteria());
        }

        public static bool IsAMountainRescueAnimal(Animal animal)
        {
            return VerifyRescueCriteria(animal, new MountainRescueCriteria());
        }

        public static bool IsADisasterRescueAnimal(Animal animal)
        {
            return VerifyRescueCriteria(animal, new DisasterRescueCriteria());
        }

        private static bool VerifyRescueCriteria(Animal animal, IRescueCriteria criteria)
        {
            bool correctType, correctBreed, correctSex, correctAge;

            correctType = String.Equals(animal.Type, criteria.TYPE, StringComparison.OrdinalIgnoreCase);    // StringComparison handles null check

            correctBreed = animal.Breed is not null && criteria.BREED_LIST.Contains(animal.Breed.ToLower());

            correctSex = String.Equals(animal.SexUponOutcome, criteria.SEX, StringComparison.OrdinalIgnoreCase);

            correctAge = animal.AgeUponOutcomeInWeeks >= criteria.MIN_AGE_IN_WEEKS && animal.AgeUponOutcomeInWeeks <= criteria.MAX_AGE_IN_WEEKS;

            return (correctType && correctBreed && correctSex && correctAge);
        }
    }
}
