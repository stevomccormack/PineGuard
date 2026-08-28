using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid CSV line
/// (comma-separated values on a single line).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCsvClauses.CsvLine"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ImportModel
/// {
///     [CsvLine]
///     public string Row { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CsvHeaderLineAttribute"/>
/// <seealso cref="MustCsvClauses.CsvLine"/>
/// <seealso href="https://pineguard.ai/docs/annotations/csv">CSV Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CsvLineAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Csv.Line.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CsvLine(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

// CsvHeaderLine/CsvRowLine may require parameters like validHeaders (list of strings).
// Attribute arguments must be constants or arrays of constants.
// [CsvHeaderLine("Id", "Name", "Date")]
/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a CSV header line containing
/// exactly the specified column names (case-insensitive, comma-separated).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCsvClauses.CsvHeaderLine"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The comma (<c>,</c>) is used as the separator. Column name comparison is ordinal case-insensitive.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ImportModel
/// {
///     [CsvHeaderLine("Id", "Name", "Date")]
///     public string Header { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CsvLineAttribute"/>
/// <seealso cref="MustCsvClauses.CsvHeaderLine"/>
/// <seealso href="https://pineguard.ai/docs/annotations/csv">CSV Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CsvHeaderLineAttribute(params string[] expectedHeaders) : ValidationAttributeBase(typeof(string), MustCodes.Csv.Header.Invalid)
{
    /// <summary>Gets the expected header column names that the CSV header line must contain.</summary>
    public string[] ExpectedHeaders { get; } = expectedHeaders;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CsvHeaderLine(strValue, ExpectedHeaders, separator: ',', comparison: StringComparison.OrdinalIgnoreCase, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
