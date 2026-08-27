using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for CSV line validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/csv">Guard CSV Clauses documentation</seealso>
public static class GuardCsvClauses
{
    /// <summary>
    /// Throws if <paramref name="line"/> is not a valid CSV line.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="line">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCsvClauses.CsvLine"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="line"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="line"/> is not a valid CSV line and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCsvClauses.CsvLine"/>:
    /// <c>Guard.Against.NotCsvLine</c> passes when the value is a valid CSV line.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotCsvLine(line);
    /// </code>
    /// </example>
    /// <seealso cref="MustCsvClauses.CsvLine"/>
    public static string NotCsvLine(this IGuardClause _,
        string? line,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        var result = Must.Be.CsvLine(line, paramName); // Guard.Against.NotCsvLine => Must.Be.CsvLine
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="line"/> is not a valid CSV header line matching <paramref name="expectedHeader"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="line">The CSV header line string to guard.</param>
    /// <param name="expectedHeader">The expected column headers, or <see langword="null"/> to skip header validation.</param>
    /// <param name="separator">The column separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="comparison">The string comparison for header name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCsvClauses.CsvHeaderLine"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="line"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="line"/> is not a valid CSV header line and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCsvClauses.CsvHeaderLine"/>:
    /// <c>Guard.Against.NotCsvHeaderLine</c> passes when the header line matches expectations.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotCsvHeaderLine(line, expectedHeader: ["Name", "Age"]);
    /// </code>
    /// </example>
    /// <seealso cref="MustCsvClauses.CsvHeaderLine"/>
    public static string NotCsvHeaderLine(this IGuardClause _,
        string? line,
        IReadOnlyList<string>? expectedHeader,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        var result = Must.Be.CsvHeaderLine(line, expectedHeader, separator, comparison, paramName); // Guard.Against.NotCsvHeaderLine => Must.Be.CsvHeaderLine
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="line"/> is not a valid CSV row line conforming to <paramref name="schema"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="line">The CSV data row string to guard.</param>
    /// <param name="schema">The expected column schema definitions, or <see langword="null"/> to skip schema validation.</param>
    /// <param name="separator">The column separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{CsvColumnSchema}?, char, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="line"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="line"/> is not a valid CSV row and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{CsvColumnSchema}?, char, string?)"/>:
    /// <c>Guard.Against.NotCsvRowLine</c> passes when the row matches the schema.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotCsvRowLine(line, schema: columnSchema);
    /// </code>
    /// </example>
    /// <seealso cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{CsvColumnSchema}?, char, string?)"/>
    public static string NotCsvRowLine(this IGuardClause _,
        string? line,
        IReadOnlyList<CsvColumnSchema>? schema,
        char separator = CsvRules.DefaultCsvSeparator,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        var result = Must.Be.CsvRowLine(line, schema, separator, paramName); // Guard.Against.NotCsvRowLine => Must.Be.CsvRowLine
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="line"/> is not a valid CSV row line matching the supplied <paramref name="header"/> and <paramref name="types"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="line">The CSV data row string to guard.</param>
    /// <param name="header">The header column names used for type lookup.</param>
    /// <param name="types">A dictionary mapping column names to their expected <see cref="CsvColumnType"/>.</param>
    /// <param name="separator">The column separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="headerNameComparison">The string comparison for header name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{string}?, IReadOnlyDictionary{string, CsvColumnType}?, char, StringComparison, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="line"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="line"/> is not a valid CSV row and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{string}?, IReadOnlyDictionary{string, CsvColumnType}?, char, StringComparison, string?)"/>:
    /// <c>Guard.Against.NotCsvRowLine</c> passes when the row conforms to the specified column types.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotCsvRowLine(line, header: ["Name", "Age"], types: columnTypes);
    /// </code>
    /// </example>
    /// <seealso cref="MustCsvClauses.CsvRowLine(IMustClause, string?, IReadOnlyList{string}?, IReadOnlyDictionary{string, CsvColumnType}?, char, StringComparison, string?)"/>
    public static string NotCsvRowLine(this IGuardClause _,
        string? line,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        var result = Must.Be.CsvRowLine(line, header, types, separator, headerNameComparison, paramName); // Guard.Against.NotCsvRowLine => Must.Be.CsvRowLine
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
