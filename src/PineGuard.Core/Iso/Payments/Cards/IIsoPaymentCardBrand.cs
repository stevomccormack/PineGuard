namespace PineGuard.Iso.Payments.Cards;

/// <summary>
/// Defines the contract for ISO/IEC 7812 payment card brand specifications.
/// </summary>
public interface IIsoPaymentCardBrand
{
    string BrandName { get; }

    int[] ValidPanLengths { get; }

    string[] IinPrefixes { get; }

    int[] DisplayFormatPattern { get; }

    bool MatchesPan(string? pan);
}
