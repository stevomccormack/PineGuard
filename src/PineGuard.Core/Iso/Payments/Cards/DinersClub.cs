using PineGuard.Utils.Iso;

namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Diners Club card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// Common IIN patterns: <c>36</c>, <c>38</c>, and range <c>300</c>–<c>305</c>. PAN length is typically 14.
/// </remarks>
public sealed class DinersClubCard : IsoPaymentCardBrand
{
    public const string Brand = "Diners Club";
    public const int PanExactLength = 14;

    public override string BrandName => Brand;

    public override int[] ValidPanLengths => [PanExactLength];

    public override string[] IinPrefixes => ["300", "305", "36", "38"];

    public override int[] DisplayFormatPattern => [4, 6, 4];

    public override bool MatchesPan(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            return false;

        var sanitized = IsoPaymentCardUtility.Sanitize(pan);
        if (string.IsNullOrEmpty(sanitized))
            return false;

        return MatchesIinRange(sanitized, 300, 305)
            || sanitized.StartsWith("36", StringComparison.Ordinal)
            || sanitized.StartsWith("38", StringComparison.Ordinal);
    }
}
