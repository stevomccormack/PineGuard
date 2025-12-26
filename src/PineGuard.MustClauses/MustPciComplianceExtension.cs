using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustPciComplianceExtension
{
    public static MustResult MaskedCard(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate =
            "{paramName} must be a PCI DSS masked card number (first 6 and last 4 digits visible).";

        if (string.IsNullOrEmpty(value))
            return MustResult.FromBool(false, messageTemplate, paramName, value);

        var ok = value.Contains('*') || value.Contains('X') || value.Contains('x');
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }

    public static MustResult CardBrand(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate =
            "{paramName} is not a recognized card brand (Visa, MasterCard, Amex).";

        if (string.IsNullOrEmpty(value))
            return MustResult.FromBool(false, messageTemplate, paramName, value);

        var sanitized = new string(value.Where(char.IsDigit).ToArray());
        
        if (sanitized.StartsWith("4") && (sanitized.Length == 13 || sanitized.Length == 16 || sanitized.Length == 19))
            return MustResult.FromBool(true, "{paramName} is a valid Visa card.", paramName, value);

        if (sanitized.Length == 16 && (sanitized.StartsWith("51") || sanitized.StartsWith("52") || sanitized.StartsWith("53") || sanitized.StartsWith("54") || sanitized.StartsWith("55")))
            return MustResult.FromBool(true, "{paramName} is a valid MasterCard.", paramName, value);

        if (sanitized.Length == 15 && (sanitized.StartsWith("34") || sanitized.StartsWith("37")))
            return MustResult.FromBool(true, "{paramName} is a valid American Express card.", paramName, value);

        return MustResult.FromBool(false, messageTemplate, paramName, value);
    }
}
