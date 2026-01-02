using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustPciComplianceExtension
{
    public static MustResult<string> MaskedCard(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate =
            "{paramName} must be a PCI DSS masked card number (first 6 and last 4 digits visible).";

        if (string.IsNullOrEmpty(value))
            return MustResult<string>.Fail(messageTemplate, paramName, value);

        var ok = value.Contains('*') || value.Contains('X') || value.Contains('x');
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    public static MustResult<string> CardBrand(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate =
            "{paramName} is not a recognized card brand (Visa, MasterCard, Amex).";

        if (string.IsNullOrEmpty(value))
            return MustResult<string>.Fail(messageTemplate, paramName, value);

        var sanitized = new string([.. value.Where(char.IsDigit)]);

        if (sanitized.StartsWith("4") && (sanitized.Length == 13 || sanitized.Length == 16 || sanitized.Length == 19))
            return MustResult<string>.Ok(value, value, paramName);

        if (sanitized.Length == 16 && (sanitized.StartsWith("51") || sanitized.StartsWith("52") || sanitized.StartsWith("53") || sanitized.StartsWith("54") || sanitized.StartsWith("55")))
            return MustResult<string>.Ok(value, value, paramName);

        if (sanitized.Length == 15 && (sanitized.StartsWith("34") || sanitized.StartsWith("37")))
            return MustResult<string>.Ok(value, value, paramName);

        return MustResult<string>.Fail(messageTemplate, paramName, value);
    }
}
