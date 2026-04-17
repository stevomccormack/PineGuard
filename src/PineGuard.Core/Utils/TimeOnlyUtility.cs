#if NET8_0_OR_GREATER
using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides <see cref="TimeOnly"/> truncation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/timeonly">TimeOnly Utility documentation</seealso>
public static class TimeOnlyUtility
{
    /// <summary>
    /// Attempts to truncate a <see cref="TimeOnly"/> to the specified precision.
    /// </summary>
    /// <param name="value">The time to truncate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">The precision level to truncate to.</param>
    /// <param name="truncated">When this method returns, contains the truncated value if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if truncation succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryTruncateToPrecision(TimeOnly? value, TimePrecision precision, out TimeOnly truncated)
    {
        if (value is not { } v)
        {
            truncated = default;
            return false;
        }

        truncated = precision switch
        {
            TimePrecision.Hour => new TimeOnly(v.Hour, 0, 0, 0),
            TimePrecision.Minute => new TimeOnly(v.Hour, v.Minute, 0, 0),
            TimePrecision.Second => new TimeOnly(v.Hour, v.Minute, v.Second, 0),
            TimePrecision.Millisecond => new TimeOnly(v.Ticks - (v.Ticks % TimeSpan.TicksPerMillisecond)),
            TimePrecision.Tick => v,
            _ => default
        };

        return precision is TimePrecision.Hour or TimePrecision.Minute or TimePrecision.Second or TimePrecision.Millisecond or TimePrecision.Tick;
    }
}
#endif
