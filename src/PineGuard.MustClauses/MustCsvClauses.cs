using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate CSV line strings.
/// </summary>
/// <seealso cref="CsvRules"/>
/// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
public static class MustCsvClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string is a valid CSV line.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="line">The string to validate as a CSV line.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="line"/> is a valid CSV line, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="line"/> is <see langword="null"/>.
    /// Delegates to <see cref="CsvRules.IsCsvLine"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid CSV line."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CsvLine(inputLine);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CsvRules.IsCsvLine"/>
    /// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
    public static MustResult<string> CsvLine(this IMustClause _,
        string? line,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        if (line is null)
            return MustResult<string>.Fail(NullMessage, paramName, line);

        const string messageTemplate = "{paramName} must be a valid CSV line.";

        var ok = CsvRules.IsCsvLine(line);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, line, line);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV header line with the expected column names.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="line">The string to validate as a CSV header line.</param>
    /// <param name="expectedHeader">The expected column names to match. Pass <see langword="null"/> to skip column name validation.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="comparison">The string comparison mode for column name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="line"/> is a valid CSV header with the expected columns, or <see langword="false"/> with
    /// a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="line"/> is <see langword="null"/>.
    /// Delegates to <see cref="CsvRules.IsCsvHeaderLine"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid CSV header line."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CsvHeaderLine(firstLine, ["Id", "Name", "Email"]);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CsvRules.IsCsvHeaderLine"/>
    /// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
    public static MustResult<string> CsvHeaderLine(this IMustClause _,
        string? line,
        IReadOnlyList<string>? expectedHeader,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        if (line is null)
            return MustResult<string>.Fail(NullMessage, paramName, line);

        const string messageTemplate = "{paramName} must be a valid CSV header line.";

        var ok = CsvRules.IsCsvHeaderLine(line, expectedHeader, separator, comparison);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, line, line);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV row line conforming to the given column schema.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="line">The string to validate as a CSV row line.</param>
    /// <param name="schema">The expected column schema. Pass <see langword="null"/> to skip schema validation.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="line"/> is a valid CSV row conforming to <paramref name="schema"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="line"/> is <see langword="null"/>.
    /// Delegates to <see cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{CsvColumnSchema}, char)"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid CSV row line."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CsvRowLine(dataLine, columnSchema);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{CsvColumnSchema}, char)"/>
    /// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
    public static MustResult<string> CsvRowLine(this IMustClause _,
        string? line,
        IReadOnlyList<CsvColumnSchema>? schema,
        char separator = CsvRules.DefaultCsvSeparator,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        if (line is null)
            return MustResult<string>.Fail(NullMessage, paramName, line);

        const string messageTemplate = "{paramName} must be a valid CSV row line.";

        var ok = CsvRules.IsCsvRowLine(line, schema, separator);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, line, line);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV row line conforming to the given header names and column types.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="line">The string to validate as a CSV row line.</param>
    /// <param name="header">The ordered list of column names. Pass <see langword="null"/> to skip header matching.</param>
    /// <param name="types">A mapping from column name to expected <see cref="CsvColumnType"/>. Pass <see langword="null"/> to skip type validation.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="headerNameComparison">The string comparison mode for matching column names. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="line"/> is a valid CSV row conforming to the header and types, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="line"/> is <see langword="null"/>.
    /// Delegates to <see cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison)"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid CSV row line."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.CsvRowLine(dataLine, ["Id", "Name"], new Dictionary<string, CsvColumnType> { ["Id"] = CsvColumnType.Integer });
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison)"/>
    /// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
    public static MustResult<string> CsvRowLine(this IMustClause _,
        string? line,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(line))] string? paramName = null)
    {
        if (line is null)
            return MustResult<string>.Fail(NullMessage, paramName, line);

        const string messageTemplate = "{paramName} must be a valid CSV row line.";

        var ok = CsvRules.IsCsvRowLine(line, header, types, separator, headerNameComparison);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, line, line);
    }
}
