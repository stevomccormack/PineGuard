namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Utility methods for payment card operations including validation, masking, and card type detection.
/// Complies with ISO/IEC 7812 and PCI DSS standards.
/// </summary>
public static partial class PaymentCardUtility
{
    // PCI DSS masking constants
    private const int UnmaskedDigitsStart = 4;
    private const int UnmaskedDigitsEnd = 4;
    private const char MaskCharacter = '*';
    private const char SpaceSeparator = ' ';
    private const int FormatGroupSize = 4;

    /// <summary>
    /// Validates a payment card number using ISO/IEC 7812 standards (PAN length + Luhn check).
    /// </summary>
    /// <param name="cardNumber">The card number to validate</param>
    /// <returns>true if valid; otherwise, false</returns>
    /// <remarks>
    /// Validates both:
    /// 1. Primary Account Number (PAN) length (12-19 digits)
    /// 2. Luhn algorithm checksum
    /// Standard: https://www.iso.org/standard/70484.html
    /// </remarks>
    public static bool IsValidCardNumber(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        var sanitized = Sanitize(cardNumber);
        return PanAlgorithm.IsValid(sanitized) && LuhnAlgorithm.IsValid(sanitized);
    }

    /// <summary>
    /// Validates only the Luhn checksum of a card number.
    /// </summary>
    /// <param name="cardNumber">The card number to validate</param>
    /// <returns>true if Luhn valid; otherwise, false</returns>
    /// <remarks>
    /// The Luhn algorithm detects simple errors in payment card numbers.
    /// Algorithm: https://en.wikipedia.org/wiki/Luhn_algorithm
    /// </remarks>
    public static bool IsLuhnValid(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        var sanitized = Sanitize(cardNumber);
        return LuhnAlgorithm.IsValid(sanitized);
    }

    /// <summary>
    /// Masks a payment card number according to PCI DSS standards (shows first 4 and last 4 digits).
    /// </summary>
    /// <param name="cardNumber">The card number to mask</param>
    /// <param name="maskChar">Character to use for masking (default: '*')</param>
    /// <returns>Masked card number or empty string if invalid</returns>
    /// <remarks>
    /// PCI DSS requirement 3.3: Mask PAN when displayed (minimum first 6 and last 4 digits).
    /// Standard: https://www.pcisecuritystandards.org/
    /// </remarks>
    public static string Mask(string? cardNumber, char maskChar = MaskCharacter)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (sanitized.Length < UnmaskedDigitsStart + UnmaskedDigitsEnd)
            return string.Empty;

        var firstDigits = sanitized[..UnmaskedDigitsStart];
        var lastDigits = sanitized[^UnmaskedDigitsEnd..];
        var maskedMiddle = new string(maskChar, sanitized.Length - UnmaskedDigitsStart - UnmaskedDigitsEnd);

        return $"{firstDigits}{maskedMiddle}{lastDigits}";
    }

    /// <summary>
    /// Masks a payment card number showing only the last 4 digits (more secure).
    /// </summary>
    /// <param name="cardNumber">The card number to mask</param>
    /// <param name="maskChar">Character to use for masking (default: '*')</param>
    /// <returns>Masked card number showing only last 4 digits</returns>
    public static string SecureMask(string? cardNumber, char maskChar = MaskCharacter)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (sanitized.Length < UnmaskedDigitsEnd)
            return string.Empty;

        var lastDigits = sanitized[^UnmaskedDigitsEnd..];
        var maskedPart = new string(maskChar, sanitized.Length - UnmaskedDigitsEnd);

        return $"{maskedPart}{lastDigits}";
    }

    /// <summary>
    /// Checks if a card number appears to be masked.
    /// </summary>
    /// <param name="cardNumber">The card number to check</param>
    /// <returns>true if masked; otherwise, false</returns>
    public static bool IsMasked(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        return cardNumber.Contains(MaskCharacter);
    }

    /// <summary>
    /// Formats a card number with spaces for readability (e.g., "4532 1488 0343 6467").
    /// Uses default 4-4-4-4 formatting.
    /// </summary>
    /// <param name="cardNumber">The card number to format</param>
    /// <returns>Formatted card number with spaces</returns>
    public static string Format(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (string.IsNullOrEmpty(sanitized))
            return string.Empty;

        // Default: 4-4-4-4 format
        var formatted = string.Empty;
        for (var i = 0; i < sanitized.Length; i += FormatGroupSize)
        {
            if (i > 0)
                formatted += SpaceSeparator;

            var remaining = Math.Min(FormatGroupSize, sanitized.Length - i);
            formatted += sanitized.Substring(i, remaining);
        }

        return formatted;
    }

    /// <summary>
    /// Removes all non-digit characters from a card number.
    /// Efficiently handles common formats: spaces, dashes, and leading/trailing whitespace.
    /// </summary>
    /// <param name="cardNumber">The card number to sanitize (e.g., "4532-1488-0343-6467" or "4532 1488 0343 6467")</param>
    /// <returns>Card number with only digits</returns>
    /// <remarks>
    /// Common formats handled:
    /// - "4532 1488 0343 6467" (space-separated)
    /// - "4532-1488-0343-6467" (dash-separated)
    /// - " 4532148803436467 " (with whitespace)
    /// - "4532148803436467" (plain digits)
    /// </remarks>
    public static string Sanitize(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var trimmed = cardNumber.AsSpan().Trim();
        if (trimmed.IsEmpty)
            return string.Empty;

        // Fast path: if already all digits (no separators), return as-is
        var hasNonDigits = false;
        foreach (var c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                hasNonDigits = true;
                break;
            }
        }

        if (!hasNonDigits)
            return trimmed.ToString();

        // Slow path: filter out non-digits using Span for efficiency
        Span<char> buffer = stackalloc char[trimmed.Length];
        var position = 0;

        foreach (var c in trimmed)
        {
            if (char.IsDigit(c))
                buffer[position++] = c;
        }

        return position == 0 ? string.Empty : new string(buffer[..position]);
    }

    /// <summary>
    /// Truncates a card number to show only the last 4 digits (for logging/display).
    /// </summary>
    /// <param name="cardNumber">The card number to truncate</param>
    /// <returns>Last 4 digits prefixed with "****"</returns>
    public static string GetLast4(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (sanitized.Length < UnmaskedDigitsEnd)
            return string.Empty;

        return $"****{sanitized[^UnmaskedDigitsEnd..]}";
    }
}