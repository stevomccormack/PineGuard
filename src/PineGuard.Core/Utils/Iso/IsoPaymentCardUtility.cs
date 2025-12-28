using PineGuard.Iso.Payments;
using PineGuard.Rules;
using PineGuard.Rules.Iso;

namespace PineGuard.Utils.Iso;

public static class IsoPaymentCardUtility
{
    public static readonly char[] DefaultAllowedSeparators = IsoPaymentCardRules.DefaultAllowedSeparators;

    public const char SpaceSeparator = IsoPaymentCardRules.SpaceSeparator;
    public const char DashSeparator = IsoPaymentCardRules.DashSeparator;

    private const int UnmaskedDigitsStart = 4;
    private const int UnmaskedDigitsEnd = 4;
    private const char MaskCharacter = '*';
    private const int FormatGroupSize = 4;

    public static bool IsValidCardNumber(string? cardNumber, char[]? allowedSeparators = null)
    {
        return allowedSeparators is null
            ? IsoPaymentCardRules.IsIsoPaymentCard(cardNumber)
            : IsoPaymentCardRules.IsIsoPaymentCard(cardNumber, allowedSeparators);
    }

    public static bool IsLuhnValid(string? cardNumber, char[]? allowedSeparators = null)
    {
        if (!StringNumberRules.TrySanitizeDigits(cardNumber, out var digitsOnly, allowedSeparators ?? DefaultAllowedSeparators))
            return false;

        return LuhnAlgorithm.IsValid(digitsOnly);
    }

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

    public static bool IsMasked(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        return cardNumber.Contains(MaskCharacter);
    }

    public static string Format(string? cardNumber, char separator = SpaceSeparator)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (string.IsNullOrEmpty(sanitized))
            return string.Empty;

        var formatted = string.Empty;
        for (var i = 0; i < sanitized.Length; i += FormatGroupSize)
        {
            if (i > 0)
                formatted += separator;

            var remaining = Math.Min(FormatGroupSize, sanitized.Length - i);
            formatted += sanitized.Substring(i, remaining);
        }

        return formatted;
    }

    public static string Sanitize(string? cardNumber)
    {
        return StringNumberRules.TrySanitizeDigits(cardNumber, out var digitsOnly, DefaultAllowedSeparators)
            ? digitsOnly
            : string.Empty;
    }

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
