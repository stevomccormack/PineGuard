using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

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
    /// <param name="value">The string to validate as a CSV line.</param>
    /// <param name="separator">The field separator, excluding quotes and line breaks.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid CSV line, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// After validating configuration, returns a failed result if <paramref name="value"/> is <see langword="null"/>.
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
        string? value,
        char separator = CsvRules.DefaultCsvSeparator,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!CsvUtility.IsSeparatorSupported(separator))
            return MustResult<string>.Fail(MustCodes.Csv.Line.Invalid, "{paramName} must be a supported CSV separator.", nameof(separator), separator);

        if (value is null)
            return MustResult<string>.Fail(MustCodes.Csv.Line.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid CSV line.";

        var ok = CsvRules.IsCsvLine(value, separator);
        return MustResult<string>.FromBool(ok, MustCodes.Csv.Line.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV header line with the expected column names.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a CSV header line.</param>
    /// <param name="expectedHeader">The expected column names to match. Must be nonempty with nonblank names.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="comparison">The string comparison mode for column name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid CSV header with the expected columns, or <see langword="false"/> with
    /// a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// After validating configuration, returns a failed result if <paramref name="value"/> is <see langword="null"/>.
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
        string? value,
        IReadOnlyList<string>? expectedHeader,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!CsvUtility.IsSeparatorSupported(separator))
            return MustResult<string>.Fail(MustCodes.Csv.Header.Invalid, "{paramName} must be a supported CSV separator.", nameof(separator), separator);

        if (!CsvUtility.IsHeaderConfigurationValid(expectedHeader))
            return MustResult<string>.Fail(MustCodes.Csv.Header.Invalid, "{paramName} must contain nonblank CSV column names.", nameof(expectedHeader), expectedHeader);
        if (!EnumRules.IsDefined<StringComparison>(comparison))
            return MustResult<string>.Fail(MustCodes.Csv.Header.Invalid, "{paramName} must be a defined string comparison.", nameof(comparison), comparison);

        if (value is null)
            return MustResult<string>.Fail(MustCodes.Csv.Header.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid CSV header line.";

        var ok = CsvRules.IsCsvHeaderLine(value, expectedHeader, separator, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Csv.Header.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV row line conforming to the given column schema.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a CSV row line.</param>
    /// <param name="schema">The expected column schema. Must contain valid column definitions.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid CSV row conforming to <paramref name="schema"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// After validating configuration, returns a failed result if <paramref name="value"/> is <see langword="null"/>.
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
        string? value,
        IReadOnlyList<CsvColumnSchema>? schema,
        char separator = CsvRules.DefaultCsvSeparator,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!CsvUtility.IsSeparatorSupported(separator))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must be a supported CSV separator.", nameof(separator), separator);

        if (!CsvUtility.IsSchemaConfigurationValid(schema))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must contain valid CSV column definitions.", nameof(schema), schema);

        if (value is null)
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid CSV row line.";

        var ok = CsvRules.IsCsvRowLine(value, schema, separator);
        return MustResult<string>.FromBool(ok, MustCodes.Csv.Row.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is a valid CSV row line conforming to the given header names and column types.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a CSV row line.</param>
    /// <param name="header">The ordered list of column names. Must be nonempty with nonblank names.</param>
    /// <param name="types">A mapping from column name to expected <see cref="CsvColumnType"/>. Must be nonempty with defined types.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="CsvRules.DefaultCsvSeparator"/>.</param>
    /// <param name="headerNameComparison">The string comparison mode for matching column names. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid CSV row conforming to the header and types, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// After validating configuration, returns a failed result if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison)"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid CSV row line."</c>
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var result = Must.Be.CsvRowLine(dataLine, ["Id", "Name"], new Dictionary<string, CsvColumnType> { ["Id"] = CsvColumnType.Int32 });
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// ]]></code>
    /// </example>
    /// <seealso cref="CsvRules.IsCsvRowLine(string, IReadOnlyList{string}, IReadOnlyDictionary{string, CsvColumnType}, char, StringComparison)"/>
    /// <seealso href="https://pineguard.ai/docs/must/csv">CSV Must Clauses documentation</seealso>
    public static MustResult<string> CsvRowLine(this IMustClause _,
        string? value,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        char separator = CsvRules.DefaultCsvSeparator,
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!CsvUtility.IsSeparatorSupported(separator))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must be a supported CSV separator.", nameof(separator), separator);

        if (!CsvUtility.IsHeaderConfigurationValid(header))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must contain nonblank CSV column names.", nameof(header), header);
        if (!CsvUtility.IsTypeMapConfigurationValid(types))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must contain defined CSV column types.", nameof(types), types);
        if (!EnumRules.IsDefined<StringComparison>(headerNameComparison))
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, "{paramName} must be a defined string comparison.", nameof(headerNameComparison), headerNameComparison);

        if (value is null)
            return MustResult<string>.Fail(MustCodes.Csv.Row.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid CSV row line.";

        var ok = CsvRules.IsCsvRowLine(value, header, types, separator, headerNameComparison);
        return MustResult<string>.FromBool(ok, MustCodes.Csv.Row.Invalid, messageTemplate, paramName, value, value);
    }
}
