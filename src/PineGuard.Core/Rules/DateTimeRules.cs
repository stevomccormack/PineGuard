using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="DateTime"/> validation predicates for temporal comparisons, range checks, and calendar queries.
/// </summary>
/// <remarks>
/// <para>
/// Members that compare instants — <see cref="IsInPast"/>, <see cref="IsInFuture"/>, <see cref="IsInRange"/>,
/// <see cref="IsBefore"/>, <see cref="IsAfter"/>, <see cref="IsSame"/>, <see cref="IsChronological"/>,
/// <see cref="IsOverlapping"/>, <see cref="IsWithin"/>, <see cref="IsWithinDaysFromNow"/> and
/// <see cref="IsWithinCalendarMonths"/> (which then counts months on the UTC calendar) — first normalize every
/// <see cref="DateTime"/> through <see cref="DateTimeUtility.ToUtc"/>: a <see cref="DateTimeKind.Local"/> value is
/// converted to UTC, a <see cref="DateTimeKind.Utc"/> value is used as-is, and a <see cref="DateTimeKind.Unspecified"/>
/// value is assumed to already be UTC and is relabeled, not converted. "Now" for <see cref="IsInPast"/>,
/// <see cref="IsInFuture"/> and <see cref="IsWithinDaysFromNow"/> is <see cref="TimeProvider.GetUtcNow"/> on the
/// injected <see cref="TimeProvider"/> (the system clock when <see langword="null"/>).
/// </para>
/// <para>
/// Members that answer calendar questions — <see cref="HasMinimumAge"/>, <see cref="IsSameDay"/>,
/// <see cref="IsWeekday"/>, <see cref="IsWeekend"/>, <see cref="IsFirstDayOfMonth"/> and
/// <see cref="IsLastDayOfMonth"/> — read the value's own civil <see cref="DateTime.Date"/> fields and ignore
/// <see cref="DateTime.Kind"/> entirely; no conversion takes place. "Today" for <see cref="HasMinimumAge"/> is the
/// calendar date in the injected <see cref="TimeProvider"/>'s <see cref="TimeProvider.LocalTimeZone"/>, or the UTC
/// calendar date when the provider is <see langword="null"/>: PineGuard never guesses the host machine's zone. The
/// kind inspectors <see cref="IsUtc"/>,
/// <see cref="IsLocal"/>, <see cref="IsUnspecified"/> and <see cref="HasExplicitKind"/> belong to neither group:
/// they report <see cref="DateTime.Kind"/> exactly as supplied.
/// </para>
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/datetime">DateTime Rules documentation</seealso>
public static class DateTimeRules
{
    /// <summary>
    /// Determines whether the specified date/time is in the past relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the past; otherwise, <see langword="false"/>.</returns>
    public static bool IsInPast(DateTime? value, Inclusion inclusion = Inclusion.Exclusive, TimeProvider? timeProvider = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return IsBefore(value, DateTimeUtility.GetUtcNow(timeProvider).UtcDateTime, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time is in the future relative to UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the current instant is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in the future; otherwise, <see langword="false"/>.</returns>
    public static bool IsInFuture(DateTime? value, Inclusion inclusion = Inclusion.Exclusive, TimeProvider? timeProvider = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return IsAfter(value, DateTimeUtility.GetUtcNow(timeProvider).UtcDateTime, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time falls within [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the date/time range.</param>
    /// <param name="max">The upper bound of the date/time range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public static bool IsInRange(DateTime? value, DateTime min, DateTime max,
        Inclusion inclusion = Inclusion.Inclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (value is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var minUtc = DateTimeUtility.ToUtc(min)!.Value;
        var maxUtc = DateTimeUtility.ToUtc(max)!.Value;

        return RuleComparison.IsInRange(valueUtc, minUtc, maxUtc, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time is before the given reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is before <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsBefore(DateTime? value, DateTime other, Inclusion inclusion = Inclusion.Exclusive,
        DateTimePrecision? precision = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (precision.HasValue && !EnumRules.IsDefined<DateTimePrecision>(precision.Value))
            return false;

        if (value is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.IsLessThan(valueUtc, otherUtc, inclusion);

        _ = DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue);
        _ = DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther);

        valueUtc = tValue;
        otherUtc = tOther;

        return RuleComparison.IsLessThan(valueUtc, otherUtc, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time is after the given reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="other">The reference date/time.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is after <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsAfter(DateTime? value, DateTime other, Inclusion inclusion = Inclusion.Exclusive,
        DateTimePrecision? precision = null)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        if (precision.HasValue && !EnumRules.IsDefined<DateTimePrecision>(precision.Value))
            return false;

        if (value is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.IsGreaterThan(valueUtc, otherUtc, inclusion);

        _ = DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue);
        _ = DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther);

        valueUtc = tValue;
        otherUtc = tOther;

        return RuleComparison.IsGreaterThan(valueUtc, otherUtc, inclusion);
    }

    /// <summary>
    /// Determines whether two date/times are the same, optionally truncated to the given precision.
    /// </summary>
    /// <param name="value">The first date/time. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date/time.</param>
    /// <param name="precision">Optional precision for date/time truncation before comparison.</param>
    /// <returns><see langword="true"/> if the date/times are equal at the given precision; otherwise, <see langword="false"/>.</returns>
    public static bool IsSame(DateTime? value, DateTime? other, DateTimePrecision? precision = null)
    {
        if (precision.HasValue && !EnumRules.IsDefined<DateTimePrecision>(precision.Value))
            return false;

        if (value is null && other is null)
            return true;

        if (value is null || other is null)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var otherUtc = DateTimeUtility.ToUtc(other)!.Value;

        if (precision is null)
            return RuleComparison.Equals(valueUtc, otherUtc);

        _ = DateTimeUtility.TryTruncateToPrecisionUtc(valueUtc, precision.Value, out var tValue);
        _ = DateTimeUtility.TryTruncateToPrecisionUtc(otherUtc, precision.Value, out var tOther);

        valueUtc = tValue;
        otherUtc = tOther;

        return RuleComparison.Equals(valueUtc, otherUtc);
    }

    /// <summary>
    /// Determines whether the start date/time precedes the end date/time (equality permitted when <paramref name="inclusion"/> is <see cref="Inclusion.Inclusive"/>).
    /// </summary>
    /// <param name="start">The start date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether the boundary is inclusive or exclusive. Defaults to <see cref="Inclusion.Exclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="start"/> precedes <paramref name="end"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsChronological(DateTime? start, DateTime? end, Inclusion inclusion = Inclusion.Exclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        return RangeRules.IsChronological(DateTimeUtility.ToUtc(start), DateTimeUtility.ToUtc(end), inclusion);
    }

    /// <summary>
    /// Determines whether two date/time ranges overlap.
    /// </summary>
    /// <param name="start1">The start of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end1">The end of the first range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="start2">The start of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end2">The end of the second range. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="inclusion">Whether to intersect closed ranges or open interiors. Singleton and one-step interiors are empty. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if the two ranges overlap; otherwise, <see langword="false"/>. Returns <see langword="false"/> if either range is inverted (start after end).</returns>
    public static bool IsOverlapping(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2,
        Inclusion inclusion = Inclusion.Inclusive)
    {
        TemporalRangeComparison.ValidateInclusion(inclusion);
        var start1Utc = DateTimeUtility.ToUtc(start1);
        var end1Utc = DateTimeUtility.ToUtc(end1);
        var start2Utc = DateTimeUtility.ToUtc(start2);
        var end2Utc = DateTimeUtility.ToUtc(end2);

        return RangeRules.IsOverlapping(start1Utc?.Ticks, end1Utc?.Ticks, start2Utc?.Ticks, end2Utc?.Ticks, inclusion);
    }

    /// <summary>
    /// Determines whether the specified date/time is within the given number of days from UTC now.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="days">The maximum number of days allowed. If negative, returns <see langword="false"/>.</param>
    /// <param name="timeProvider">The clock that supplies the current instant. If <see langword="null"/>, the system clock is used.</param>
    /// <returns><see langword="true"/> if the absolute day difference from UTC now is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinDaysFromNow(DateTime? value, int days, TimeProvider? timeProvider = null)
    {
        if (value is null || days < 0)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var diffDays = Math.Abs((valueUtc - DateTimeUtility.GetUtcNow(timeProvider).UtcDateTime).TotalDays);
        return diffDays <= days;
    }

    /// <summary>
    /// Determines whether the specified date/time is within a given time window of a reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="window">The maximum allowed time difference. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the time difference is within the window; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithin(DateTime? value, DateTime? reference, TimeSpan window)
    {
        if (value is null || reference is null)
            return false;

        if (window < TimeSpan.Zero)
            return false;

        var diff = DateTimeUtility.Diff(value, reference);
        var diffTicks = diff!.Value.Ticks;
        var absTicks = Math.Abs(diffTicks);

        return absTicks <= window.Ticks;
    }

    /// <summary>
    /// Determines whether the specified date/time is within the given number of calendar months from a reference date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="reference">The reference date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="months">The maximum number of calendar months allowed. If negative, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the month difference is within the limit; otherwise, <see langword="false"/>.</returns>
    public static bool IsWithinCalendarMonths(DateTime? value, DateTime? reference, int months)
    {
        if (value is null || reference is null)
            return false;

        if (months < 0)
            return false;

        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        var referenceUtc = DateTimeUtility.ToUtc(reference)!.Value;
        var valueMonthIndex = (valueUtc.Year * 12) + valueUtc.Month;
        var referenceMonthIndex = (referenceUtc.Year * 12) + referenceUtc.Month;

        var monthDiff = Math.Abs(valueMonthIndex - referenceMonthIndex);
        return monthDiff <= months;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on a weekday (Monday through Friday).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekday; otherwise, <see langword="false"/>.</returns>
    public static bool IsWeekday(DateTime? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a weekend day; otherwise, <see langword="false"/>.</returns>
    public static bool IsWeekend(DateTime? value)
    {
        // ReSharper disable once UseNullPropagation
        if (value is null)
            return false;

        var dayOfWeek = value.Value.DayOfWeek;
        return dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on the first day of its month.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the first day of the month; otherwise, <see langword="false"/>.</returns>
    public static bool IsFirstDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        return value.Value.Day == 1;
    }

    /// <summary>
    /// Determines whether the specified date/time falls on the last day of its month.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is the last day of the month; otherwise, <see langword="false"/>.</returns>
    public static bool IsLastDayOfMonth(DateTime? value)
    {
        if (value is null)
            return false;

        var date = value.Value;
        return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
    }

    /// <summary>
    /// Determines whether two date/times fall on the same calendar day, compared by their own calendar
    /// date; <see cref="DateTime.Kind"/> is ignored.
    /// </summary>
    /// <param name="value">The first date/time. If <see langword="null"/> and <paramref name="other"/> is also <see langword="null"/>, returns <see langword="true"/>.</param>
    /// <param name="other">The second date/time.</param>
    /// <returns><see langword="true"/> if both values have the same calendar date; otherwise, <see langword="false"/>.</returns>
    public static bool IsSameDay(DateTime? value, DateTime? other)
    {
        if (value is null && other is null) return true;
        if (value is null || other is null) return false;

        return value.Value.Date == other.Value.Date;
    }

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Utc"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is UTC; otherwise, <see langword="false"/>.</returns>
    public static bool IsUtc(DateTime? value) => value is not null && value.Value.Kind == DateTimeKind.Utc;

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Local"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is local; otherwise, <see langword="false"/>.</returns>
    public static bool IsLocal(DateTime? value) => value is not null && value.Value.Kind == DateTimeKind.Local;

    /// <summary>
    /// Determines whether the specified date/time has <see cref="DateTimeKind.Unspecified"/> kind.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> has unspecified kind; otherwise, <see langword="false"/>.</returns>
    public static bool IsUnspecified(DateTime? value) =>
        value is not null && value.Value.Kind == DateTimeKind.Unspecified;

    /// <summary>
    /// Checks whether the specified date/time has an explicit <see cref="DateTimeKind"/> (not <see cref="DateTimeKind.Unspecified"/>).
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> has an explicit kind; otherwise, <see langword="false"/>.</returns>
    public static bool HasExplicitKind(DateTime? value) =>
        value is not null && value.Value.Kind != DateTimeKind.Unspecified;

    /// <summary>
    /// Determines whether the specified date of birth is at least <paramref name="years"/> whole years ago.
    /// </summary>
    /// <param name="value">The date of birth to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="years">The minimum age in whole years. Negative returns <see langword="false"/>.</param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its UTC
    /// calendar date; inject a provider to evaluate today in its <see cref="TimeProvider.LocalTimeZone"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> meets the minimum age; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Age is the whole number of completed years between the birth date and today: the year difference,
    /// minus one if the birthday's month and day have not yet occurred this year. A 29 February birth date
    /// therefore reaches its next birthday on 1 March of a non-leap year, because that year has no 29th to
    /// match against — this is PineGuard's calendar convention. The birth date is always its own
    /// <see cref="DateTime.Date"/>, regardless of <see cref="DateTime.Kind"/> — it is never converted.
    /// "Today" is derived from one sampled instant with checked projection into the clock's
    /// <see cref="TimeProvider.LocalTimeZone"/>. A <see langword="null"/> <paramref name="timeProvider"/>
    /// reads the system clock and uses the UTC calendar date: PineGuard never guesses the host's zone, so
    /// inject a provider to evaluate today in a chosen zone. If the projection is not representable, this
    /// method returns <see langword="false"/>.
    /// </remarks>
    public static bool HasMinimumAge(DateTime? value, int years, TimeProvider? timeProvider = null)
    {
        if (value is null || years < 0)
            return false;

        if (!DateTimeUtility.TryGetTodayDate(timeProvider, out var today))
            return false;

        return DateTimeUtility.TryGetAge(value.Value.Date, today, out var age) && age >= years;
    }
}
