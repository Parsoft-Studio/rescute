using rescute.Domain.Aggregates.TimelineItems;
using System.Collections.Generic;

namespace rescute.Domain.Extensions
{
    public static class TimelineItemExtensions
    {
        /// <summary>
        /// Creates a deep copy of the list of <see cref="TimelineItem"/>s by calling their <see cref="TimelineItem.Clone"/> function and adding the result to a new list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TimelineItem> DeepCopy(this List<TimelineItem> list)
        {
            var result = new List<TimelineItem>();
            foreach (TimelineItem e in list)
            {
                result.Add(e.Clone() as TimelineItem);
            }
            return result;
        }
    }
}
