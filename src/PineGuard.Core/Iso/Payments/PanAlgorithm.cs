using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.Iso.Payments;

/// <summary>
/// Validates Primary Account Number (PAN) length according to ISO/IEC 7812.
/// </summary>
public static class PanAlgorithm
{
    public const int PanMinLength = 12;
    public const int PanMaxLength = 19;

    public static bool IsValid(string? digitsOnly)
    {
        if (digitsOnly is null)
            return false;

        if (!StringRules.IsDigitsOnly(digitsOnly))
            return false;

        return RuleComparison.IsBetween(digitsOnly.Length, PanMinLength, PanMaxLength, RangeInclusion.Inclusive);
    }
}
