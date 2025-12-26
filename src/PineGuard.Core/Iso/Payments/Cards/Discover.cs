namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Discover card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// Common IIN patterns: <c>6011</c>, <c>65</c>, and range <c>644</c>–<c>649</c>. PAN length is typically 16.
/// </remarks>
public sealed class DiscoverCard : IsoPaymentCardBrand
{
    public const string Brand = "Discover";
    public const int PanExactLength = 16;

    public override string BrandName => Brand;

    public override int[] ValidPanLengths => [PanExactLength];

    public override string[] IinPrefixes => ["6011", "644", "649", "65"];

    public override int[] DisplayFormatPattern => [4, 4, 4, 4];

    public override bool MatchesPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return false;

        var sanitized = PaymentCardUtility.Sanitize(pan);
        if (sanitized.Length != PanExactLength)
            return false;

        return sanitized.StartsWith("6011", StringComparison.Ordinal)
            || sanitized.StartsWith("65", StringComparison.Ordinal)
            || MatchesIinRange(sanitized, 644, 649);
    }
}
