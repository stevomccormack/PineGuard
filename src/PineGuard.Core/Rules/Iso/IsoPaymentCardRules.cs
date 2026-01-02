using PineGuard.Common;
using PineGuard.Iso.Payments;
using PineGuard.Iso.Payments.Cards;
using PineGuard.Utils;

namespace PineGuard.Rules.Iso;

public static class IsoPaymentCardRules
{
    public const char SpaceSeparator = ' ';
    public const char DashSeparator = '-';

    public static readonly char[] DefaultAllowedSeparators = [SpaceSeparator, DashSeparator];

    public static bool IsIsoPaymentCard(string? value) =>
        IsIsoPaymentCard(value, DefaultAllowedSeparators);

    public static bool IsIsoPaymentCard(string? value, char[] allowedSeparators)
    {
        ArgumentNullException.ThrowIfNull(allowedSeparators);

        if (!StringUtility.TryParseDigits(value, out var digitsOnly, allowedSeparators))
            return false;

        if (!RuleComparison.IsBetween(digitsOnly.Length, IsoPaymentCardBrand.MinPanLength, IsoPaymentCardBrand.MaxPanLength, RangeInclusion.Inclusive))
            return false;

        return PanAlgorithm.IsValid(digitsOnly) && LuhnAlgorithm.IsValid(digitsOnly);
    }
}