#if NET8_0_OR_GREATER
using System.Numerics;
using PineGuard.Common;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure numeric validation predicates for any <see cref="INumber{TSelf}"/> type.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/number">Number Rules documentation</seealso>
public static class NumberRules
{
    /// <summary>
    /// The smallest value a percentage can take (<c>0</c>).
    /// </summary>
    public const int MinPercentage = 0;

    /// <summary>
    /// The largest value a percentage can take (<c>100</c>).
    /// </summary>
    public const int MaxPercentage = 100;

    /// <summary>
    /// Determines whether the specified value is strictly positive (greater than zero).
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is greater than zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsPositive<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value > T.Zero;

    /// <summary>
    /// Determines whether the specified value is strictly negative (less than zero).
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is less than zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsNegative<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value < T.Zero;

    /// <summary>
    /// Determines whether the specified value is exactly zero.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> equals zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsZero<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value == T.Zero;

    /// <summary>
    /// Determines whether the specified value is not zero.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsNotZero<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value != T.Zero;

    /// <summary>
    /// Determines whether the specified value is zero or positive (greater than or equal to zero).
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is &gt;= zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsZeroOrPositive<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value >= T.Zero;

    /// <summary>
    /// Determines whether the specified value is zero or negative (less than or equal to zero).
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is &lt;= zero; otherwise, <see langword="false"/>.</returns>
    public static bool IsZeroOrNegative<T>(T? value) where T : struct, INumber<T> =>
        value is not null && value.Value <= T.Zero;

    /// <summary>
    /// Determines whether the specified value is greater than <paramref name="min"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The exclusive lower bound.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> &gt; <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsGreaterThan<T>(T? value, T min) where T : struct, INumber<T> =>
        value > min;

    /// <summary>
    /// Determines whether the specified value is greater than or equal to <paramref name="min"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> &gt;= <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsGreaterThanOrEqual<T>(T? value, T min) where T : struct, INumber<T> =>
        value >= min;

    /// <summary>
    /// Determines whether the specified value is less than <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> &lt; <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsLessThan<T>(T? value, T max) where T : struct, INumber<T> =>
        value < max;

    /// <summary>
    /// Determines whether the specified value is less than or equal to <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="max">The inclusive upper bound.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> &lt;= <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsLessThanOrEqual<T>(T? value, T max) where T : struct, INumber<T> =>
        value <= max;

    /// <summary>
    /// Determines whether the specified value falls within the range [<paramref name="min"/>, <paramref name="max"/>].
    /// </summary>
    /// <typeparam name="T">A value type that implements <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="min">The lower bound of the range.</param>
    /// <param name="max">The upper bound of the range.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the specified range; otherwise, <see langword="false"/>.</returns>
    public static bool IsInRange<T>(T? value, T min, T max, Inclusion inclusion = Inclusion.Inclusive)
        where T : struct, IComparable<T> =>
        value is not null && RuleComparison.IsBetween(value.Value, min, max, inclusion);

    /// <summary>
    /// Determines whether the specified value is a percentage — between
    /// <see cref="MinPercentage"/> and <see cref="MaxPercentage"/> inclusive.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within 0–100; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// The scale is the everyday one a human reads off a form — <c>0</c> to <c>100</c>, not <c>0.0</c> to
    /// <c>1.0</c> — so <c>0.5</c> is half a percent, not fifty percent.
    /// </remarks>
    public static bool IsPercentage<T>(T? value) where T : struct, INumber<T> =>
        IsInRange(value, T.CreateChecked(MinPercentage), T.CreateChecked(MaxPercentage));

    /// <summary>
    /// Determines whether the specified value is approximately equal to <paramref name="target"/>
    /// within the given <paramref name="tolerance"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="target">The target value to compare against.</param>
    /// <param name="tolerance">
    /// The maximum allowed absolute difference. If <see langword="null"/> or negative, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <c>|value - target| &lt;= tolerance</c>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The distance between <paramref name="value"/> and <paramref name="target"/> is computed by subtracting the
    /// smaller from the larger (never <c>Abs</c> of a raw subtraction), which avoids wraparound for unsigned
    /// <typeparamref name="T"/>. The subtraction runs in a checked context so that a true
    /// difference too large to represent in <typeparamref name="T"/> (e.g. <see cref="int.MaxValue"/> vs.
    /// <see cref="int.MinValue"/>) is treated as out of tolerance rather than throwing <see cref="OverflowException"/>.
    /// </remarks>
    public static bool IsApproximately<T>(T? value, T target, T? tolerance) where T : struct, INumber<T>
    {
        if (value is null || tolerance is null)
            return false;

        if (tolerance.Value < T.Zero)
            return false;

        T diff;
        try
        {
            diff = checked(value.Value >= target ? value.Value - target : target - value.Value);
        }
        catch (OverflowException)
        {
            return false;
        }

        return diff <= tolerance.Value;
    }

    /// <summary>
    /// Determines whether the specified value is a multiple of <paramref name="factor"/>.
    /// </summary>
    /// <typeparam name="T">A numeric value type that implements <see cref="INumber{TSelf}"/>.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="factor">The factor to test divisibility against. If zero, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is evenly divisible by <paramref name="factor"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// For signed integer <typeparamref name="T"/>, <c>T.MinValue % -1</c> overflows at the CLR level because the
    /// internally computed quotient (<c>T.MinValue / -1</c>) exceeds <c>T.MaxValue</c>. Mathematically every value
    /// is evenly divisible by <c>-1</c>, so that case is treated as <see langword="true"/> instead of throwing
    /// <see cref="OverflowException"/>.
    /// </remarks>
    public static bool IsMultipleOf<T>(T? value, T factor) where T : struct, INumber<T>
    {
        if (value is null)
            return false;

        if (factor == T.Zero)
            return false;

        try
        {
            return value.Value % factor == T.Zero;
        }
        catch (OverflowException)
        {
            return true;
        }
    }

    /// <summary>
    /// Determines whether the specified integer value is even.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is divisible by 2; otherwise, <see langword="false"/>.</returns>
    public static bool IsEven(int? value) =>
        IsMultipleOf(value, 2);

    /// <summary>
    /// Determines whether the specified long integer value is even.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is divisible by 2; otherwise, <see langword="false"/>.</returns>
    public static bool IsEven(long? value) =>
        IsMultipleOf(value, 2);

    /// <summary>
    /// Determines whether the specified integer value is odd.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not divisible by 2; otherwise, <see langword="false"/>.</returns>
    public static bool IsOdd(int? value) =>
        value is not null && !IsMultipleOf(value, 2);

    /// <summary>
    /// Determines whether the specified long integer value is odd.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not divisible by 2; otherwise, <see langword="false"/>.</returns>
    public static bool IsOdd(long? value) =>
        value is not null && !IsMultipleOf(value, 2);

    /// <summary>
    /// Determines whether the specified float is a finite number (not NaN, not infinite).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is finite; otherwise, <see langword="false"/>.</returns>
    public static bool IsFinite(float? value) =>
        value is not null && float.IsFinite(value.Value);

    /// <summary>
    /// Determines whether the specified double is a finite number (not NaN, not infinite).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is finite; otherwise, <see langword="false"/>.</returns>
    public static bool IsFinite(double? value) =>
        value is not null && double.IsFinite(value.Value);

    /// <summary>
    /// Determines whether the specified float is NaN (not a number).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is NaN; otherwise, <see langword="false"/>.</returns>
    public static bool IsNaN(float? value) =>
        value is not null && float.IsNaN(value.Value);

    /// <summary>
    /// Determines whether the specified double is NaN (not a number).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is NaN; otherwise, <see langword="false"/>.</returns>
    public static bool IsNaN(double? value) =>
        value is not null && double.IsNaN(value.Value);
}
#endif
