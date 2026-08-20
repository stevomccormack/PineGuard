using System.Globalization;
using System.Text;
using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides CSV parsing and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/csv">CSV Utility documentation</seealso>
public static class CsvUtility
{
    /// <summary>
    /// Attempts to parse a single line of RFC 4180-style CSV text into its constituent fields.
    /// </summary>
    /// <param name="value">
    /// The CSV line to parse. If <see langword="null"/> or empty/whitespace, returns <see langword="false"/>.
    /// </param>
    /// <param name="fields">
    /// When this method returns <see langword="true"/>, contains the parsed fields in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was successfully parsed as a well-formed CSV line
    /// (no embedded line breaks and no unterminated quoted field); otherwise, <see langword="false"/>.
    /// Whitespace immediately before an opening quote and whitespace immediately after a closing quote
    /// are both permitted and discarded (e.g., <c>a, "b"</c> and <c>"a" ,b</c> parse identically); any
    /// other non-whitespace content adjacent to a quote makes the line invalid.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="separator"/> is <c>"</c>, <c>\r</c>, or <c>\n</c> — none of which can
    /// ever function as a separator, since the quote and line-break checks always take precedence.
    /// </exception>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvLine("a,b,\"c,d\"", out var fields); // true, fields = ["a", "b", "c,d"]
    /// </code>
    /// </example>
    public static bool TryParseCsvLine(string? value, out IReadOnlyList<string>? fields, char separator = ',')
    {
        fields = null;

        if (separator is '"' or '\r' or '\n')
            throw new ArgumentException("The separator cannot be a double quote or a line-break character.", nameof(separator));

        if (value is null)
            return false;

        if (!StringUtility.TryGetTrimmed(value, out _))
            return false;

        var list = new List<string>();
        var builder = new StringBuilder();
        var inQuotes = false;
        var afterClosingQuote = false;

        for (var i = 0; i < value.Length; i++)
        {
            var ch = value[i];

            if (IsLineBreak(ch))
                return false;

            if (afterClosingQuote)
            {
                if (!HandleAfterClosingQuote(ch, separator, builder, list, ref afterClosingQuote))
                    return false;
                continue;
            }

            if (inQuotes)
            {
                HandleInQuotes(value, ref i, ch, builder, ref inQuotes, ref afterClosingQuote);
                continue;
            }

            if (!HandleUnquoted(ch, separator, builder, list, ref inQuotes))
                return false;
        }

        if (inQuotes)
            return false;

        list.Add(builder.ToString());

        fields = list;
        return true;
    }

    private static bool IsLineBreak(char ch) => ch is '\r' or '\n';

    private static bool HandleAfterClosingQuote(
        char ch, char separator, StringBuilder builder, List<string> fields, ref bool afterClosingQuote)
    {
        if (ch != separator)
            return char.IsWhiteSpace(ch);

        fields.Add(builder.ToString());
        builder.Clear();
        afterClosingQuote = false;
        return true;
    }

    private static void HandleInQuotes(
        string value, ref int i, char ch, StringBuilder builder, ref bool inQuotes, ref bool afterClosingQuote)
    {
        if (ch == '"')
        {
            if (i + 1 < value.Length && value[i + 1] == '"')
            {
                builder.Append('"');
                i++;
                return;
            }

            inQuotes = false;
            afterClosingQuote = true;
            return;
        }

        builder.Append(ch);
    }

    private static bool HandleUnquoted(
        char ch, char separator, StringBuilder builder, List<string> fields, ref bool inQuotes)
    {
        if (ch == '"')
        {
            if (!IsWhiteSpaceOnly(builder))
                return false;

            builder.Clear();
            inQuotes = true;
            return true;
        }

        if (ch == separator)
        {
            fields.Add(builder.ToString());
            builder.Clear();
            return true;
        }

        builder.Append(ch);
        return true;
    }

    private static bool IsWhiteSpaceOnly(StringBuilder builder)
    {
        for (var i = 0; i < builder.Length; i++)
        {
            if (!char.IsWhiteSpace(builder[i]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Attempts to parse a single line of CSV text into a header row of trimmed, non-empty column names.
    /// </summary>
    /// <param name="value">
    /// The CSV header line to parse. If not a well-formed CSV line (see <see cref="TryParseCsvLine"/>),
    /// returns <see langword="false"/>.
    /// </param>
    /// <param name="header">
    /// When this method returns <see langword="true"/>, contains the parsed and trimmed header names in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was successfully parsed and every field trims to a
    /// non-empty header name; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvHeaderLine("Name, Age ,Email", out var header); // true, header = ["Name", "Age", "Email"]
    /// </code>
    /// </example>
    public static bool TryParseCsvHeaderLine(string? value, out IReadOnlyList<string>? header, char separator = ',')
    {
        header = null;

        if (!TryParseCsvLine(value, out var fields, separator) || fields is null)
            return false;

        var list = new List<string>(capacity: fields.Count);

        foreach (var field in fields)
        {
            if (!StringUtility.TryGetTrimmed(field, out var trimmed))
                return false;

            list.Add(trimmed);
        }

        header = list;
        return true;
    }

    /// <summary>
    /// Attempts to parse a CSV header line and verify that it matches an expected set of column names.
    /// </summary>
    /// <param name="value">
    /// The CSV header line to parse. If not a well-formed CSV line, returns <see langword="false"/>.
    /// </param>
    /// <param name="expectedHeader">
    /// The expected header names, in order. If <see langword="null"/> or empty, returns <see langword="false"/>.
    /// </param>
    /// <param name="header">
    /// When this method returns <see langword="true"/>, contains the parsed header names in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <param name="comparison">
    /// The <see cref="StringComparison"/> used to compare parsed header names against
    /// <paramref name="expectedHeader"/>. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> parses to the same number of fields as
    /// <paramref name="expectedHeader"/> and each parsed field matches the corresponding expected name
    /// under <paramref name="comparison"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvHeaderLine("name,age", ["Name", "Age"], out var header); // true
    /// </code>
    /// </example>
    public static bool TryParseCsvHeaderLine(
        string? value,
        IReadOnlyList<string>? expectedHeader,
        out IReadOnlyList<string>? header,
        char separator = ',',
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        header = null;

        if (expectedHeader is null || expectedHeader.Count == 0)
            return false;

        if (!TryParseCsvHeaderLine(value, out var parsedHeader, separator) || parsedHeader is null)
            return false;

        if (parsedHeader.Count != expectedHeader.Count)
            return false;

        for (var i = 0; i < parsedHeader.Count; i++)
        {
            if (!StringUtility.TryGetTrimmed(expectedHeader[i], out var expected))
                return false;

            if (!string.Equals(parsedHeader[i], expected, comparison))
                return false;
        }

        header = parsedHeader;
        return true;
    }

    /// <summary>
    /// Attempts to parse a CSV row line and verify that it contains an exact number of fields.
    /// </summary>
    /// <param name="value">
    /// The CSV row line to parse. If not a well-formed CSV line, returns <see langword="false"/>.
    /// </param>
    /// <param name="expectedFieldCount">
    /// The exact number of fields required. Must be positive; if zero or negative, returns <see langword="false"/>.
    /// </param>
    /// <param name="fields">
    /// When this method returns <see langword="true"/>, contains the parsed fields in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was successfully parsed into exactly
    /// <paramref name="expectedFieldCount"/> fields; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvRowLine("a,b,c", 3, out var fields); // true
    /// </code>
    /// </example>
    public static bool TryParseCsvRowLine(
        string? value,
        int expectedFieldCount,
        out IReadOnlyList<string>? fields,
        char separator = ',')
    {
        fields = null;

        if (expectedFieldCount <= 0)
            return false;

        if (!TryParseCsvLine(value, out var parsed, separator) || parsed is null)
            return false;

        if (parsed.Count != expectedFieldCount)
            return false;

        fields = parsed;
        return true;
    }

    /// <summary>
    /// Attempts to parse a CSV row line and validate each field against a column schema.
    /// </summary>
    /// <param name="value">
    /// The CSV row line to parse. If not a well-formed CSV line, returns <see langword="false"/>.
    /// </param>
    /// <param name="schema">
    /// The per-column schema describing the name, type, and constraints expected for each field, in order.
    /// If <see langword="null"/> or empty, returns <see langword="false"/>.
    /// </param>
    /// <param name="fields">
    /// When this method returns <see langword="true"/>, contains the parsed fields in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> parses to <c>schema.Count</c> fields and every
    /// field satisfies its corresponding <see cref="CsvColumnSchema"/> (required, max length, and value
    /// type); otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvRowLine("1,John", schema, out var fields);
    /// </code>
    /// </example>
    public static bool TryParseCsvRowLine(
        string? value,
        IReadOnlyList<CsvColumnSchema>? schema,
        out IReadOnlyList<string>? fields,
        char separator = ',')
    {
        fields = null;

        if (schema is null || schema.Count == 0)
            return false;

        if (!TryParseCsvRowLine(value, schema.Count, out var parsedFields, separator) || parsedFields is null)
            return false;

        for (var i = 0; i < parsedFields.Count; i++)
        {
            if (!ValidateColumnField(parsedFields[i], schema[i]))
                return false;
        }

        fields = parsedFields;
        return true;
    }

    /// <summary>
    /// Attempts to parse a CSV row line, building a column schema from a header and a map of column types.
    /// </summary>
    /// <param name="value">
    /// The CSV row line to parse. If not a well-formed CSV line, returns <see langword="false"/>.
    /// </param>
    /// <param name="header">
    /// The column names for the row, in order. If <see langword="null"/> or empty, returns <see langword="false"/>.
    /// </param>
    /// <param name="types">
    /// A map of column name to <see cref="CsvColumnType"/>. Columns whose name is not found in the map
    /// default to <see cref="CsvColumnType.String"/>. If <see langword="null"/> or empty, returns
    /// <see langword="false"/>.
    /// </param>
    /// <param name="fields">
    /// When this method returns <see langword="true"/>, contains the parsed fields in order.
    /// When <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <param name="separator">The field separator character. Defaults to <c>,</c>.</param>
    /// <param name="headerNameComparison">
    /// The <see cref="StringComparison"/> used to match <paramref name="header"/> names against the (trimmed)
    /// keys of <paramref name="types"/>. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>. Each
    /// header name is compared in turn against every trimmed key in <paramref name="types"/> using
    /// <paramref name="headerNameComparison"/>, regardless of the comparer <paramref name="types"/> itself was
    /// constructed with.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a column schema could be built from <paramref name="header"/> and
    /// <paramref name="types"/> and <paramref name="value"/> satisfies that schema; otherwise,
    /// <see langword="false"/>. Every column built from <paramref name="header"/> and <paramref name="types"/>
    /// is marked required (<see cref="CsvColumnSchema.IsRequired"/> is always <see langword="true"/>) and uses
    /// <see cref="CsvColumnSchema.DefaultMaxLength"/>; use the schema-based overload directly for control over
    /// either setting.
    /// </returns>
    /// <example>
    /// <code>
    /// CsvUtility.TryParseCsvRowLine("1,John", ["Id", "Name"], types, out var fields);
    /// </code>
    /// </example>
    public static bool TryParseCsvRowLine(
        string? value,
        IReadOnlyList<string>? header,
        IReadOnlyDictionary<string, CsvColumnType>? types,
        out IReadOnlyList<string>? fields,
        char separator = ',',
        StringComparison headerNameComparison = StringComparison.OrdinalIgnoreCase)
    {
        fields = null;

        if (header is null || header.Count == 0)
            return false;

        if (types is null || types.Count == 0)
            return false;

        if (!TryGetSchema(header, types, headerNameComparison, out var schema) || schema is null)
            return false;

        return TryParseCsvRowLine(value, schema, out fields, separator);
    }

    private static bool TryGetSchema(
        IReadOnlyList<string> header,
        IReadOnlyDictionary<string, CsvColumnType> types,
        StringComparison headerNameComparison,
        out IReadOnlyList<CsvColumnSchema>? schema)
    {
        schema = null;

        var result = new List<CsvColumnSchema>(capacity: header.Count);

        foreach (var headerName in header)
        {
            if (!StringUtility.TryGetTrimmed(headerName, out var normalizedHeaderName)) return false;

            if (!TryGetTypeByName(types, normalizedHeaderName, headerNameComparison, out var match))
                match = CsvColumnType.String;

            result.Add(new CsvColumnSchema(normalizedHeaderName, match, IsRequired: true));
        }

        schema = result;
        return true;
    }

    private static bool TryGetTypeByName(
        IReadOnlyDictionary<string, CsvColumnType> types,
        string headerName,
        StringComparison comparison,
        out CsvColumnType type)
    {
        type = default;

        foreach (var kvp in types)
        {
            if (!StringUtility.TryGetTrimmed(kvp.Key, out var candidateName))
                continue;

            if (!string.Equals(candidateName, headerName, comparison))
                continue;

            type = kvp.Value;
            return true;
        }

        return false;
    }

    private static bool ValidateColumnField(string rawField, CsvColumnSchema column)
    {
        if (!StringUtility.TryGetTrimmed(column.Name, out _))
            return false;

        if (column.MaxLength <= 0)
            return false;

        if (rawField.Length > column.MaxLength)
            return false;

        if (!StringUtility.TryGetTrimmed(rawField, out var trimmed))
            return !column.IsRequired;

        return IsCsvFieldValueType(trimmed, column.Type);
    }

    private static bool IsCsvFieldValueType(string trimmedValue, CsvColumnType type)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (type)
        {
            case CsvColumnType.String: return true;

            case CsvColumnType.Int32: return StringUtility.NumberTypes.TryParseInt32(trimmedValue, out _, provider: CultureInfo.InvariantCulture);
            case CsvColumnType.Int64: return StringUtility.NumberTypes.TryParseInt64(trimmedValue, out _, provider: CultureInfo.InvariantCulture);
            case CsvColumnType.Decimal: return StringUtility.NumberTypes.TryParseDecimal(trimmedValue, out _, provider: CultureInfo.InvariantCulture);
            case CsvColumnType.Single: return StringUtility.NumberTypes.TryParseSingle(trimmedValue, out _, provider: CultureInfo.InvariantCulture);
            case CsvColumnType.Double: return StringUtility.NumberTypes.TryParseDouble(trimmedValue, out _, provider: CultureInfo.InvariantCulture);

            case CsvColumnType.Guid: return StringUtility.Guid.TryParse(trimmedValue, out Guid _);
            case CsvColumnType.Bool: return StringUtility.Bool.TryParse(trimmedValue, out _);

#if NET8_0_OR_GREATER
            case CsvColumnType.DateOnly: return StringUtility.DateOnly.TryParse(trimmedValue, out _);
            case CsvColumnType.TimeOnly: return StringUtility.TimeOnly.TryParse(trimmedValue, out _);
#else
            case CsvColumnType.DateOnly: return DateTime.TryParseExact(trimmedValue, DateOnlyFallbackFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            case CsvColumnType.TimeOnly: return DateTime.TryParseExact(trimmedValue, TimeOnlyFallbackFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
#endif
            case CsvColumnType.DateTimeOffset: return StringUtility.DateTimeOffset.TryParse(trimmedValue, out _);

            default: return false;
        }
    }

#if !NET8_0_OR_GREATER
    /// <summary>
    /// Invariant-culture, exact-match date formats used to validate <see cref="CsvColumnType.DateOnly"/>
    /// fields on targets that predate <c>System.DateOnly</c> (netstandard2.1).
    /// </summary>
    private static readonly string[] DateOnlyFallbackFormats = ["yyyy-MM-dd"];

    /// <summary>
    /// Invariant-culture, exact-match time formats used to validate <see cref="CsvColumnType.TimeOnly"/>
    /// fields on targets that predate <c>System.TimeOnly</c> (netstandard2.1).
    /// </summary>
    private static readonly string[] TimeOnlyFallbackFormats = ["HH:mm:ss", "HH:mm:ss.fff", "HH:mm"];
#endif
}
