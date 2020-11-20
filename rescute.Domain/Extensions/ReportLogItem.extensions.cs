using rescute.Domain.Entities.LogItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Extensions
{
    public static class ReportLogItemExtensions
    {
        /// <summary>
        /// Creates a deep copy of the list of <see cref="ReportLogItem"/>s by calling their <see cref="ReportLogItem.Clone"/> function and adding the result to a new list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<ReportLogItem> DeepCopy(this List<ReportLogItem> list)
        {
            var result = new List<ReportLogItem>();
            foreach (ReportLogItem e in list)
            {
                result.Add(e.Clone() as ReportLogItem);
            }
            return result;
        }
    }
}
