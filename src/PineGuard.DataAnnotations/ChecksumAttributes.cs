using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field satisfies the Luhn checksum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustChecksumClauses.Luhn"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Spaces and hyphens are stripped before verification, so a card number written in groups validates the
/// same as its unseparated form. A passing value proves only that the digits are internally consistent —
/// never that the sequence identifies a real account, device or person.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PaymentModel
/// {
///     [Luhn]
///     public string CardNumber { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustChecksumClauses.Luhn"/>
/// <seealso href="https://pineguard.ai/docs/annotations/checksum">Checksum Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LuhnAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Checksum.Luhn.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Luhn(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
