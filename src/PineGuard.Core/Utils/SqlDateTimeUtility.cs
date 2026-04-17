using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.Utils;

/// <summary>
/// Provides SQL Server date/time range validation and conversion utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/sqldatetime">SQL DateTime Utility documentation</seealso>
public static class SqlDateTimeUtility
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// Attempts to validate and convert a <see cref="DateOnly"/> to a SQL-compatible date.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="sqlDate">When this method returns, contains the validated date if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the date falls within the SQL Server date range; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreateSqlDateOnly(DateOnly? value, out DateOnly sqlDate)
    {
        sqlDate = default;

        if (value is null)
            return false;

        var dt = value.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
        if (!RuleComparison.IsBetween(dt, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue))
            return false;

        sqlDate = value.Value;
        return true;
    }
#endif

    /// <summary>
    /// Attempts to validate and convert a <see cref="DateTimeOffset"/> to a SQL-compatible date/time.
    /// </summary>
    /// <param name="value">The date/time offset to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="sqlDateTime">When this method returns, contains the validated value if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the value falls within the SQL Server datetime range; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreateSqlDateTime(DateTimeOffset? value, out DateTimeOffset sqlDateTime)
    {
        sqlDateTime = default;

        if (value is null)
            return false;

        var v = value.Value;

        // SQL datetime is stored without offset; compare as instant in UTC.
        var utc = v.UtcDateTime;
        if (!RuleComparison.IsBetween(utc, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue))
            return false;

        sqlDateTime = v;
        return true;
    }

    /// <summary>
    /// Attempts to validate and convert a <see cref="DateTime"/> to a SQL-compatible date/time.
    /// </summary>
    /// <param name="value">The date/time to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="sqlDateTime">When this method returns, contains the validated value if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the value falls within the SQL Server datetime range; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreateSqlDateTime(DateTime? value, out DateTime sqlDateTime)
    {
        sqlDateTime = default;

        if (value is null)
            return false;

        var v = value.Value;
        if (!RuleComparison.IsBetween(v, SqlDateTimeRules.MinValue, SqlDateTimeRules.MaxValue))
            return false;

        sqlDateTime = v;
        return true;
    }
}
