namespace PineGuard.Common;

/// <summary>
/// Specifies the precision level for <c>DateOnly</c> truncation in comparisons.
/// </summary>
public enum DatePrecision
{
    /// <summary>
    /// Truncate to the year component.
    /// </summary>
    Year,

    /// <summary>
    /// Truncate to the month component.
    /// </summary>
    Month,

    /// <summary>
    /// Truncate to the day component (no truncation for <c>DateOnly</c>).
    /// </summary>
    Day
}

/// <summary>
/// Specifies the precision level for <c>TimeOnly</c> truncation in comparisons.
/// </summary>
public enum TimePrecision
{
    /// <summary>
    /// Truncate to the hour component.
    /// </summary>
    Hour,

    /// <summary>
    /// Truncate to the minute component.
    /// </summary>
    Minute,

    /// <summary>
    /// Truncate to the second component.
    /// </summary>
    Second,

    /// <summary>
    /// Truncate to the millisecond component.
    /// </summary>
    Millisecond,

    /// <summary>
    /// No truncation; compare at tick precision.
    /// </summary>
    Tick
}

/// <summary>
/// Specifies the precision level for <see cref="DateTime"/> and <see cref="DateTimeOffset"/> truncation in comparisons.
/// </summary>
public enum DateTimePrecision
{
    /// <summary>
    /// Truncate to the year component.
    /// </summary>
    Year,

    /// <summary>
    /// Truncate to the month component.
    /// </summary>
    Month,

    /// <summary>
    /// Truncate to the day component.
    /// </summary>
    Day,

    /// <summary>
    /// Truncate to the hour component.
    /// </summary>
    Hour,

    /// <summary>
    /// Truncate to the minute component.
    /// </summary>
    Minute,

    /// <summary>
    /// Truncate to the second component.
    /// </summary>
    Second,

    /// <summary>
    /// Truncate to the millisecond component.
    /// </summary>
    Millisecond
}
