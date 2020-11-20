using System;
using System.Collections.Generic;
using System.Text;

namespace rescute.Domain.Entities.LogItems
{
    /// <summary>
    /// Represents an item that can be considered as a reply to a <see cref="ReportLogItem"/>.
    /// </summary>
    public interface ILogItemReplier
    {
        ReportLogItem RepliesTo { get; }
    }
}
