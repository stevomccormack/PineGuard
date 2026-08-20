using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides validation predicates for SQL Server-compatible date and datetime ranges.
/// </summary>
/// <remarks>
/// SQL Server's <c>datetime</c> type supports dates from 1753-01-01 to 9999-12-31.
/// Use these rules before persisting dates to SQL Server <c>datetime</c> columns.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/sqldatetime">SQL DateTime Rules documentation</seealso>
public static class SqlDateTimeRules
{
    /// <summary>
    /// The minimum value supported by the SQL Server <c>datetime</c> type (1753-01-01 00:00:00).
    /// </summary>
    public static readonly DateTime MinValue = new(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

    /// <summary>
    /// The maximum value supported by the SQL Server <c>datetime</c> type (9999-12-31 23:59:59.997).
    /// </summary>
    public static readonly DateTime MaxValue = new(9999, 12, 31, 23, 59, 59, 997, DateTimeKind.Unspecified);

#if NET8_0_OR_GREATER
    /// <summary>
    /// Determines whether the specified date falls within the SQL Server <c>datetime</c> type's date range
    /// (1753-01-01 to 9999-12-31), the conservative bound enforced by this rule.
    /// </summary>
    /// <param name="value">The date to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is on or after 1753-01-01, the SQL Server <c>datetime</c>
    /// minimum; otherwise, <see langword="false"/>. Note this is stricter than the SQL Server <c>date</c> type
    /// itself, which supports 0001-01-01 through 9999-12-31.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = SqlDateTimeRules.IsInSqlDateRange(new DateOnly(2024, 1, 1)); // true
    /// </code>
    /// </example>
    public static bool IsInSqlDateRange(DateOnly? value) =>
        SqlDateTimeUtility.TryCreateSqlDateOnly(value, out _);
#endif

    /// <summary>
    /// Determines whether the specified <see cref="DateTimeOffset"/> falls within the SQL Server
    /// <c>datetime</c> type range (1753-01-01 to 9999-12-31).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is within the SQL Server <c>datetime</c> range;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsInSqlDateTimeRange(DateTimeOffset? value) =>
        SqlDateTimeUtility.TryCreateSqlDateTime(value, out _);

    /// <summary>
    /// Determines whether the specified <see cref="DateTime"/> falls within the SQL Server
    /// <c>datetime</c> type range (1753-01-01 to 9999-12-31).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is within the SQL Server <c>datetime</c> range;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsInSqlDateTimeRange(DateTime? value) =>
        SqlDateTimeUtility.TryCreateSqlDateTime(value, out _);
}
