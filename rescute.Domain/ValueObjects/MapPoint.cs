using rescute.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.ValueObjects
{
    public class MapPoint : ValueObject
    {
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
