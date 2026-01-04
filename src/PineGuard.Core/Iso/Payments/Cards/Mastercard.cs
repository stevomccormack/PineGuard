using PineGuard.Utils.Iso;

namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Mastercard card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// Common IIN ranges: <c>51</c>–<c>55</c> and <c>2221</c>–<c>2720</c>. PAN length is typically 16.
/// </remarks>
public sealed class MastercardCard : IsoPaymentCardBrand
{
    public const string Brand = "Mastercard";
    public const int PanExactLength = 16;

    public MastercardCard() : base(
        Brand,
        [PanExactLength],
        ["51", "52", "53", "54", "55", "2221", "2720"],
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

        return MatchesIinRange(sanitized, 51, 55) || MatchesIinRange(sanitized, 2221, 2720);
    }
}
