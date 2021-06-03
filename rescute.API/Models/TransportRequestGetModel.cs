using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class TransportRequestGetModel
    {
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public double ToLattitude { get; set; }
        public double ToLongitude { get; set; }

        public string Description { get; set; }
        public string AnimalId { get; set; }
        public string EventId { get; set; }
        public string CreatedById { get; set; }
    }
}
