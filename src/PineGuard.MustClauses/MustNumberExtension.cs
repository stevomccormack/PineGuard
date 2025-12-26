using System.Numerics;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustNumberExtension
{
    public static MustResult Positive<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value > TNumber.Zero,
            "{paramName} must be a positive number.",
            paramName,
            value);

    public static MustResult Zero<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value == TNumber.Zero,
            "{paramName} must be zero.",
            paramName,
            value);

    public static MustResult NotZero<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value != TNumber.Zero,
            "{paramName} must not be zero.",
            paramName,
            value);

    public static MustResult Negative<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value < TNumber.Zero,
            "{paramName} must be a negative number.",
            paramName,
            value);

    public static MustResult ZeroOrPositive<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value >= TNumber.Zero,
            "{paramName} must be zero or a positive number.",
            paramName,
            value);

    public static MustResult ZeroOrNegative<TNumber>(
        this IMustClause _,
        TNumber value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where TNumber : INumber<TNumber>
        => MustResult.FromBool(
            value <= TNumber.Zero,
            "{paramName} must be zero or a negative number.",
            paramName,
            value);
}
