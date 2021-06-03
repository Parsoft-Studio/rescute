using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class StatusReportPostModel
    {
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string AnimalId { get; set; }
        public IEnumerable<IFormFile> Attachments { get; set; }
    }
}
