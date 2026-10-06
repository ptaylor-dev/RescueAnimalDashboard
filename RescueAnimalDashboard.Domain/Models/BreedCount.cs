namespace RescueAnimalDashboard.Domain.Models
{
    public struct BreedCount(string name, int count)
    {
        public string Name { get; set; } = name;
        public int Count { get; set; } = count;
    }
}
