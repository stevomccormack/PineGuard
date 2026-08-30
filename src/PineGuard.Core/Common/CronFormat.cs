namespace PineGuard.Common;

/// <summary>
/// Specifies how many fields a cron expression is expected to carry.
/// </summary>
public enum CronFormat
{
    /// <summary>
    /// The five-field form used by <c>crontab</c>: minute, hour, day of month, month, day of week.
    /// </summary>
    Standard,

    /// <summary>
    /// The six-field form used by Quartz-style schedulers: a leading seconds field followed by the
    /// five <see cref="Standard"/> fields.
    /// </summary>
    WithSeconds
}
