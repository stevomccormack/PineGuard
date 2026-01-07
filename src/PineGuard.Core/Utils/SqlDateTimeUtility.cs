using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.Utils;

public static class SqlDateTimeUtility
{
    public static bool TryCreateSqlDateOnly(DateOnly? value, out DateOnly sqlDate)
    {
        sqlDate = default;

        if (value is null)
            return false;

        var dt = value.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
        if (!RuleComparison.IsBetween(dt, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue, Inclusion.Inclusive))
            return false;

        sqlDate = value.Value;
        return true;
    }

    public static bool TryCreateSqlDateTime(DateTimeOffset? value, out DateTimeOffset sqlDateTime)
    {
        sqlDateTime = default;

        if (value is null)
            return false;

        var v = value.Value;

        // SQL datetime is stored without offset; compare as instant in UTC.
        var utc = v.UtcDateTime;
        if (!RuleComparison.IsBetween(utc, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue, Inclusion.Inclusive))
            return false;

        sqlDateTime = v;
        return true;
    }

    public static bool TryCreateSqlDateTime(DateTime? value, out DateTime sqlDateTime)
    {
        sqlDateTime = default;

        if (value is null)
            return false;

        var v = value.Value;
        if (!RuleComparison.IsBetween(v, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue, Inclusion.Inclusive))
            return false;

        sqlDateTime = v;
        return true;
    }
}
