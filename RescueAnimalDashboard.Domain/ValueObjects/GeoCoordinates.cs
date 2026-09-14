namespace RescueAnimalDashboard.Domain.ValueObjects
{
    public class GeoCoordinates(double latitude, double longitude)
    {
        public double Latitude { get; init; } = latitude;
        public double Longitude { get; init; } = longitude;
    }
}
