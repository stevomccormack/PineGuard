namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Visa card brand specifications (ISO/IEC 7812).
/// </summary>
/// <remarks>
/// PAN = full card number. IIN = leading digits of PAN used for issuer/brand identification.
/// Visa PANs start with IIN prefix <c>4</c>.
/// </remarks>
public sealed class VisaCard : IsoPaymentCardBrand
{
    public const string Brand = "Visa";
    public const int PanLength13 = 13;
    public const int PanLength16 = 16;
    public const int PanLength19 = 19;

    public override string BrandName => Brand;

    public override int[] ValidPanLengths => [PanLength13, PanLength16, PanLength19];

    public override string[] IinPrefixes => ["4"];

    public override int[] DisplayFormatPattern => [4, 4, 4, 4];
}
