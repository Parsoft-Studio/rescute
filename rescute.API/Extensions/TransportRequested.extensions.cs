using rescute.API.Models;
using rescute.Domain.Aggregates;
using rescute.Domain.Aggregates.TimelineItems;
using rescute.Domain.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace rescute.API.Extensions
{
    public static class TransportRequestedExtensions
    {
        public static TransportRequestGetModel ToModel(this TransportRequest timelineEvent)
        {
            var model = new TransportRequestGetModel
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
        public static IReadOnlyCollection<TransportRequestGetModel> ToModel(this IEnumerable<TransportRequest> timelineEvents)
        {
            var result = new List<TransportRequestGetModel>();
            foreach (TransportRequest timelineEvent in timelineEvents)
            {
                result.Add(timelineEvent.ToModel());
            }

            return result;
        }

    }
}
