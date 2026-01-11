using System.Numerics;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static class MustNumberExtension
{
    public static MustResult<TNumber> Positive<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value > TNumber.Zero,
            "{paramName} must be a positive number.",
            paramName,
            value,
            value);

    public static MustResult<TNumber> Zero<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value == TNumber.Zero,
            "{paramName} must be zero.",
            paramName,
            value,
            value);

    public static MustResult<TNumber> NotZero<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value != TNumber.Zero,
            "{paramName} must not be zero.",
            paramName,
            value,
            value);

    public static MustResult<TNumber> Negative<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value < TNumber.Zero,
            "{paramName} must be a negative number.",
            paramName,
            value,
            value);

    public static MustResult<TNumber> ZeroOrPositive<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value >= TNumber.Zero,
            "{paramName} must be zero or a positive number.",
            paramName,
            value,
            value);

    public static MustResult<TNumber> ZeroOrNegative<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult<TNumber>.FromBool(
            value <= TNumber.Zero,
            "{paramName} must be zero or a negative number.",
            paramName,
            value,
            value);
}
