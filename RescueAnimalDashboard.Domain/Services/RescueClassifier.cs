using RescueAnimalDashboard.Domain.Interfaces;
using RescueAnimalDashboard.Domain.Models;

namespace RescueAnimalDashboard.Domain.Services
{
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

        public static bool IsRescueAnimal(Animal animal, RescueCategory rescueCategory)
        {
            var criteria = GetCriteria(rescueCategory);

            return criteria is null || VerifyRescueCriteria(animal, criteria);
        }

        public static IRescueCriteria? GetCriteria(RescueCategory? rescueCategory)
        {
            return rescueCategory switch
            {
                RescueCategory.Water => new WaterRescueCriteria(),
                RescueCategory.Mountain => new MountainRescueCriteria(),
                RescueCategory.Disaster => new DisasterRescueCriteria(),
                _ => null
            };
        }

        private static bool VerifyRescueCriteria(Animal animal, IRescueCriteria criteria)
        {
            bool IsCorrectType, IsCorrectBreed, IsCorrectSex, IsCorrectAge;

            IsCorrectType = String.Equals(
                animal.Type,
                criteria.TYPE,
                StringComparison.OrdinalIgnoreCase);    // StringComparison handles null check

            IsCorrectBreed = animal.Breed is not null
                && criteria.BreedList.Contains(animal.Breed.ToLower());

            IsCorrectSex = String.Equals(
                animal.SexUponOutcome,
                criteria.SEX,
                StringComparison.OrdinalIgnoreCase);

            IsCorrectAge = animal.AgeUponOutcomeInWeeks >= criteria.MinAgeInWeeks
                && animal.AgeUponOutcomeInWeeks <= criteria.MaxAgeInWeeks;

            return (IsCorrectType && IsCorrectBreed && IsCorrectSex && IsCorrectAge);
        }
    }
}
