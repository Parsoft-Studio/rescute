using System;

namespace rescute.Domain.ValueObjects;

public record Distance
{
    /// <summary>
    /// Returns the distance represented by this instance in meters.
    /// </summary>
    public int Meters { get; private set; }

    public Distance(MapPoint pointA, MapPoint pointB)
    {
        // Conversion factors: Approx. meters per degree of latitude and longitude
        const double metersPerDegreeLat = 111000.0;

        // Calculate latitude and longitude differences
        double deltaLat = pointB.Latitude - pointA.Latitude;
        double deltaLon = pointB.Longitude - pointA.Longitude;

        // Adjust longitude scaling by the latitude to account for the Earth's shape
        double metersPerDegreeLon =
            metersPerDegreeLat * Math.Cos(DegreesToRadians((pointA.Latitude + pointB.Latitude) / 2));

        // Pythagorean theorem to calculate the distance
        Meters = (int)Math.Sqrt(
            Math.Pow(deltaLat * metersPerDegreeLat, 2) +
            Math.Pow(deltaLon * metersPerDegreeLon, 2)
        );
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    /// <summary>
    /// Returns the distance represented by this instance in kilometers
    /// </summary>
    public double Kilometers => Math.Round((double)Meters / 1000, 1);
}