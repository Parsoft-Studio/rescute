using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rescute.API.Models
{
    public class AttachmentModel
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
