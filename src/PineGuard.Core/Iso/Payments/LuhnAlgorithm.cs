using PineGuard.Rules;

namespace PineGuard.Iso.Payments;

public static class LuhnAlgorithm
{
    private const int LuhnModulus = 10;
    private const int LuhnMultiplier = 2;
    private const int LuhnReduction = 9;

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var digits = value.Trim();
        if (!NumberStringRules.IsDigitsOnly(digits))
            return false;

        var sum = 0;
        var alternate = false;

        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var n = digits[i] - '0';
            if (alternate)
            {
                n *= LuhnMultiplier;
                if (n > LuhnReduction)
                    n -= LuhnReduction;
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % LuhnModulus == 0;
    }
}
