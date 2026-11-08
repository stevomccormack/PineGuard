using PineGuard.Common;

namespace PineGuard.Rules;

/// <summary>
/// Provides numeric validation predicates, including concrete numeric operations on every supported target.
/// </summary>
public static partial class NumberRules
{
    /// <summary>
    /// The smallest value a percentage can take (<c>0</c>).
    /// </summary>
    public const int MinPercentage = 0;

    /// <summary>
    /// The largest value a percentage can take (<c>100</c>).
    /// </summary>
    public const int MaxPercentage = 100;

    /// <summary>Determines whether a decimal value is positive.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is greater than zero.</returns>
    public static bool IsPositive(decimal? value) => value > 0;

    /// <summary>Determines whether a decimal value is negative.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is less than zero.</returns>
    public static bool IsNegative(decimal? value) => value < 0;

    /// <summary>Determines whether a decimal value is zero.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is zero.</returns>
    public static bool IsZero(decimal? value) => value == 0;

    /// <summary>Determines whether a decimal value is nonzero.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is present and differs from zero.</returns>
    public static bool IsNotZero(decimal? value) => value is not null && value != 0;

    /// <summary>Determines whether a decimal value is zero or positive.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is greater than or equal to zero.</returns>
    public static bool IsZeroOrPositive(decimal? value) => value >= 0;

    /// <summary>Determines whether a decimal value is zero or negative.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is less than or equal to zero.</returns>
    public static bool IsZeroOrNegative(decimal? value) => value <= 0;

    /// <summary>Determines whether a decimal value is greater than another.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="other">The exclusive lower bound.</param>
    /// <returns>True when value is greater than other.</returns>
    public static bool IsGreaterThan(decimal? value, decimal other) => value > other;

    /// <summary>Determines whether a decimal value is greater than or equal to another.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="other">The inclusive lower bound.</param>
    /// <returns>True when value is greater than or equal to other.</returns>
    public static bool IsGreaterThanOrEqual(decimal? value, decimal other) => value >= other;

    /// <summary>Determines whether a decimal value is less than another.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="other">The exclusive upper bound.</param>
    /// <returns>True when value is less than other.</returns>
    public static bool IsLessThan(decimal? value, decimal other) => value < other;

    /// <summary>Determines whether a decimal value is less than or equal to another.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="other">The inclusive upper bound.</param>
    /// <returns>True when value is less than or equal to other.</returns>
    public static bool IsLessThanOrEqual(decimal? value, decimal other) => value <= other;

    /// <summary>Determines whether a decimal value falls within a range.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="min">The lower bound.</param>
    /// <param name="max">The upper bound.</param>
    /// <param name="inclusion">Whether both endpoints are included.</param>
    /// <returns>True when the value belongs to the range; reversed bounds return false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The inclusion value is undefined.</exception>
    public static bool IsInRange(decimal? value, decimal min, decimal max, Inclusion inclusion = Inclusion.Inclusive)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            throw new ArgumentOutOfRangeException(nameof(inclusion), inclusion, null);

        return value is not null && RuleComparison.IsInRange(value.Value, min, max, inclusion);
    }

    /// <summary>Determines whether a decimal value is a percentage on the inclusive 0–100 scale.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <returns>True when the value is between the percentage bounds.</returns>
    public static bool IsPercentage(decimal? value) => IsInRange(value, MinPercentage, MaxPercentage);

    /// <summary>Determines whether a decimal value lies within an absolute tolerance of a target.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="target">The target value.</param>
    /// <param name="tolerance">The maximum absolute difference; null or negative returns false.</param>
    /// <returns>True when the distance is within tolerance; an unrepresentable distance returns false.</returns>
    public static bool IsApproximately(decimal? value, decimal target, decimal? tolerance)
    {
        if (value is null || tolerance is null || tolerance < 0)
            return false;

        try
        {
            var difference = value.Value >= target ? value.Value - target : target - value.Value;
            return difference <= tolerance.Value;
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    /// <summary>Determines whether a decimal value is an exact multiple of a factor.</summary>
    /// <param name="value">The value to validate; null returns false.</param>
    /// <param name="factor">The divisor; zero returns false.</param>
    /// <returns>True when division has no remainder.</returns>
    public static bool IsMultipleOf(decimal? value, decimal factor) =>
        value is not null && factor != 0 && value.Value % factor == 0;

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
