#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for SQL Server date and datetime range validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/sql-datetime">Fluent SQL DateTime Extensions documentation</seealso>
public static class FluentSqlDateTimeExtensions
{
    /// <summary>
    /// Validates that the nullable <see cref="DateOnly"/> value falls within the SQL Server <c>date</c> type range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.BirthDate).InSqlDateRange();</code></example>
    /// <seealso cref="MustSqlDateTimeClauses.InSqlDateRange"/>
    public static IRuleBuilderOptions<TModel, DateOnly?> InSqlDateRange<TModel>(
        this IRuleBuilder<TModel, DateOnly?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.InSqlDateRange(val.Value, paramName: null)
                : MustResult<DateOnly>.Ok(default),
            message, MustCodes.Date.Sql.OutOfRange);

    /// <summary>
    /// Validates that the <see cref="DateOnly"/> value falls within the SQL Server <c>date</c> type range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateRange"/>.</remarks>
    /// <example><code>RuleFor(x => x.BirthDate).InSqlDateRange();</code></example>
    /// <seealso cref="MustSqlDateTimeClauses.InSqlDateRange"/>
    public static IRuleBuilderOptions<TModel, DateOnly> InSqlDateRange<TModel>(
        this IRuleBuilder<TModel, DateOnly> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.InSqlDateRange(val, paramName: null),
            message, MustCodes.Date.Sql.OutOfRange);

    /// <summary>
    /// Validates that the nullable <see cref="DateTimeOffset"/> value falls within the SQL Server <c>datetime</c> type range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTimeOffset, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.CreatedAt).InSqlDateTimeRange();</code></example>
    /// <seealso cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTimeOffset, string)"/>
    public static IRuleBuilderOptions<TModel, DateTimeOffset?> InSqlDateTimeRange<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
                ? Must.Be.InSqlDateTimeRange(val.Value, paramName: null)
                : MustResult<DateTimeOffset>.Ok(default),
            message, MustCodes.Date.Sql.OutOfRange);

    /// <summary>
    /// Validates that the <see cref="DateTimeOffset"/> value falls within the SQL Server <c>datetime</c> type range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTimeOffset, string)"/>.</remarks>
    /// <example><code>RuleFor(x => x.CreatedAt).InSqlDateTimeRange();</code></example>
    /// <seealso cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTimeOffset, string)"/>
    public static IRuleBuilderOptions<TModel, DateTimeOffset> InSqlDateTimeRange<TModel>(
        this IRuleBuilder<TModel, DateTimeOffset> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.InSqlDateTimeRange(val, paramName: null),
            message, MustCodes.Date.Sql.OutOfRange);

    /// <summary>
    /// Validates that the nullable <see cref="DateTime"/> value falls within the SQL Server <c>datetime</c> type range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTime, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.ModifiedAt).InSqlDateTimeRange();</code></example>
    /// <seealso cref="MustSqlDateTimeClauses.InSqlDateTimeRange(IMustClause, DateTime, string)"/>
    public static IRuleBuilderOptions<TModel, DateTime?> InSqlDateTimeRange<TModel>(
        this IRuleBuilder<TModel, DateTime?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue
            ? Must.Be.InSqlDateTimeRange(val.Value, paramName: null)
            : MustResult<DateTime>.Ok(default),
            message, MustCodes.Date.Sql.OutOfRange);
}
#endif
