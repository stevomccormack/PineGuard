using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure cron-expression validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/cron">Cron Rules documentation</seealso>
public static class CronRules
{
    /// <summary>
    /// The number of fields in a <see cref="CronFormat.Standard"/> expression (<c>5</c>).
    /// </summary>
    public const int StandardFieldCount = 5;

    /// <summary>
    /// The number of fields in a <see cref="CronFormat.WithSeconds"/> expression (<c>6</c>).
    /// </summary>
    public const int WithSecondsFieldCount = 6;

    /// <summary>
    /// The smallest step a <c>/</c> increment may specify (<c>1</c>).
    /// </summary>
    public const int MinStep = 1;

    /// <summary>
    /// The lowest value the seconds field accepts (<c>0</c>).
    /// </summary>
    public const int MinSecond = 0;

    /// <summary>
    /// The highest value the seconds field accepts (<c>59</c>).
    /// </summary>
    public const int MaxSecond = 59;

    /// <summary>
    /// The lowest value the minutes field accepts (<c>0</c>).
    /// </summary>
    public const int MinMinute = 0;

    /// <summary>
    /// The highest value the minutes field accepts (<c>59</c>).
    /// </summary>
    public const int MaxMinute = 59;

    /// <summary>
    /// The lowest value the hours field accepts (<c>0</c>).
    /// </summary>
    public const int MinHour = 0;

    /// <summary>
    /// The highest value the hours field accepts (<c>23</c>).
    /// </summary>
    public const int MaxHour = 23;

    /// <summary>
    /// The lowest value the day-of-month field accepts (<c>1</c>).
    /// </summary>
    public const int MinDayOfMonth = 1;

    /// <summary>
    /// The highest value the day-of-month field accepts (<c>31</c>).
    /// </summary>
    public const int MaxDayOfMonth = 31;

    /// <summary>
    /// The lowest value the month field accepts (<c>1</c>, January).
    /// </summary>
    public const int MinMonth = 1;

    /// <summary>
    /// The highest value the month field accepts (<c>12</c>, December).
    /// </summary>
    public const int MaxMonth = 12;

    /// <summary>
    /// The lowest value the day-of-week field accepts (<c>0</c>, Sunday).
    /// </summary>
    public const int MinDayOfWeek = 0;

    /// <summary>
    /// The highest value the day-of-week field accepts (<c>7</c>, which is Sunday again — the
    /// <c>crontab(5)</c> convention that lets a week be written either <c>0-6</c> or <c>1-7</c>).
    /// </summary>
    public const int MaxDayOfWeek = 7;

    /// <summary>
    /// Determines whether the specified value is a cron expression in the given format.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="format">The field layout to validate against. Defaults to <see cref="CronFormat.Standard"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a cron expression in <paramref name="format"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Fields are separated by spaces or tabs and each accepts <c>*</c>, a single value, a <c>-</c> range,
    /// a <c>,</c> list of either, and a <c>/</c> step on any of those. The month field also accepts
    /// <c>JAN</c>–<c>DEC</c> and the day-of-week field <c>SUN</c>–<c>SAT</c>, in any casing and anywhere a
    /// number would be accepted. Every value is checked against its own field's range, so <c>60</c> is
    /// rejected as a minute and <c>0</c> as a day of the month.
    /// </para>
    /// <para>
    /// Deliberately outside the v1 grammar: the <c>@yearly</c>-style macros, and the Quartz-only
    /// <c>?</c>, <c>L</c>, <c>W</c> and <c>#</c> characters. A descending range such as <c>5-1</c> is
    /// rejected rather than read as wrapping around the end of the field.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = CronRules.IsCronExpression("*/15 9-17 * * MON-FRI");                  // true
    /// bool seconds = CronRules.IsCronExpression("0 0 12 * * *", CronFormat.WithSeconds); // true
    /// bool invalid = CronRules.IsCronExpression("60 * * * *");                           // false (minute out of range)
    /// </code>
    /// </example>
    public static bool IsCronExpression(string? value, CronFormat format = CronFormat.Standard) =>
        CronUtility.TryParse(value, format, out _);
}
