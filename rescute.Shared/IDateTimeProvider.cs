using System;

namespace rescute.Shared;

public interface IDateTimeProvider
{
    /// <summary>
    /// Returns the current date/time, including zone information.
    /// </summary>
    DateTime Now { get; }
    /// <summary>
    /// Returns the current date/time, without zone information.
    /// </summary>
    DateTime UtcNow { get; }
}