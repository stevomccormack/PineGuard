using PineGuard.Externals.Iso.Payments;
using PineGuard.Externals.Iso.Payments.Cards;
using PineGuard.Rules.Iso;
using System.Text;

namespace PineGuard.Utils.Iso;

public static class IsoPaymentCardUtility
{
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
        if (!StringUtility.TryParseDigits(cardNumber, out var digitsOnly, allowedSeparators ?? IsoPaymentCardBrand.DefaultAllowedSeparators))
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

    public static string Format(string? cardNumber, char separator = IsoPaymentCardBrand.SpaceSeparator)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return string.Empty;

        var sanitized = Sanitize(cardNumber);
        if (string.IsNullOrEmpty(sanitized))
            return string.Empty;

        var groupCount = (sanitized.Length + FormatGroupSize - 1) / FormatGroupSize;
        var builder = new StringBuilder(sanitized.Length + Math.Max(0, groupCount - 1));

        for (var i = 0; i < sanitized.Length; i += FormatGroupSize)
        {
            if (i > 0)
                builder.Append(separator);

            var remaining = Math.Min(FormatGroupSize, sanitized.Length - i);
            builder.Append(sanitized, i, remaining);
        }

        return builder.ToString();
    }

    public static string Sanitize(string? cardNumber)
    {
        return StringUtility.TryParseDigits(cardNumber, out var digitsOnly, IsoPaymentCardBrand.DefaultAllowedSeparators)
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
