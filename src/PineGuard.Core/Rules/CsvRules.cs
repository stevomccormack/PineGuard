using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure CSV content validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/csv">CSV Rules documentation</seealso>
public static class CsvRules
{
    /// <summary>
    /// The default CSV field separator character (comma).
    /// </summary>
    public const char DefaultCsvSeparator = ',';

    /// <summary>
    /// Determines whether the specified string is a valid CSV line (one or more delimited fields).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is parseable as a CSV line; otherwise, <see langword="false"/>.</returns>
    /// <param name="separator">The field separator, excluding quotes and line breaks.</param>
    /// <exception cref="ArgumentException">The separator is a quote or line break.</exception>
    public static bool IsCsvLine(string? value, char separator = DefaultCsvSeparator) =>
        CsvUtility.TryParseCsvLine(value, out _, separator);

    /// <summary>
    /// Determines whether the specified string is a valid CSV header line matching the expected column names.
    /// </summary>
    /// <param name="value">The header line to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="expectedHeader">The expected column names in order. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="DefaultCsvSeparator"/>.</param>
    /// <param name="comparison">The string comparison for column name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns><see langword="true"/> if the header line matches the expected columns; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="separator"/> is <c>"</c>, <c>\r</c>, or <c>\n</c> — none of which can
    /// ever function as a separator, since the quote and value-break checks always take precedence.
    /// </exception>
    public static bool IsCsvHeaderLine(
        string? value,
        IReadOnlyList<string>? expectedHeader,
        char separator = DefaultCsvSeparator,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        => CsvUtility.TryParseCsvHeaderLine(value, expectedHeader, out _, separator, comparison);

    /// <summary>
    /// Determines whether the specified string is a valid CSV data row matching the given column schema.
    /// </summary>
    /// <param name="value">The data row to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="schema">The column schema defining expected column types. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="DefaultCsvSeparator"/>.</param>
    /// <returns><see langword="true"/> if the row conforms to the schema; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="separator"/> is <c>"</c>, <c>\r</c>, or <c>\n</c> — none of which can
    /// ever function as a separator, since the quote and value-break checks always take precedence.
    /// </exception>
    public static bool IsCsvRowLine(
        string? value,
        IReadOnlyList<CsvColumnSchema>? schema,
        char separator = DefaultCsvSeparator)
        => CsvUtility.TryParseCsvRowLine(value, schema, out _, separator);

    /// <summary>
    /// Determines whether the specified string is a valid CSV data row matching the given header and column type definitions.
    /// </summary>
    /// <param name="value">The data row to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="header">The column header names. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="types">A mapping of column names to expected types. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="separator">The field separator character. Defaults to <see cref="DefaultCsvSeparator"/>.</param>
    /// <param name="headerNameComparison">The string comparison for header name matching. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns><see langword="true"/> if the row conforms to the header and type definitions; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="separator"/> is <c>"</c>, <c>\r</c>, or <c>\n</c> — none of which can
    /// ever function as a separator, since the quote and value-break checks always take precedence.
    /// </exception>
    public static bool IsCsvRowLine(
        string? value,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        char separator = DefaultCsvSeparator,
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase)
        => CsvUtility.TryParseCsvRowLine(value, header, types, out _, separator, headerNameComparison);
}
