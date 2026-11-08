using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid CSV line
/// (delimited fields on a single line).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCsvClauses.CsvLine"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Configuration is checked first; a <see langword="null"/> property value then skips value validation.
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
/// <param name="separator">The CSV field separator, excluding quotes and line breaks.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CsvLineAttribute(char separator = CsvRules.DefaultCsvSeparator) : ValidationAttributeBase(typeof(string), MustCodes.Csv.Line.Invalid)
{
    /// <summary>Gets the configured field separator.</summary>
    public char Separator { get; } = separator;

    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var configuration = Must.Be.CsvLine(null, Separator, paramName: null);
        return configuration.ParamName is not null
            ? FromMustResult(configuration, validationContext)
            : base.IsValid(value, validationContext);
    }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CsvLine(strValue, Separator, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

// CsvHeaderLine/CsvRowLine may require parameters like validHeaders (list of strings).
// Attribute arguments must be constants or arrays of constants.
// [CsvHeaderLine("Id", "Name", "Date")]
/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a CSV header line containing
/// exactly the specified column names.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCsvClauses.CsvHeaderLine"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The separator defaults to comma and comparison to ordinal case-insensitive; both are configurable.
/// Configuration is checked first; a <see langword="null"/> property value then skips value validation.
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
    /// <summary>Gets or sets the CSV field separator.</summary>
    public char Separator { get; set; } = CsvRules.DefaultCsvSeparator;

    /// <summary>Gets or sets the comparison used for header names.</summary>
    public StringComparison Comparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>Gets the expected header column names that the CSV header line must contain.</summary>
    public string[] ExpectedHeaders { get; } = expectedHeaders;

    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var configuration = Must.Be.CsvHeaderLine(null, ExpectedHeaders, Separator, Comparison, paramName: null);
        return configuration.ParamName is not null
            ? FromMustResult(configuration, validationContext)
            : base.IsValid(value, validationContext);
    }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CsvHeaderLine(strValue, ExpectedHeaders, separator: Separator, comparison: Comparison, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
