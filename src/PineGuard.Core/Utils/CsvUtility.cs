using System.Text;
using PineGuard.Common;

namespace PineGuard.Utils;

/// <summary>
/// Provides CSV parsing and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/csv">CSV Utility documentation</seealso>
public static class CsvUtility
{
    public static bool TryParseCsvLine(string? value, out IReadOnlyList<string>? fields, char separator = ',')
    {
        fields = null;

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
            if (builder.Length != 0)
                return false;

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

        if (parsedFields.Where((field, i) => !ValidateColumnField(field, schema[i])).Any())
            return false;

        fields = parsedFields;
        return true;
    }

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

        var allowExactKeyLookup = headerNameComparison == StringComparison.Ordinal;

        var result = new List<CsvColumnSchema>(capacity: header.Count);

        foreach (var headerName in header)
        {
            if (!StringUtility.TryGetTrimmed(headerName, out var normalizedHeaderName)) return false;

            if (allowExactKeyLookup)
            {
                var type = types.GetValueOrDefault(normalizedHeaderName, CsvColumnType.String);

                result.Add(new CsvColumnSchema(normalizedHeaderName, type, IsRequired: true));
                continue;
            }

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

        if (!StringUtility.TryGetTrimmed(rawField, out var trimmed))
            return !column.IsRequired;

        return trimmed.Length <= column.MaxLength && IsCsvFieldValueType(trimmed, column.Type);
    }

    private static bool IsCsvFieldValueType(string trimmedValue, CsvColumnType type)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (type)
        {
            case CsvColumnType.String: return true;

            case CsvColumnType.Int32: return StringUtility.NumberTypes.TryParseInt32(trimmedValue, out _);
            case CsvColumnType.Int64: return StringUtility.NumberTypes.TryParseInt64(trimmedValue, out _);
            case CsvColumnType.Decimal: return StringUtility.NumberTypes.TryParseDecimal(trimmedValue, out _);
            case CsvColumnType.Single: return StringUtility.NumberTypes.TryParseSingle(trimmedValue, out _);
            case CsvColumnType.Double: return StringUtility.NumberTypes.TryParseDouble(trimmedValue, out _);

            case CsvColumnType.Guid: return StringUtility.Guid.TryParse(trimmedValue, out Guid _);
            case CsvColumnType.Bool: return StringUtility.Bool.TryParse(trimmedValue, out _);

#if NET8_0_OR_GREATER
            case CsvColumnType.DateOnly: return StringUtility.DateOnly.TryParse(trimmedValue, out _);
            case CsvColumnType.TimeOnly: return StringUtility.TimeOnly.TryParse(trimmedValue, out _);
#endif
            case CsvColumnType.DateTimeOffset: return StringUtility.DateTimeOffset.TryParse(trimmedValue, out _);

            default: return false;
        }
    }
}
