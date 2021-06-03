using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class StatusReportGetModel
    {
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public string AnimalId { get; set; }
        public string EventId { get; set; }
        public string CreatedById { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }
}
