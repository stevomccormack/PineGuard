namespace PineGuard.Utils;

/// <summary>
/// Provides checksum verification utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/checksum">Checksum Utility documentation</seealso>
public static class ChecksumUtility
{
    /// <summary>
    /// Determines whether the specified digit sequence satisfies the Luhn (mod 10) checksum.
    /// </summary>
    /// <param name="digits">
    /// The digit characters to verify, with any separators already stripped. An empty span, or a span
    /// containing any character outside <c>0</c>–<c>9</c>, returns <see langword="false"/>.
    /// </param>
    /// <returns><see langword="true"/> if the sequence satisfies the Luhn checksum; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Reading right to left, every second digit is doubled and reduced by nine when the result exceeds
    /// nine; the sequence is valid when the resulting sum is a multiple of ten. The check verifies the
    /// trailing check digit only — it says nothing about what the sequence identifies.
    /// </remarks>
    public static bool IsLuhn(ReadOnlySpan<char> digits)
    {
        if (digits.IsEmpty)
            return false;

        var sum = 0;
        var doubled = false;

        for (var index = digits.Length - 1; index >= 0; index--)
        {
            var character = digits[index];

            if (character is < '0' or > '9')
                return false;

            var digit = character - '0';

            if (doubled)
            {
                digit *= 2;

                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            doubled = !doubled;
        }

        return sum % 10 == 0;
    }
}
