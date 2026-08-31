using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides date/time conversion, truncation, and comparison utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/datetime">DateTime Utility documentation</seealso>
public static class DateTimeUtility
{
#if NET8_0_OR_GREATER
    // TimeProvider ships in-box from .NET 8 onwards but does not exist on netstandard2.1, which
    // would need a Microsoft.Bcl.TimeProvider package reference. This gate lifts — and the clock
    // seam extends to the DateTime and DateTimeOffset rules — once that reference lands.

    /// <summary>
    /// Reads the current UTC instant from the supplied clock, falling back to the system clock.
    /// </summary>
    /// <param name="timeProvider">The clock to read. If <see langword="null"/>, <see cref="TimeProvider.System"/> is used.</param>
    /// <returns>The current instant reported by <paramref name="timeProvider"/>.</returns>
    /// <remarks>
    /// Every rule that compares a value against "now" reads the clock through this method, so a
    /// caller can pin the instant — in a test, or anywhere determinism matters — by supplying a
    /// <see cref="TimeProvider"/> instead of depending on the machine clock.
    /// </remarks>
    public static DateTimeOffset GetUtcNow(TimeProvider? timeProvider) =>
        (timeProvider ?? TimeProvider.System).GetUtcNow();
#endif

    /// <summary>
    /// Attempts to convert the specified <see cref="DateTime"/> to UTC.
    /// </summary>
    /// <param name="value">The date/time to convert. If <see langword="null"/>, returns <see langword="null"/>.</param>
    /// <returns>The UTC-converted value, or <see langword="null"/> if <paramref name="value"/> is <see langword="null"/>.</returns>
    /// <remarks>
    /// <see cref="DateTimeKind.Local"/> values are converted via <see cref="DateTime.ToUniversalTime"/>.
    /// <see cref="DateTimeKind.Unspecified"/> values are deliberately assumed to already represent UTC and are
    /// relabeled via <see cref="DateTime.SpecifyKind(DateTime, DateTimeKind)"/> rather than converted —
    /// the opposite of <see cref="DateTime.ToUniversalTime"/>, which treats <see cref="DateTimeKind.Unspecified"/>
    /// as local. This convention underpins every comparison in <see cref="PineGuard.Rules.DateTimeRules"/> that
    /// normalizes via this method.
    /// </remarks>
    public static DateTime? ToUtc(DateTime? value)
    {
        if (value is null) return null;

        return value.Value.Kind switch
        {
            DateTimeKind.Local => value.Value.ToUniversalTime(),
            DateTimeKind.Utc => value.Value,
            _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        };
    }

    /// <summary>
    /// Attempts to truncate a <see cref="DateTime"/> to the specified precision in UTC.
    /// </summary>
    /// <param name="value">The date/time to truncate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">The precision level to truncate to.</param>
    /// <param name="truncated">When this method returns, contains the truncated value if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if truncation succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryTruncateToPrecisionUtc(DateTime? value, DateTimePrecision precision, out DateTime? truncated)
    {
        if (value is null)
        {
            truncated = null;
            return false;
        }

        var utc = ToUtc(value)!.Value;

        switch (precision)
        {
            case DateTimePrecision.Year:
                truncated = new DateTime(utc.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Month:
                truncated = new DateTime(utc.Year, utc.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Day:
                truncated = new DateTime(utc.Year, utc.Month, utc.Day, 0, 0, 0, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Hour:
                truncated = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Minute:
                truncated = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, 0, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Second:
                truncated = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, 0, DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Millisecond:
                truncated = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Millisecond, DateTimeKind.Utc);
                return true;
            default:
                truncated = null;
                return false;
        }
    }

    /// <summary>
    /// Attempts to truncate a <see cref="DateTimeOffset"/> to the specified precision in UTC.
    /// </summary>
    /// <param name="value">The date/time offset to truncate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">The precision level to truncate to.</param>
    /// <param name="truncated">When this method returns, contains the truncated value if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if truncation succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryTruncateToPrecisionUtc(DateTimeOffset? value, DateTimePrecision precision, out DateTimeOffset? truncated)
    {
        if (value is null || !TryTruncateToPrecisionUtc(value.Value.UtcDateTime, precision, out var truncatedUtc))
        {
            truncated = null;
            return false;
        }

        truncated = new DateTimeOffset(truncatedUtc!.Value, TimeSpan.Zero);
        return true;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Attempts to truncate a <see cref="DateOnly"/> to the specified precision.
    /// </summary>
    /// <param name="value">The date to truncate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">The precision level to truncate to.</param>
    /// <param name="truncated">When this method returns, contains the truncated value if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if truncation succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool TryTruncateToPrecision(DateOnly? value, DatePrecision precision, out DateOnly? truncated)
    {
        if (value is null)
        {
            truncated = null;
            return false;
        }

        switch (precision)
        {
            case DatePrecision.Year:
                truncated = new DateOnly(value.Value.Year, 1, 1);
                return true;
            case DatePrecision.Month:
                truncated = new DateOnly(value.Value.Year, value.Value.Month, 1);
                return true;
            case DatePrecision.Day:
                truncated = value;
                return true;
            default:
                truncated = null;
                return false;
        }
    }
#endif

    /// <summary>
    /// Computes the difference between two date/times after converting both to UTC.
    /// </summary>
    /// <param name="value">The first date/time. If <see langword="null"/>, returns <see langword="null"/>.</param>
    /// <param name="other">The second date/time. If <see langword="null"/>, returns <see langword="null"/>.</param>
    /// <returns>The <see cref="TimeSpan"/> difference, or <see langword="null"/> if either input is <see langword="null"/>.</returns>
    public static TimeSpan? Diff(DateTime? value, DateTime? other)
    {
        if (value is null || other is null) return null;
        return ToUtc(value)!.Value - ToUtc(other)!.Value;
    }
}
