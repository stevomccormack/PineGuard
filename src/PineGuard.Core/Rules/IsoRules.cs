using PineGuard.Providers;
using System.Globalization;

namespace PineGuard.Rules;

public static class IsoRules
{
    public static bool IsIso3166Alpha2(string? value, IIsoDataProvider provider) =>
        IsAlpha(value, 2) && provider.IsIso3166Alpha2(value!);

    public static bool IsNotIso3166Alpha2(string? value, IIsoDataProvider provider) =>
        !IsIso3166Alpha2(value, provider);

    public static bool IsIso3166Alpha3(string? value, IIsoDataProvider provider) =>
        IsAlpha(value, 3) && provider.IsIso3166Alpha3(value!);

    public static bool IsNotIso3166Alpha3(string? value, IIsoDataProvider provider) =>
        !IsIso3166Alpha3(value, provider);

    public static bool IsIso3166Numeric(string? value, IIsoDataProvider provider) =>
        IsDigits(value, 3) && provider.IsIso3166Numeric(value!);

    public static bool IsNotIso3166Numeric(string? value, IIsoDataProvider provider) =>
        !IsIso3166Numeric(value, provider);

    public static bool IsIso4217CurrencyCode(string? value, IIsoDataProvider provider) =>
        IsAlpha(value, 3) && provider.IsIso4217CurrencyCode(value!);

    public static bool IsNotIso4217CurrencyCode(string? value, IIsoDataProvider provider) =>
        !IsIso4217CurrencyCode(value, provider);

    public static bool IsIso639Alpha2(string? value, IIsoDataProvider provider) =>
        IsAlpha(value, 2) && provider.IsIso639Alpha2(value!);

    public static bool IsNotIso639Alpha2(string? value, IIsoDataProvider provider) =>
        !IsIso639Alpha2(value, provider);

    public static bool IsIso639Alpha3(string? value, IIsoDataProvider provider) =>
        IsAlpha(value, 3) && provider.IsIso639Alpha3(value!);

    public static bool IsNotIso639Alpha3(string? value, IIsoDataProvider provider) =>
        !IsIso639Alpha3(value, provider);

    public static bool IsIso8601DateOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return DateOnly.TryParseExact(
            value.Trim(),
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
    }

    public static bool IsNotIso8601DateOnly(string? value) => !IsIso8601DateOnly(value);

    public static bool IsIso8601DateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var text = value.Trim();

        var formats = new[]
        {
            "yyyy-MM-dd'T'HH:mm:ss",
            "yyyy-MM-dd'T'HH:mm:ss.FFFFFFF",
            "yyyy-MM-dd'T'HH:mm:ss'Z'",
            "yyyy-MM-dd'T'HH:mm:ss.FFFFFFF'Z'",
            "yyyy-MM-dd'T'HH:mm:sszzz",
            "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFzzz"
        };

        return DateTimeOffset.TryParseExact(
            text,
            formats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out _);
    }

    public static bool IsNotIso8601DateTime(string? value) => !IsIso8601DateTime(value);

    public static bool IsIso7812Luhn(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var digits = value.Trim();
        if (!IsDigitsOnly(digits))
        {
            return false;
        }

        var sum = 0;
        var alternate = false;

        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var n = digits[i] - '0';
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                {
                    n -= 9;
                }
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    public static bool IsNotIso7812Luhn(string? value) => !IsIso7812Luhn(value);

    public static bool IsIso7812Pan(string? value, RangeInclusion inclusion = RangeInclusion.Inclusive)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var digits = value.Trim();
        if (!IsDigitsOnly(digits))
        {
            return false;
        }

        return RuleComparison.IsBetween(digits.Length, 12, 19, inclusion);
    }

    public static bool IsNotIso7812Pan(string? value, RangeInclusion inclusion = RangeInclusion.Inclusive) =>
        !IsIso7812Pan(value, inclusion);

    public static bool IsIso7812CreditCard(string? value) =>
        IsIso7812Pan(value, RangeInclusion.Inclusive) && IsIso7812Luhn(value);

    public static bool IsNotIso7812CreditCard(string? value) => !IsIso7812CreditCard(value);

    private static bool IsAlpha(string? value, int length)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var s = value.Trim();
        if (s.Length != length)
        {
            return false;
        }

        foreach (var ch in s)
        {
            if (!char.IsLetter(ch))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsDigits(string? value, int length)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var s = value.Trim();
        if (s.Length != length)
        {
            return false;
        }

        return IsDigitsOnly(s);
    }

    private static bool IsDigitsOnly(string value)
    {
        foreach (var ch in value)
        {
            if (ch < '0' || ch > '9')
            {
                return false;
            }
        }

        return true;
    }
}
