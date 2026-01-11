using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static class MustStringNumberExtension
{
    public static MustResult<decimal> NotZero(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.NotZero(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} cannot be zero.", paramName, value);
    }

    public static MustResult<decimal> Zero(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Zero(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} must be zero.", paramName, value);
    }

    public static MustResult<decimal> Positive(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Positive(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} must be a positive number.", paramName, value);
    }

    public static MustResult<decimal> ZeroOrPositive(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.ZeroOrPositive(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} must be zero or a positive number.", paramName, value);
    }

    public static MustResult<decimal> Negative(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.Negative(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} must be a negative number.", paramName, value);
    }

    public static MustResult<decimal> ZeroOrNegative(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (decimal.TryParse(value, out var dec))
            return _.ZeroOrNegative(dec, paramName);

        return MustResult<decimal>.Fail("{paramName} must be zero or a negative number.", paramName, value);
    }
}
