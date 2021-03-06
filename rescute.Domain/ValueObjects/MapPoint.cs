using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class MapPoint : ValueObject
    {
        public static MapPoint Empty => new MapPoint(double.NaN, double.NaN);
        public static bool IsEmpty(MapPoint point)
        {
            return (double.IsNaN(point.Latitude) && double.IsNaN(point.Longitude));
        }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public MapPoint(double latitude, double longitude)
        {
            if ( latitude < -90 || latitude > 90) throw new ArgumentException("Lattitude must be between -90 and 90.");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("Longitude must be between -180 and 180.");
            Latitude = latitude;
            Longitude = longitude;
        }
        private MapPoint (){}

    }
}
