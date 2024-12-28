using System;
using rescute.Shared;

namespace rescute.Domain.ValueObjects;

public record MapPoint
{
    public static MapPoint Empty => new(double.NaN, double.NaN);
    public double Latitude { get; }
    public double Longitude { get; }

    public MapPoint(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90) throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude is < -180 or > 180) throw new ArgumentException("Longitude must be between -180 and 180.");
        Latitude = latitude;
        Longitude = longitude;
    }

    private MapPoint()
    {
    }

    public static bool IsEmpty(MapPoint point)
    {
        return point.Equals(Empty);
    }
}