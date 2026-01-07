using PineGuard.Rules;

namespace PineGuard.Externals.Iso.Payments;

public static class LuhnAlgorithm
{
    private const int LuhnModulus = 10;
    private const int LuhnMultiplier = 2;
    private const int LuhnReduction = 9;

    public static bool IsValid(string? digitsOnly)
    {
        if (digitsOnly is null)
            return false;

        if (!StringRules.IsDigitsOnly(digitsOnly))
            return false;

        var sum = 0;
        var alternate = false;

        for (var i = digitsOnly.Length - 1; i >= 0; i--)
        {
            var digit = digitsOnly[i] - '0';
            sum += ApplyLuhnTransform(digit, alternate);
            alternate = !alternate;
        }

        return IsValidChecksum(sum);
    }

    private static int ApplyLuhnTransform(int digit, bool shouldDouble)
    {
        if (!shouldDouble)
            return digit;

        digit *= LuhnMultiplier;
        return digit > LuhnReduction ? digit - LuhnReduction : digit;
    }

    private static bool IsValidChecksum(int checksum)
    {
        return checksum % LuhnModulus == 0;
    }   
}
