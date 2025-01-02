using System;

namespace rescute.Shared;

/// <summary>
/// An abstraction over date and time related functions. 
/// </summary>
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