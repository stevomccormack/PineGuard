using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class PaymentCards
    {
        public static bool IsPaymentCard(string? value) =>
            IsoPaymentCardRules.IsIsoPaymentCard(value);

        public static bool IsPaymentCard(string? value, char[] allowedSeparators) =>
            IsoPaymentCardRules.IsIsoPaymentCard(value, allowedSeparators);
    }
}
