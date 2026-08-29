#if NET8_0_OR_GREATER
using System.Numerics;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate numeric values,
/// delegating to <see cref="NumberRules"/> for core validation logic.
/// </summary>
/// <seealso cref="NumberRules"/>
/// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
public static class MustNumberClauses
{
    /// <summary>
    /// Validates that the specified value must be positive.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be positive."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> Positive<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be positive.";

        var ok = NumberRules.IsPositive<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.NotPositive, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be negative.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be negative."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> Negative<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be negative.";

        var ok = NumberRules.IsNegative<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.NotNegative, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be zero.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be zero."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> Zero<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be zero.";

        var ok = NumberRules.IsZero<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.NotZero, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be zero.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be zero."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> NotZero<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must not be zero.";

        var ok = NumberRules.IsNotZero<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.Zero, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be zero or positive.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be zero or positive."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> ZeroOrPositive<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be zero or positive.";

        var ok = NumberRules.IsZeroOrPositive<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.Negative, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be zero or negative.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be zero or negative."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> ZeroOrNegative<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be zero or negative.";

        var ok = NumberRules.IsZeroOrNegative<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Sign.Positive, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be greater than the minimum.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be greater than the minimum."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> GreaterThan<T>(this IMustClause _,
        T value,
        T min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be greater than the minimum.";

        var ok = NumberRules.IsGreaterThan(value, min);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.NotGreater, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be greater than or equal to the minimum.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be greater than or equal to the minimum."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> GreaterThanOrEqual<T>(this IMustClause _,
        T value,
        T min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be greater than or equal to the minimum.";

        var ok = NumberRules.IsGreaterThanOrEqual(value, min);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.BelowMinimum, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be less than the maximum.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be less than the maximum."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> LessThan<T>(this IMustClause _,
        T value,
        T max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be less than the maximum.";

        var ok = NumberRules.IsLessThan(value, max);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.NotLess, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be less than or equal to the maximum.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be less than or equal to the maximum."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> LessThanOrEqual<T>(this IMustClause _,
        T value,
        T max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be less than or equal to the maximum.";

        var ok = NumberRules.IsLessThanOrEqual(value, max);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.Exceeded, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a valid range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a valid range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> InRange<T>(this IMustClause _,
        T value,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            return MustResult<T>.Fail(MustCodes.Number.Range.Invalid, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be within the expected range.";

        var ok = NumberRules.IsInRange(value, min, max, inclusion);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a valid range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a valid range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> OutOfRange<T>(this IMustClause _,
        T value,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        if (min.CompareTo(max) > 0)
            return MustResult<T>.Fail(MustCodes.Number.Range.Invalid, "{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be out of the expected range.";

        var ok = !NumberRules.IsInRange(value, min, max, inclusion);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a percentage between 0 and 100.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a percentage between 0 and 100."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> Percentage<T>(this IMustClause _,
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be a percentage between 0 and 100.";

        var ok = NumberRules.IsPercentage<T>(value);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Range.NotPercentage, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a non-null tolerance.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="target">The target value to compare against.</param>
    /// <param name="tolerance">The tolerance for approximate comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-null tolerance."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> Approximately<T>(this IMustClause _,
        T value,
        T target,
        T? tolerance,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        if (tolerance is null)
            return MustResult<T>.Fail(MustCodes.Number.Tolerance.Null, "{paramName} requires a non-null tolerance.", nameof(tolerance), tolerance);

        const string messageTemplate = "{paramName} must be approximately the target value.";

        var ok = NumberRules.IsApproximately(value, target, tolerance);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Proximity.NotApproximate, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value requires a non-null tolerance.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="target">The target value to compare against.</param>
    /// <param name="tolerance">The tolerance for approximate comparison.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-null tolerance."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> NotApproximately<T>(this IMustClause _,
        T value,
        T target,
        T? tolerance,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        if (tolerance is null)
            return MustResult<T>.Fail(MustCodes.Number.Tolerance.Null, "{paramName} requires a non-null tolerance.", nameof(tolerance), tolerance);

        const string messageTemplate = "{paramName} must not be approximately the target value.";

        var ok = !NumberRules.IsApproximately(value, target, tolerance);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Proximity.Approximate, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a multiple of the specified factor.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a multiple of the specified factor."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> MultipleOf<T>(this IMustClause _,
        T value,
        T factor,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must be a multiple of the specified factor.";

        var ok = NumberRules.IsMultipleOf(value, factor);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Divisibility.NotMultiple, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be a multiple of the specified factor.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a multiple of the specified factor."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<T> NotMultipleOf<T>(this IMustClause _,
        T value,
        T factor,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        const string messageTemplate = "{paramName} must not be a multiple of the specified factor.";

        var ok = !NumberRules.IsMultipleOf(value, factor);
        return MustResult<T>.FromBool(ok, MustCodes.Number.Divisibility.Multiple, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be even.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be even."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<int> Even(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be even.";

        var ok = NumberRules.IsEven(value);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Parity.Odd, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be even.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be even."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<long> Even(this IMustClause _,
        long value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be even.";

        var ok = NumberRules.IsEven(value);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Parity.Odd, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be odd.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be odd."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<int> Odd(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be odd.";

        var ok = NumberRules.IsOdd(value);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Parity.Even, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be odd.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be odd."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<long> Odd(this IMustClause _,
        long value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be odd.";

        var ok = NumberRules.IsOdd(value);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Parity.Even, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be finite.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be finite."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<float> Finite(this IMustClause _,
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be finite.";

        var ok = NumberRules.IsFinite(value);
        return MustResult<float>.FromBool(ok, MustCodes.Number.Form.NotFinite, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be finite.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be finite."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<double> Finite(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be finite.";

        var ok = NumberRules.IsFinite(value);
        return MustResult<double>.FromBool(ok, MustCodes.Number.Form.NotFinite, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be finite.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be finite."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<float> NotFinite(this IMustClause _,
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be finite.";

        var ok = !NumberRules.IsFinite(value);
        return MustResult<float>.FromBool(ok, MustCodes.Number.Form.Finite, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be finite.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be finite."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<double> NotFinite(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be finite.";

        var ok = !NumberRules.IsFinite(value);
        return MustResult<double>.FromBool(ok, MustCodes.Number.Form.Finite, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be NaN.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be NaN."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<float> NotNaN(this IMustClause _,
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be NaN.";

        var ok = !NumberRules.IsNaN(value);
        return MustResult<float>.FromBool(ok, MustCodes.Number.Form.Nan, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be NaN.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be NaN."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<double> NotNaN(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be NaN.";

        var ok = !NumberRules.IsNaN(value);
        return MustResult<double>.FromBool(ok, MustCodes.Number.Form.Nan, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be NaN.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be NaN."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<float> NaN(this IMustClause _,
        float value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be NaN.";

        var ok = NumberRules.IsNaN(value);
        return MustResult<float>.FromBool(ok, MustCodes.Number.Form.NotNan, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be NaN.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be NaN."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/number">Number Must Clauses documentation</seealso>
    public static MustResult<double> NaN(this IMustClause _,
        double value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be NaN.";

        var ok = NumberRules.IsNaN(value);
        return MustResult<double>.FromBool(ok, MustCodes.Number.Form.NotNan, messageTemplate, paramName, value, value);
    }
}
#endif
