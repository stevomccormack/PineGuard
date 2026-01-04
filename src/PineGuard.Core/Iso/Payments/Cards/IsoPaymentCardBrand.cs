using PineGuard.Utils.Iso;
using System.Text.RegularExpressions;

namespace PineGuard.Iso.Payments.Cards;

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
            throw new ArgumentException("BrandName cannot be null or whitespace.", nameof(brandName));

        if (validPanLengths.Length == 0)
            throw new ArgumentException("ValidPanLengths cannot be empty.", nameof(validPanLengths));

        for (var i = 0; i < validPanLengths.Length; i++)
        {
            var len = validPanLengths[i];
            if (len < MinPanLength || len > MaxPanLength)
                throw new ArgumentException($"ValidPanLengths must be between {MinPanLength} and {MaxPanLength}.", nameof(validPanLengths));
        }

        if (iinPrefixes.Length == 0)
            throw new ArgumentException("IinPrefixes cannot be empty.", nameof(iinPrefixes));

        for (var i = 0; i < iinPrefixes.Length; i++)
        {
            var prefix = iinPrefixes[i];
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("IinPrefixes cannot contain null or whitespace values.", nameof(iinPrefixes));

            for (var j = 0; j < prefix.Length; j++)
            {
                var ch = prefix[j];
                if (ch is < '0' or > '9')
                    throw new ArgumentException("IinPrefixes must contain digits only.", nameof(iinPrefixes));
            }
        }

        if (displayFormatPattern.Length == 0)
            throw new ArgumentException("DisplayFormatPattern cannot be empty.", nameof(displayFormatPattern));

        var total = 0;
        for (var i = 0; i < displayFormatPattern.Length; i++)
        {
            var part = displayFormatPattern[i];
            if (part <= 0)
                throw new ArgumentException("DisplayFormatPattern parts must be greater than 0.", nameof(displayFormatPattern));

            total += part;
        }

        if (!Array.Exists(validPanLengths, length => length == total))
            throw new ArgumentException("DisplayFormatPattern must sum to a valid PAN length.", nameof(displayFormatPattern));

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

        foreach (var prefix in _iinPrefixes)
        {
            if (sanitized.StartsWith(prefix, StringComparison.Ordinal))
                return true;
        }

        return false;
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

    public static IIsoPaymentCardBrand? FromPan(string? pan)
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

    public static bool TryFromPan(string? pan, out IIsoPaymentCardBrand? brand)
    {
        brand = FromPan(pan);
        return brand is not null;
    }

    public static IIsoPaymentCardBrand? FromBrandName(string? brandName)
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