namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// American Express card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// Amex PANs are typically 15 digits and start with <c>34</c> or <c>37</c>. Display format is commonly <c>4-6-5</c>.
/// </remarks>
public sealed class AmericanExpressCard : IsoPaymentCardBrand
{
    public const string Brand = "American Express";
    public const int PanExactLength = 15;

    public AmericanExpressCard() : base(
        Brand,
        [PanExactLength],
        ["34", "37"],
        [4, 6, 5])
    {
    }
}
