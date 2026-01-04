using PineGuard.Utils.Iso;

namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// JCB card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// JCB IIN range is <c>3528</c>–<c>3589</c>. PAN length is typically 16.
/// </remarks>
public sealed class JcbCard : IsoPaymentCardBrand
{
    public const string Brand = "JCB";
    public const int PanExactLength = 16;

    public JcbCard() : base(
        Brand,
        [PanExactLength],
        ["3528", "3589"],
        [4, 4, 4, 4])
    {
    }

    public override bool MatchesPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return false;

        var sanitized = IsoPaymentCardUtility.Sanitize(pan);
        if (sanitized.Length != PanExactLength)
            return false;

        return MatchesIinRange(sanitized, 3528, 3589);
    }
}
