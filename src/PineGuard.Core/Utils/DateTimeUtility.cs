using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides date/time conversion, truncation, and comparison utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/datetime">DateTime Utility documentation</seealso>
public static class DateTimeUtility
{
    // TimeSpan.TicksPerMicrosecond only exists from .NET 7; the constant keeps netstandard2.1 building.
    private const long TicksPerMicrosecond = 10;

    /// <summary>
    /// Reads the current UTC instant from the supplied clock, falling back to the system clock.
    /// </summary>
    /// <param name="timeProvider">The clock to read. If <see langword="null"/>, <see cref="TimeProvider.System"/> is used.</param>
    /// <returns>The current instant reported by <paramref name="timeProvider"/>.</returns>
    /// <remarks>
    /// Every rule that compares a value against "now" reads the clock through this method, so a
    /// caller can pin the instant — in a test, or anywhere determinism matters — by supplying a
    /// <see cref="TimeProvider"/> instead of depending on the machine clock.
    /// <see cref="TimeProvider"/> ships in-box from .NET 8 onwards; on <c>netstandard2.1</c> it comes
    /// from the <c>Microsoft.Bcl.TimeProvider</c> package this project references for that target.
    /// </remarks>
    public static DateTimeOffset GetUtcNow(TimeProvider? timeProvider) =>
        (timeProvider ?? TimeProvider.System).GetUtcNow();

    /// <summary>
    /// Reads the current instant expressed in the clock's <see cref="TimeProvider.LocalTimeZone"/>.
    /// </summary>
    /// <param name="timeProvider">The clock to read. If <see langword="null"/>, <see cref="TimeProvider.System"/> is used.</param>
    /// <returns>The current instant reported by <paramref name="timeProvider"/>, expressed in its local time zone.</returns>
    public static DateTimeOffset GetLocalNow(TimeProvider? timeProvider) =>
        (timeProvider ?? TimeProvider.System).GetLocalNow();

    /// <summary>
    /// Attempts to derive today's calendar date, sampling the clock exactly once and guarding the projection
    /// against out-of-range results. With an injected clock the date is read in that clock's
    /// <see cref="TimeProvider.LocalTimeZone"/>; with no clock it is the UTC calendar date.
    /// </summary>
    /// <param name="timeProvider">
    /// The clock to read. If <see langword="null"/>, the system clock is sampled and today is its UTC calendar date.
    /// </param>
    /// <param name="today">
    /// When this method returns <see langword="true"/>, contains the calendar date, at midnight with
    /// <see cref="DateTimeKind.Unspecified"/> kind. Otherwise, <see langword="default"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the projected date is representable as a <see cref="DateTime"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This is the single guarded reference every calendar-relative rule reads "today" through, and it is
    /// where PineGuard's zone contract lives: <b>PineGuard never guesses the host's zone.</b> Without an
    /// injected clock, today is the UTC calendar date, so a rule answers the same on a developer machine, in
    /// CI and in production regardless of the process time zone. To evaluate "today" in a particular zone,
    /// inject a <see cref="TimeProvider"/> whose <see cref="TimeProvider.LocalTimeZone"/> is that zone. The
    /// method samples <see cref="TimeProvider.GetUtcNow"/> once and, for an injected clock, reads
    /// <see cref="TimeProvider.LocalTimeZone"/> once and obtains that zone's UTC offset at the sampled instant
    /// once, before delegating to <see cref="DateTimeOffsetUtility.TryGetDateAtOffset"/>. There is no public
    /// <c>GetTodayDate</c>/<c>GetToday</c> wrapper — new predicates call this method directly rather than
    /// reading <see cref="TimeProvider.GetLocalNow"/> unchecked.
    /// </remarks>
    internal static bool TryGetTodayDate(TimeProvider? timeProvider, out DateTime today)
    {
        var utcNow = GetUtcNow(timeProvider);
        var offset = timeProvider is null ? TimeSpan.Zero : timeProvider.LocalTimeZone.GetUtcOffset(utcNow);

        return DateTimeOffsetUtility.TryGetDateAtOffset(utcNow, offset, out today);
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Attempts to compute the whole number of completed years between a date of birth and a reference date.
    /// </summary>
    /// <param name="birthDate">The date of birth. If <see langword="null"/> or after <paramref name="asOf"/>, returns <see langword="false"/>.</param>
    /// <param name="asOf">The reference date "today" is measured against. Reads no clock.</param>
    /// <param name="years">
    /// When this method returns <see langword="true"/>, contains the completed age in whole years; otherwise, <c>0</c>.
    /// </param>
    /// <returns><see langword="true"/> if the age could be computed; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Computes the calendar-year difference and subtracts one when the birthday's month/day has not yet
    /// occurred in <paramref name="asOf"/>'s year, rather than applying <c>AddYears</c> to a threshold. A 29
    /// February birth date therefore reaches its next birthday on 1 March of a non-leap year, because that
    /// year has no 29th to match against — this is PineGuard's calendar convention.
    /// </remarks>
    internal static bool TryGetAge(DateOnly? birthDate, DateOnly asOf, out int years)
    {
        if (birthDate is null || birthDate.Value > asOf)
        {
            years = 0;
            return false;
        }

        var birth = birthDate.Value;
        var age = asOf.Year - birth.Year;
        if (birth.Month > asOf.Month || (birth.Month == asOf.Month && birth.Day > asOf.Day))
            age--;

        years = age;
        return true;
    }
#endif

    /// <summary>
    /// Attempts to compute the whole number of completed years between a date of birth and a reference date.
    /// </summary>
    /// <param name="birthDate">
    /// The date of birth, compared by its own <see cref="DateTime.Date"/> regardless of <see cref="DateTime.Kind"/>.
    /// If <see langword="null"/> or after <paramref name="asOf"/>'s date, returns <see langword="false"/>.
    /// </param>
    /// <param name="asOf">The reference date/time "today" is measured against. Reads no clock.</param>
    /// <param name="years">
    /// When this method returns <see langword="true"/>, contains the completed age in whole years; otherwise, <c>0</c>.
    /// </param>
    /// <returns><see langword="true"/> if the age could be computed; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Computes the calendar-year difference and subtracts one when the birthday's month/day has not yet
    /// occurred in <paramref name="asOf"/>'s year, rather than applying <c>AddYears</c> to a threshold. A 29
    /// February birth date therefore reaches its next birthday on 1 March of a non-leap year, because that
    /// year has no 29th to match against — this is PineGuard's calendar convention.
    /// </remarks>
    internal static bool TryGetAge(DateTime? birthDate, DateTime asOf, out int years)
    {
        if (birthDate is null || birthDate.Value.Date > asOf.Date)
        {
            years = 0;
            return false;
        }

        var birth = birthDate.Value.Date;
        var today = asOf.Date;
        var age = today.Year - birth.Year;
        if (birth.Month > today.Month || (birth.Month == today.Month && birth.Day > today.Day))
            age--;

        years = age;
        return true;
    }

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
            case DateTimePrecision.Microsecond:
                truncated = new DateTime(utc.Ticks - (utc.Ticks % TicksPerMicrosecond), DateTimeKind.Utc);
                return true;
            case DateTimePrecision.Tick:
                truncated = utc;
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
