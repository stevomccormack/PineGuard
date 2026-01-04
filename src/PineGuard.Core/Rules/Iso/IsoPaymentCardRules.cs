using PineGuard.Common;
using PineGuard.Iso.Payments;
using PineGuard.Iso.Payments.Cards;
using PineGuard.Utils;

namespace PineGuard.Rules.Iso;

public static partial class IsoPaymentCardRules
{
    public static bool IsIsoPaymentCard(string? value) =>
        IsIsoPaymentCard(value, IsoPaymentCardBrand.DefaultAllowedSeparators);

    public static bool IsIsoPaymentCard(string? value, char[] allowedSeparators)
    {
        ArgumentNullException.ThrowIfNull(allowedSeparators);

        if (allowedSeparators.Length == 0)
        {
            if (!StringUtility.TryParseDigitsOnly(value, out var digitsOnly))
                return false;

            if (!RuleComparison.IsBetween(digitsOnly.Length, IsoPaymentCardBrand.MinPanLength, IsoPaymentCardBrand.MaxPanLength, RangeInclusion.Inclusive))
                return false;

            return PanAlgorithm.IsValid(digitsOnly) && LuhnAlgorithm.IsValid(digitsOnly);
        }

        if (allowedSeparators.Length == IsoPaymentCardBrand.DefaultAllowedSeparators.Length
            && allowedSeparators.Contains(IsoPaymentCardBrand.SpaceSeparator)
            && allowedSeparators.Contains(IsoPaymentCardBrand.DashSeparator))
        {
            if (!StringUtility.TryGetTrimmed(value, out var trimmed))
                return false;

            if (!IsoPaymentCardBrand.IsoCardNumberWithSeparatorsRegex().IsMatch(trimmed))
                return false;
        }

        if (!StringUtility.TryParseDigits(value, out var parsedDigitsOnly, allowedSeparators))
            return false;

        if (!RuleComparison.IsBetween(parsedDigitsOnly.Length, IsoPaymentCardBrand.MinPanLength, IsoPaymentCardBrand.MaxPanLength, RangeInclusion.Inclusive))
            return false;

        return PanAlgorithm.IsValid(parsedDigitsOnly) && LuhnAlgorithm.IsValid(parsedDigitsOnly);
    }
}