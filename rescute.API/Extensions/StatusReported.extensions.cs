using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace rescute.API.Extensions
{
    public static class StatusReportedExtensions
    {
        public static StatusReportGetModel ToModel(this StatusReport timelineEvent, string relativeAttachmentsRootPath)
        {
            var model = new StatusReportGetModel
            {
                AnimalId = timelineEvent.AnimalId.ToString(),
                Description = timelineEvent.Description,
                Lattitude = timelineEvent.EventLocation.Latitude,
                Longitude = timelineEvent.EventLocation.Longitude,
                EventId = timelineEvent.Id.ToString(),
                CreatedById = timelineEvent.CreatedBy.ToString()
            };

            var modelAttachments = new List<AttachmentModel>();
            foreach (Attachment att in timelineEvent.Attachments)
            {
                modelAttachments.Add(new AttachmentModel()
                {
                    CreationDate = att.CreationDate,
                    Description = att.Description,
                    Url = Path.Combine(relativeAttachmentsRootPath, att.FileName).Replace("\\", "/"),
                    Type = att.Type.ToString()
                });
            }
            model.Attachments = modelAttachments;

            return model;
        }
        public static IReadOnlyCollection<StatusReportGetModel> ToModel(this IEnumerable<StatusReport> timelineEvents, string relativeAttachmentsRootPath)
        {
            var result = new List<StatusReportGetModel>();
            foreach (StatusReport timelineEvent in timelineEvents)
            {
                result.Add(timelineEvent.ToModel(relativeAttachmentsRootPath));
            }

            return result;
        }

    }
}
