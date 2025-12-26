using PineGuard.Iso.Payments;
using PineGuard.Iso.Payments.Cards;

namespace PineGuard.Rules.Iso;

public static class IsoPaymentCardRules
{
    public static bool IsIsoPaymentCard(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var sanitized = PaymentCardUtility.Sanitize(value);

        if (sanitized.Length is < IsoPaymentCardBrand.MinPanLength or > IsoPaymentCardBrand.MaxPanLength)
            return false;

        return PanAlgorithm.IsValid(sanitized) && LuhnAlgorithm.IsValid(sanitized);
    }
}    