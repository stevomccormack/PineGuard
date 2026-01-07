using PineGuard.Common;
using PineGuard.Iso.Payments.Cards;

namespace PineGuard.Externals.Iso.Payments.Cards;

/// <summary>
/// String enumeration of supported ISO/IEC 7812 payment card brands.
/// </summary>
public sealed class CardBrand : StringEnumeration
{
    public static readonly CardBrand Visa = new(VisaCard.Brand);
    public static readonly CardBrand Mastercard = new(MastercardCard.Brand);
    public static readonly CardBrand AmericanExpress = new(AmericanExpressCard.Brand);
    public static readonly CardBrand Discover = new(DiscoverCard.Brand);
    public static readonly CardBrand DinersClub = new(DinersClubCard.Brand);
    public static readonly CardBrand Jcb = new(JcbCard.Brand);

    private CardBrand(string value) : base(value, value) { }

    /// <summary>
    /// Detects card brand from a PAN (Primary Account Number).
    /// </summary>
    public static CardBrand? FromPan(string? pan)
    {
        var detected = IsoPaymentCardBrandUtility.FromPan(pan);
        if (detected is null)
            return null;

        return IsoPaymentCardBrandUtility.ToIsoCardBrand(detected);
    }

    /// <summary>
    /// Tries to detect card brand from a PAN.
    /// </summary>
    public static bool TryFromPan(string? pan, out CardBrand? result)
    {
        result = FromPan(pan);
        return result is not null;
    }
}

public static class IsoCardBrandUtility
{
    public static IIsoPaymentCardBrand? ToIsoPaymentCardBrand(CardBrand? brand)
    {
        if (brand is null)
            return null;

        return brand.Value switch
        {
            var v when string.Equals(v, VisaCard.Brand, StringComparison.OrdinalIgnoreCase) => new VisaCard(),
            var v when string.Equals(v, MastercardCard.Brand, StringComparison.OrdinalIgnoreCase) => new MastercardCard(),
            var v when string.Equals(v, AmericanExpressCard.Brand, StringComparison.OrdinalIgnoreCase) => new AmericanExpressCard(),
            var v when string.Equals(v, DiscoverCard.Brand, StringComparison.OrdinalIgnoreCase) => new DiscoverCard(),
            var v when string.Equals(v, DinersClubCard.Brand, StringComparison.OrdinalIgnoreCase) => new DinersClubCard(),
            var v when string.Equals(v, JcbCard.Brand, StringComparison.OrdinalIgnoreCase) => new JcbCard(),
            _ => null
        };
    }
}

public static class IsoCardBrandExtension
{
    public static IIsoPaymentCardBrand? ToIsoPaymentCardBrand(this CardBrand? brand) =>
        IsoCardBrandUtility.ToIsoPaymentCardBrand(brand);
}
