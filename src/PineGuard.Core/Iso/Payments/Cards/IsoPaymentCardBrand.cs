namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Base implementation for ISO/IEC 7812 payment card brand specifications.
/// </summary>
public abstract class IsoPaymentCardBrand : IIsoPaymentCardBrand
{
    public const int MinPanLength = PanAlgorithm.PanMinLength;
    public const int MaxPanLength = PanAlgorithm.PanMaxLength;
    public const int CvvExactLength = 3;
    public const int CvvAmexExactLength = 4;

    public abstract string BrandName { get; }
    public abstract int[] ValidPanLengths { get; }
    public abstract string[] IinPrefixes { get; }
    public abstract int[] DisplayFormatPattern { get; }

    public virtual bool MatchesPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return false;

        var sanitized = PaymentCardUtility.Sanitize(pan);
        if (string.IsNullOrEmpty(sanitized))
            return false;

        if (!Array.Exists(ValidPanLengths, length => sanitized.Length == length))
            return false;

        foreach (var prefix in IinPrefixes)
        {
            if (sanitized.StartsWith(prefix, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    protected bool MatchesIinRange(string sanitizedPan, int rangeStart, int rangeEnd)
    {
        if (sanitizedPan.Length < 4)
            return false;

        var prefixLength = rangeStart.ToString().Length;
        if (sanitizedPan.Length < prefixLength)
            return false;

        var panPrefix = sanitizedPan[..prefixLength];
        if (!int.TryParse(panPrefix, out var prefix))
            return false;

        return prefix >= rangeStart && prefix <= rangeEnd;
    }
}

public static class IsoPaymentCardBrandUtility
{
    private static readonly Lazy<IIsoPaymentCardBrand[]> _all = new(() =>
    [
        new VisaCard(),
        new MastercardCard(),
        new AmericanExpressCard(),
        new DiscoverCard(),
        new DinersClubCard(),
        new JcbCard(),
    ]);

    public static IReadOnlyList<IIsoPaymentCardBrand> All => _all.Value;

    public static IIsoPaymentCardBrand? DetectFromPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return null;

        foreach (var brand in All)
        {
            if (brand.MatchesPan(pan))
                return brand;
        }

        return null;
    }

    public static bool TryDetectFromPan(string? pan, out IIsoPaymentCardBrand? brand)
    {
        brand = DetectFromPan(pan);
        return brand is not null;
    }

    public static IIsoPaymentCardBrand? GetByName(string? brandName)
    {
        if (string.IsNullOrWhiteSpace(brandName))
            return null;

        foreach (var brand in All)
        {
            if (string.Equals(brand.BrandName, brandName, StringComparison.OrdinalIgnoreCase))
                return brand;
        }

        return null;
    }

    public static bool TryGetByName(string? brandName, out IIsoPaymentCardBrand? brand)
    {
        brand = GetByName(brandName);
        return brand is not null;
    }
}

public static class IsoPaymentCardBrandExtension
{
    public static IsoCardBrand? ToIsoCardBrand(this IIsoPaymentCardBrand? brand)
    {
        if (brand is null)
            return null;

        return brand.BrandName switch
        {
            VisaCard.Brand => IsoCardBrand.Visa,
            MastercardCard.Brand => IsoCardBrand.Mastercard,
            AmericanExpressCard.Brand => IsoCardBrand.AmericanExpress,
            DiscoverCard.Brand => IsoCardBrand.Discover,
            DinersClubCard.Brand => IsoCardBrand.DinersClub,
            JcbCard.Brand => IsoCardBrand.Jcb,
            _ => null
        };
    }

}