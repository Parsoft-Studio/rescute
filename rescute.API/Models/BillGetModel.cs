using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class BillGetModel
    {
        public string Description { get; set; }
        public string AnimalId { get; set; }
        public decimal Total { get; set; }
        public bool IncludesLabResults { get; set; }
        public bool IncludesPrescription { get; set; }
        public bool IncludesVetFee { get; set; }
        public IEnumerable<string> RelatedMedicalDocumentIds { get; set; }
        public IEnumerable<AttachmentModel> Attachments { get; set; }
    }
}
