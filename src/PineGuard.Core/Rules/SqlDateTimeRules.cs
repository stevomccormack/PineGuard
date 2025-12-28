using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static class SqlDateTimeRules
{
    // Based on System.Data.SqlTypes.SqlDateTime.MinValue/MaxValue.
    // Kept as DateTime values to avoid System.Data dependency.
    public static readonly DateTime MinValue = new(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
    public static readonly DateTime MaxValue = new(9999, 12, 31, 23, 59, 59, 997, DateTimeKind.Unspecified);


    public static bool IsInSqlDateRange(DateOnly? value) =>
        SqlDateTimeUtility.TryCreateSqlDateOnly(value, out _);

    public static bool IsInSqlDateTimeRange(DateTimeOffset? value) =>
        SqlDateTimeUtility.TryCreateSqlDateTime(value, out _);

    public static bool IsInSqlDateTimeRange(DateTime? value) =>
        SqlDateTimeUtility.TryCreateSqlDateTime(value, out _);
}
