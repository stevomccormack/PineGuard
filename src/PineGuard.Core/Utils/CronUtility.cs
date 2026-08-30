using System.Globalization;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides cron-expression parsing utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/cron">Cron Utility documentation</seealso>
public static class CronUtility
{
    private const char ListSeparator = ',';
    private const char RangeSeparator = '-';
    private const char StepSeparator = '/';
    private const string AnyValue = "*";

    private static readonly char[] FieldSeparators = [' ', '\t'];

    private static readonly string[] MonthNames = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

    private static readonly string[] DayOfWeekNames = ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"];

    private static readonly FieldSpec[] FieldSpecs =
    [
        new(CronRules.MinSecond, CronRules.MaxSecond, null, 0),
        new(CronRules.MinMinute, CronRules.MaxMinute, null, 0),
        new(CronRules.MinHour, CronRules.MaxHour, null, 0),
        new(CronRules.MinDayOfMonth, CronRules.MaxDayOfMonth, null, 0),
        new(CronRules.MinMonth, CronRules.MaxMonth, MonthNames, CronRules.MinMonth),
        new(CronRules.MinDayOfWeek, CronRules.MaxDayOfWeek, DayOfWeekNames, CronRules.MinDayOfWeek)
    ];

    /// <summary>
    /// Attempts to split the specified value into the fields of a cron expression, validating each one.
    /// </summary>
    /// <param name="value">The expression to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="format">The field layout to parse against.</param>
    /// <param name="fields">
    /// When this method returns, contains the fields in expression order if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a cron expression in <paramref name="format"/>; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The fields are returned exactly as they were written, because normalising them would mean choosing a
    /// canonical form — expanding <c>MON-FRI</c> to <c>1-5</c>, or <c>*/15</c> to a list — and a caller that
    /// wants the firing times needs a scheduler, not a splitter. What this returns is the field a caller can
    /// address by position: <c>fields[0]</c> is the minute under <see cref="CronFormat.Standard"/> and the
    /// second under <see cref="CronFormat.WithSeconds"/>.
    /// </para>
    /// <para>
    /// The grammar accepted is described on <see cref="CronRules.IsCronExpression"/>.
    /// </para>
    /// </remarks>
    public static bool TryParse(string? value, CronFormat format, out IReadOnlyList<string>? fields)
    {
        fields = null;

        if (!TryGetFieldCount(format, out var fieldCount))
            return false;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var parsed = trimmed.Split(FieldSeparators, StringSplitOptions.RemoveEmptyEntries);

        if (parsed.Length != fieldCount)
            return false;

        var specOffset = FieldSpecs.Length - fieldCount;

        for (var index = 0; index < parsed.Length; index++)
        {
            if (!IsField(parsed[index], FieldSpecs[specOffset + index]))
                return false;
        }

        fields = parsed;
        return true;
    }

    private static bool TryGetFieldCount(CronFormat format, out int fieldCount)
    {
        fieldCount = format switch
        {
            CronFormat.Standard => CronRules.StandardFieldCount,
            CronFormat.WithSeconds => CronRules.WithSecondsFieldCount,
            _ => 0
        };

        return fieldCount > 0;
    }

    private static bool IsField(string field, FieldSpec spec)
    {
        foreach (var item in field.Split(ListSeparator))
        {
            if (!IsItem(item, spec))
                return false;
        }

        return true;
    }

    private static bool IsItem(string item, FieldSpec spec)
    {
        var range = item;
        var stepIndex = item.IndexOf(StepSeparator);

        if (stepIndex >= 0)
        {
            range = item[..stepIndex];

            if (!IsStep(item[(stepIndex + 1)..]))
                return false;
        }

        if (range == AnyValue)
            return true;

        var rangeIndex = range.IndexOf(RangeSeparator);

        if (rangeIndex < 0)
            return TryGetValue(range, spec, out _);

        return TryGetValue(range[..rangeIndex], spec, out var start)
            && TryGetValue(range[(rangeIndex + 1)..], spec, out var end)
            && start <= end;
    }

    private static bool IsStep(string step) =>
        int.TryParse(step, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed) && parsed >= CronRules.MinStep;

    private static bool TryGetValue(string text, FieldSpec spec, out int value)
    {
        value = 0;

        if (spec.Names is not null && TryGetNamedValue(text, spec.Names, spec.NameOffset, out value))
            return true;

        return int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value)
            && value >= spec.MinValue
            && value <= spec.MaxValue;
    }

    private static bool TryGetNamedValue(string text, string[] names, int nameOffset, out int value)
    {
        for (var index = 0; index < names.Length; index++)
        {
            if (!string.Equals(names[index], text, StringComparison.OrdinalIgnoreCase))
                continue;

            value = nameOffset + index;
            return true;
        }

        value = 0;
        return false;
    }

    private readonly record struct FieldSpec(int MinValue, int MaxValue, string[]? Names, int NameOffset);
}
