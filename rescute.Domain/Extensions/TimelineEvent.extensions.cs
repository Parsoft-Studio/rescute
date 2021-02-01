using rescute.Domain.Aggregates.TimelineEvents;
using System.Collections.Generic;

namespace rescute.Domain.Extensions
{
    public static class TimelineEventExtensions
    {
        /// <summary>
        /// Creates a deep copy of the list of <see cref="TimelineEvent"/>s by calling their <see cref="TimelineEvent.Clone"/> function and adding the result to a new list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TimelineEvent> DeepCopy(this List<TimelineEvent> list)
        {
            var result = new List<TimelineEvent>();
            foreach (TimelineEvent e in list)
            {
                result.Add(e.Clone() as TimelineEvent);
            }
            return result;
        }
    }
}
