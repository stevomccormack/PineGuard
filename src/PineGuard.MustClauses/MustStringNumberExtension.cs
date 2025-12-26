using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustStringNumberExtension
{
    public static MustResult NotZero(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.NotZero(dec, paramName);

        return MustResult.FromBool(false, "{paramName} cannot be zero.", paramName, value);
    }

    public static MustResult Zero(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Zero(dec, paramName);

        return MustResult.FromBool(false, "{paramName} must be zero.", paramName, value);
    }

    public static MustResult Positive(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Positive(dec, paramName);

        return MustResult.FromBool(false, "{paramName} must be a positive number.", paramName, value);
    }

    public static MustResult ZeroOrPositive(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.ZeroOrPositive(dec, paramName);

        return MustResult.FromBool(false, "{paramName} must be zero or a positive number.", paramName, value);
    }

    public static MustResult Negative(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Negative(dec, paramName);

        return MustResult.FromBool(false, "{paramName} must be a negative number.", paramName, value);
    }

    public static MustResult ZeroOrNegative(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.ZeroOrNegative(dec, paramName);

        return MustResult.FromBool(false, "{paramName} must be zero or a negative number.", paramName, value);
    }
}
