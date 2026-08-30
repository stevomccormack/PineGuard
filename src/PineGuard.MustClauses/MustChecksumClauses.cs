using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate check digits,
/// delegating to <see cref="ChecksumRules"/> for core validation logic.
/// </summary>
/// <seealso cref="ChecksumRules"/>
/// <seealso href="https://pineguard.ai/docs/must/checksum">Checksum Must Clauses documentation</seealso>
public static class MustChecksumClauses
{
    /// <summary>
    /// Validates that the specified value must satisfy the Luhn checksum.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate. Spaces and hyphens are stripped before verification.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="ChecksumRules.IsLuhn"/>, which proves only that the digits are
    /// internally consistent — never that the sequence identifies a real account, device or person.
    /// The failure message follows the pattern <c>"{paramName} must satisfy the Luhn checksum."</c>
    /// </remarks>
    /// <seealso cref="ChecksumRules.IsLuhn"/>
    /// <seealso href="https://pineguard.ai/docs/must/checksum">Checksum Must Clauses documentation</seealso>
    public static MustResult<string> Luhn(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Checksum.Luhn.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must satisfy the Luhn checksum.";

        var ok = ChecksumRules.IsLuhn(value);
        return MustResult<string>.FromBool(ok, MustCodes.Checksum.Luhn.Invalid, messageTemplate, paramName, value, value);
    }
}
