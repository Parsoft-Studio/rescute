using rescute.Domain.Entities.LogItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Extensions
{
    public static class ReportLogItemExtensions
    {
        /// <summary>
        /// Creates a deep copy of the list of <see cref="LogItem"/>s by calling their <see cref="LogItem.Clone"/> function and adding the result to a new list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<LogItem> DeepCopy(this List<LogItem> list)
        {
            var result = new List<LogItem>();
            foreach (LogItem e in list)
            {
                result.Add(e.Clone() as LogItem);
            }
            return result;
        }
    }
}
