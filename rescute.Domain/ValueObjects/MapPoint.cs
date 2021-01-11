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
        public MapPoint(double lattitude, double longitude)
        {
            Latitude = lattitude;
            Longitude = longitude;
        }
        private MapPoint (){}

    }
}
