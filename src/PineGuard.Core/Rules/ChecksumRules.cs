using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure checksum validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/checksum">Checksum Rules documentation</seealso>
public static class ChecksumRules
{
    /// <summary>
    /// The fewest digits a Luhn-verifiable sequence can carry (a payload digit and its check digit).
    /// </summary>
    public const int MinLuhnLength = 2;

    /// <summary>
    /// Determines whether the specified value satisfies the Luhn (mod 10) checksum.
    /// </summary>
    /// <param name="value">
    /// The value to validate. Spaces and hyphens are stripped before verification; any other non-digit
    /// character, or fewer than <see cref="MinLuhnLength"/> digits, returns <see langword="false"/>.
    /// If <see langword="null"/> or whitespace, returns <see langword="false"/>.
    /// </param>
    /// <returns><see langword="true"/> if the digits satisfy the Luhn checksum; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This is the check-digit algorithm behind payment card numbers, IMEIs and several national
    /// identifiers. It proves only that the digits are internally consistent — never that the sequence
    /// identifies a real account, device or person.
    /// </remarks>
    public static bool IsLuhn(string? value) =>
        StringUtility.TryParseDigits(value, out var digits)
        && digits.Length >= MinLuhnLength
        && ChecksumUtility.IsLuhn(digits.AsSpan());
}
