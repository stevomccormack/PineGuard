#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate date and date/time values against
/// SQL Server's supported date/time ranges.
/// </summary>
/// <seealso cref="SqlDateTimeRules"/>
/// <seealso href="https://pineguard.ai/docs/must/sql-datetime">SQL DateTime Must Clauses documentation</seealso>
public static class MustSqlDateTimeClauses
{
    /// <summary>
    /// Validates that the specified <see cref="DateOnly"/> value falls within the SQL Server <c>date</c> column range
    /// (0001-01-01 to 9999-12-31).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="DateOnly"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is within the SQL date range, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="SqlDateTimeRules.IsInSqlDateRange"/>. The failure message follows the pattern
    /// <c>"{paramName} must be within the SQL date range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.InSqlDateRange(birthDate);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="SqlDateTimeRules.IsInSqlDateRange"/>
    /// <seealso href="https://pineguard.ai/docs/must/sql-datetime">SQL DateTime Must Clauses documentation</seealso>
    public static MustResult<DateOnly> InSqlDateRange(this IMustClause _,
        DateOnly value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the SQL date range.";

        var ok = SqlDateTimeRules.IsInSqlDateRange(value);
        return MustResult<DateOnly>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="DateTimeOffset"/> value falls within the SQL Server <c>datetime2</c>
    /// column range (0001-01-01 to 9999-12-31).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="DateTimeOffset"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is within the SQL datetime range, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="SqlDateTimeRules.IsInSqlDateTimeRange(Nullable{DateTimeOffset})"/>. The failure message follows
    /// the pattern <c>"{paramName} must be within the SQL date/time range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.InSqlDateTimeRange(createdAt);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="SqlDateTimeRules.IsInSqlDateTimeRange(Nullable{DateTimeOffset})"/>
    /// <seealso href="https://pineguard.ai/docs/must/sql-datetime">SQL DateTime Must Clauses documentation</seealso>
    public static MustResult<DateTimeOffset> InSqlDateTimeRange(this IMustClause _,
        DateTimeOffset value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the SQL date/time range.";

        var ok = SqlDateTimeRules.IsInSqlDateTimeRange(value);
        return MustResult<DateTimeOffset>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="DateTime"/> value falls within the SQL Server <c>datetime2</c>
    /// column range (0001-01-01 to 9999-12-31).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="DateTime"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is within the SQL datetime range, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="SqlDateTimeRules.IsInSqlDateTimeRange(Nullable{DateTime})"/>. The failure message follows
    /// the pattern <c>"{paramName} must be within the SQL date/time range."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.InSqlDateTimeRange(updatedAt);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="SqlDateTimeRules.IsInSqlDateTimeRange(Nullable{DateTime})"/>
    /// <seealso href="https://pineguard.ai/docs/must/sql-datetime">SQL DateTime Must Clauses documentation</seealso>
    public static MustResult<DateTime> InSqlDateTimeRange(this IMustClause _,
        DateTime value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be within the SQL date/time range.";

        var ok = SqlDateTimeRules.IsInSqlDateTimeRange(value);
        return MustResult<DateTime>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
#endif
