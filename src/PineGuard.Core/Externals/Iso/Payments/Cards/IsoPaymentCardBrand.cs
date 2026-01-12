using PineGuard.Extensions;
using PineGuard.Utils.Iso;
using System.Text.RegularExpressions;

namespace PineGuard.Externals.Iso.Payments.Cards;

/// <summary>
/// Base implementation for ISO/IEC 7812 payment card brand specifications.
/// </summary>
public abstract partial class IsoPaymentCardBrand : IIsoPaymentCardBrand
{
    public const string IsoStandard = "ISO/IEC 7812";

    public const int MinPanLength = PanAlgorithm.PanMinLength;
    public const int MaxPanLength = PanAlgorithm.PanMaxLength;
    public const int CvvExactLength = 3;
    public const int CvvAmexExactLength = 4;

    public const char SpaceSeparator = ' ';
    public const char DashSeparator = '-';
    public static readonly char[] DefaultAllowedSeparators = [SpaceSeparator, DashSeparator];

    public const string IsoCardNumberWithSeparatorsPattern = "^[0-9 \\-]+$";

    [GeneratedRegex(IsoCardNumberWithSeparatorsPattern, RegexOptions.CultureInvariant)]
    public static partial Regex IsoCardNumberWithSeparatorsRegex();

    private readonly int[] _validPanLengths;
    private readonly string[] _iinPrefixes;
    private readonly int[] _displayFormatPattern;

    protected IsoPaymentCardBrand(
        string brandName,
        int[] validPanLengths,
        string[] iinPrefixes,
        int[] displayFormatPattern)
    {
        ArgumentNullException.ThrowIfNull(brandName);
        ArgumentNullException.ThrowIfNull(validPanLengths);
        ArgumentNullException.ThrowIfNull(iinPrefixes);
        ArgumentNullException.ThrowIfNull(displayFormatPattern);

        if (string.IsNullOrWhiteSpace(brandName))
            throw new ArgumentException($"{nameof(brandName).TitleCase()} cannot be null or whitespace.", nameof(brandName));

        if (validPanLengths.Length == 0)
            throw new ArgumentException($"{nameof(validPanLengths).TitleCase()} cannot be empty.", nameof(validPanLengths));

        foreach (var len in validPanLengths)
        {
            if (len < MinPanLength || len > MaxPanLength)
                throw new ArgumentOutOfRangeException(nameof(validPanLengths), len, $"{nameof(validPanLengths).TitleCase()} must contain values between {MinPanLength} and {MaxPanLength}.");
        }

        if (iinPrefixes.Length == 0)
            throw new ArgumentException($"{nameof(iinPrefixes).TitleCase()} cannot be empty.", nameof(iinPrefixes));

        foreach (var prefix in iinPrefixes)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException($"{nameof(iinPrefixes).TitleCase()} cannot contain null or whitespace values.", nameof(iinPrefixes));

            foreach (var ch in prefix)
            {
                if (ch is < '0' or > '9')
                    throw new ArgumentException($"{nameof(iinPrefixes).TitleCase()} must contain digits only.", nameof(iinPrefixes));
            }
        }

        if (displayFormatPattern.Length == 0)
            throw new ArgumentException($"{nameof(displayFormatPattern).TitleCase()} cannot be empty.", nameof(displayFormatPattern));

        var total = 0;
        foreach (var part in displayFormatPattern)
        {
            if (part <= 0)
                throw new ArgumentOutOfRangeException(nameof(displayFormatPattern), part, $"{nameof(displayFormatPattern).TitleCase()} must contain values greater than 0.");

            total += part;
        }

        if (!Array.Exists(validPanLengths, length => length == total))
            throw new ArgumentException($"{nameof(displayFormatPattern).TitleCase()} must sum to a valid PAN length.", nameof(displayFormatPattern));

        BrandName = brandName;

        _validPanLengths = validPanLengths;
        _iinPrefixes = iinPrefixes;
        _displayFormatPattern = displayFormatPattern;
    }

    public string BrandName { get; }

    public int[] ValidPanLengths => _validPanLengths;

    public string[] IinPrefixes => _iinPrefixes;

    public int[] DisplayFormatPattern => _displayFormatPattern;

    public virtual bool MatchesPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return false;

        var sanitized = IsoPaymentCardUtility.Sanitize(pan);
        if (string.IsNullOrEmpty(sanitized))
            return false;

        if (!Array.Exists(_validPanLengths, length => sanitized.Length == length))
            return false;

        return _iinPrefixes.Any(prefix => sanitized.StartsWith(prefix, StringComparison.Ordinal));
    }

    protected static bool MatchesIinRange(string sanitizedPan, int rangeStart, int rangeEnd)
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
    private static readonly Lazy<IIsoPaymentCardBrand[]> AllBrands = new(() =>
    [
        new VisaCard(),
        new MastercardCard(),
        new AmericanExpressCard(),
        new DiscoverCard(),
        new DinersClubCard(),
        new JcbCard(),
    ]);

    public static IReadOnlyList<IIsoPaymentCardBrand> All => AllBrands.Value;

    public static IIsoPaymentCardBrand? FromPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return null;

        return All.FirstOrDefault(brand => brand.MatchesPan(pan));
    }

    public static bool TryFromPan(string? pan, out IIsoPaymentCardBrand? brand)
    {
        brand = FromPan(pan);
        return brand is not null;
    }

    public static IIsoPaymentCardBrand? FromBrandName(string? brandName)
    {
        if (string.IsNullOrWhiteSpace(brandName))
            return null;

        var trimmed = brandName.Trim();

        return All.FirstOrDefault(brand => string.Equals(brand.BrandName, trimmed, StringComparison.OrdinalIgnoreCase));
    }

    public static bool TryFromBrandName(string? brandName, out IIsoPaymentCardBrand? brand)
    {
        brand = FromBrandName(brandName);
        return brand is not null;
    }

    public static CardBrand? ToIsoCardBrand(IIsoPaymentCardBrand? brand)
    {
        if (brand is null)
            return null;

        return brand.BrandName switch
        {
            VisaCard.Brand => CardBrand.Visa,
            MastercardCard.Brand => CardBrand.Mastercard,
            AmericanExpressCard.Brand => CardBrand.AmericanExpress,
            DiscoverCard.Brand => CardBrand.Discover,
            DinersClubCard.Brand => CardBrand.DinersClub,
            JcbCard.Brand => CardBrand.Jcb,
            _ => null
        };
    }
}

public static class IsoPaymentCardBrandExtension
{
    public static CardBrand? ToIsoCardBrand(this IIsoPaymentCardBrand? brand) =>
        IsoPaymentCardBrandUtility.ToIsoCardBrand(brand);
}