using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineEvents;
using rescute.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace rescute.API.Extensions
{
    public static class TransportRequestedExtensions
    {
        public static TransportRequestedGetModel ToModel(this TransportRequested timelineEvent, string relativeAttachmentsRootPath)
        {
            var model = new TransportRequestedGetModel
            {
                AnimalId = timelineEvent.AnimalId.ToString(),
                Description = timelineEvent.Description,
                Lattitude = timelineEvent.EventLocation.Latitude,
                Longitude = timelineEvent.EventLocation.Longitude,
                ToLattitude = timelineEvent.ToLocation.Latitude,
                ToLongitude = timelineEvent.ToLocation.Longitude,
                EventId = timelineEvent.Id.ToString(),
                CreatedById = timelineEvent.CreatedBy.ToString()
            };


            return model;
        }
        public static IReadOnlyCollection<TransportRequestedGetModel> ToModel(this IEnumerable<TransportRequested> timelineEvents, string relativeAttachmentsRootPath)
        {
            var result = new List<TransportRequestedGetModel>();
            foreach (TransportRequested timelineEvent in timelineEvents)
            {
                result.Add(timelineEvent.ToModel(relativeAttachmentsRootPath));
            }

            return result;
        }

    }
}
