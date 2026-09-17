using RescueAnimalDashboard.Application.Interfaces;
using RescueAnimalDashboard.Domain.ValueObjects;

namespace RescueAnimalDashboard.Infrastructure.Repositories
{
    public class AnimalRepositoryFromCsv : IAnimalRepository
    {
        #region Constants
        private readonly string CSV_PATH = "Resources\\aac_shelter_outcomes.csv";

        private readonly int ANIMAL_ID_COLUMN = 2;
        private readonly int TYPE_COLUMN = 3;
        private readonly int BREED_COLUNM = 4;
        private readonly int COLOR_COLUMN = 5;
        private readonly int DATE_OF_BIRTH_COLUMN = 6;
        private readonly int DATETIME_COLUMN = 7;
        private readonly int NAME_COLUMN = 9;
        private readonly int OUTCOME_COLUMN = 11;
        private readonly int SUB_OUTCOME_COLUMN = 10;
        private readonly int SEX_UPON_OUTCOME_COLUMN = 12;
        private readonly int LATITUDE_COLUMN = 13;
        private readonly int LONGITUDE_COLUMN = 14;
        private readonly int AGE_UPON_OUTCOME_COLUMN = 1;
        private readonly int OUTCOME_AGE_IN_WEEKS_COLUMN = 15;
        #endregion

        private readonly string _csvPath;
        private readonly List<Animal> _animals = [];

        public AnimalRepositoryFromCsv()
        {
            _csvPath = Path.Combine(AppContext.BaseDirectory, CSV_PATH);
            LoadAnimalsFromCsv();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return _animals;
        }

        private void LoadAnimalsFromCsv()
        {
            // Loads entries from CSV file
            var csvData = File.ReadAllLines(_csvPath).Skip(1).ToList();

            foreach (var line in csvData)
            {
                try
                {
                    // Parse record into Animal object instance and add to repository
                    var parsedLine = line.Split(',');

                    Animal animal = new
                        (
                        parsedLine[ANIMAL_ID_COLUMN].Trim(' ', '"'),
                        parsedLine[TYPE_COLUMN].Trim(' ', '"'),
                        parsedLine[BREED_COLUNM].Trim(' ', '"'),
                        parsedLine[COLOR_COLUMN].Trim(' ', '"'),
                        DateOnly.Parse(parsedLine[DATE_OF_BIRTH_COLUMN].Trim(' ', '"')),
                        DateTime.Parse(parsedLine[DATETIME_COLUMN].Trim(' ', '"')),
                        parsedLine[NAME_COLUMN].Trim(' ', '"'),
                        parsedLine[OUTCOME_COLUMN].Trim(' ', '"'),
                        parsedLine[SUB_OUTCOME_COLUMN].Trim(' ', '"'),
                        parsedLine[SEX_UPON_OUTCOME_COLUMN].Trim(' ', '"'),
                        new(
                            double.Parse(parsedLine[LATITUDE_COLUMN].Trim(' ', '"')),
                            double.Parse(parsedLine[LONGITUDE_COLUMN].Trim(' ', '"'))
                            ),
                        parsedLine[AGE_UPON_OUTCOME_COLUMN].Trim(' ', '"'),
                        double.Parse(parsedLine[OUTCOME_AGE_IN_WEEKS_COLUMN].Trim(' ', '"'))
                        );
                    _animals.Add(animal);
                }
                catch (Exception ex)
                {
                    // Log offending record
                    Console.WriteLine($"Could not create animal instance from line: {line}");
                    // Log specific error message
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            Console.WriteLine($"{_animals.Count} entries added to repository!");
        }
    }
}
