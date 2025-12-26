using PineGuard.Common;

namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// String enumeration of supported ISO/IEC 7812 payment card brands.
/// </summary>
public sealed class IsoCardBrand : StringEnumeration
{
    public static readonly IsoCardBrand Visa = new(VisaCard.Brand);
    public static readonly IsoCardBrand Mastercard = new(MastercardCard.Brand);
    public static readonly IsoCardBrand AmericanExpress = new(AmericanExpressCard.Brand);
    public static readonly IsoCardBrand Discover = new(DiscoverCard.Brand);
    public static readonly IsoCardBrand DinersClub = new(DinersClubCard.Brand);
    public static readonly IsoCardBrand Jcb = new(JcbCard.Brand);

    private IsoCardBrand(string value) : base(value, value) { }

    /// <summary>
    /// Detects card brand from a PAN (Primary Account Number).
    /// </summary>
    public static IsoCardBrand? FromPan(string? pan)
    {
        var detected = IsoPaymentCardBrandUtility.DetectFromPan(pan);
        if (detected is null)
            return null;

        return detected.BrandName switch
        {
            VisaCard.Brand => Visa,
            MastercardCard.Brand => Mastercard,
            AmericanExpressCard.Brand => AmericanExpress,
            DiscoverCard.Brand => Discover,
            DinersClubCard.Brand => DinersClub,
            JcbCard.Brand => Jcb,
            _ => null
        };
    }

    /// <summary>
    /// Tries to detect card brand from a PAN.
    /// </summary>
    public static bool TryFromPan(string? pan, out IsoCardBrand? result)
    {
        result = FromPan(pan);
        return result is not null;
    }
}

public static class IsoCardBrandUtility
{
    public static IIsoPaymentCardBrand? ToIsoPaymentCardBrand(IsoCardBrand? brand)
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
    public static IIsoPaymentCardBrand? ToIsoPaymentCardBrand(this IsoCardBrand? brand)
    {
        return IsoCardBrandUtility.ToIsoPaymentCardBrand(brand);
    }
}
