using Microsoft.AspNetCore.Http;
using rescute.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class AnimalGetModel
    {
        public IEnumerable<AttachmentModel> Attachments { get; set; }
        public string Description { get; set; } 
        public string AnimalType { get;  set; }
        public Guid AnimalId { get; set; }
        public Guid IntroducedById { get; set; }
    }
}
