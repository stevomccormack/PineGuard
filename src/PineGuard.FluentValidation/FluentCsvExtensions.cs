using FluentValidation;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for CSV property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/csv">Fluent CSV Extensions documentation</seealso>
public static class FluentCsvExtensions
{
    /// <summary>
    /// Validates that the string value is a well-formed CSV line.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCsvClauses.CsvLine"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.CsvRow).CsvLine();</code></example>
    /// <seealso cref="MustCsvClauses.CsvLine"/>
    public static IRuleBuilderOptions<TModel, string?> CsvLine<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CsvLine(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a well-formed CSV header line matching the expected columns.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="expectedHeader">The expected header column names to match against.</param>
    /// <param name="separator">The CSV separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="comparison">The string comparison mode for header names.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCsvClauses.CsvHeaderLine"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Header).CsvHeaderLine(new[] { "Id", "Name", "Email" });</code></example>
    /// <seealso cref="MustCsvClauses.CsvHeaderLine"/>
    public static IRuleBuilderOptions<TModel, string?> CsvHeaderLine<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        IReadOnlyList<string>? expectedHeader,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CsvHeaderLine(val, expectedHeader, separator, comparison, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a well-formed CSV data row matching the specified column schema.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="schema">The column schema definitions for type-checking each field.</param>
    /// <param name="separator">The CSV separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCsvClauses.CsvRowLine(IMustClause, string, IReadOnlyList{CsvColumnSchema}, char, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.DataRow).CsvRowLine(schema);</code></example>
    /// <seealso cref="MustCsvClauses.CsvRowLine(IMustClause, string, IReadOnlyList{CsvColumnSchema}, char, string)"/>
    public static IRuleBuilderOptions<TModel, string?> CsvRowLine<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        IReadOnlyList<CsvColumnSchema>? schema,
        char separator = CsvRules.DefaultCsvSeparator,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CsvRowLine(val, schema, separator, paramName: null),
            message);

    /// <summary>
    /// Validates that the string value is a well-formed CSV data row matching the specified header and column types.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="header">The header column names.</param>
    /// <param name="types">A mapping of column names to their expected types.</param>
    /// <param name="separator">The CSV separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="headerNameComparison">The string comparison mode for header name matching.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCsvClauses.CsvRowLine(IMustClause, string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.DataRow).CsvRowLine(header, types);</code></example>
    /// <seealso cref="MustCsvClauses.CsvRowLine(IMustClause, string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison, string)"/>
    public static IRuleBuilderOptions<TModel, string?> CsvRowLine<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.CsvRowLine(val, header, types, separator, headerNameComparison, paramName: null),
            message);
}
