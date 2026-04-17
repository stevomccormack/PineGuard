#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate numeric string representations,
/// parsing the input string before delegating to number rules.
/// </summary>
/// <seealso cref="NumberRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
public static class MustStringNumbersClauses
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    private const string MinToken = "{Min}";
    private const string MaxToken = "{Max}";
    private const string TargetToken = "{Target}";
    private const string FactorToken = "{Factor}";

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> Positive(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be positive.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsPositive<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> Negative(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be negative.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsNegative<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> Zero(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be zero.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsZero<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> ZeroOrPositive(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be zero or positive.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsZeroOrPositive<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> ZeroOrNegative(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be zero or negative.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsZeroOrNegative<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> GreaterThan(this IMustClause _,
        string? value,
        decimal min,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be greater than '{Min}'.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsGreaterThan(parsed, min);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> GreaterThanOrEqual(this IMustClause _,
        string? value,
        decimal min,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be greater than or equal to '{Min}'.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsGreaterThanOrEqual(parsed, min);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> LessThan(this IMustClause _,
        string? value,
        decimal max,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be less than '{Max}'.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsLessThan(parsed, max);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> LessThanOrEqual(this IMustClause _,
        string? value,
        decimal max,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be less than or equal to '{Max}'.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsLessThanOrEqual(parsed, max);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> InRange(this IMustClause _,
        string? value,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (min > max)
            return MustResult<decimal>.Fail("{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must be within the expected range.";



        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> OutOfRange(this IMustClause _,
        string? value,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (min > max)
            return MustResult<decimal>.Fail("{paramName} requires a valid range.", nameof(min), min);

        const string messageTemplate = "{paramName} must not be between '{Min}' and '{Max}'.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)).Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = !NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(MinToken, min.ToString(CultureInfo.InvariantCulture)).Replace(MaxToken, max.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="target">The target value to compare against.</param>
    /// <param name="tolerance">The tolerance for approximate comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> Approximately(this IMustClause _,
        string? value,
        decimal target,
        decimal? tolerance,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (tolerance is null)
            return MustResult<decimal>.Fail("{paramName} requires a non-null tolerance.", nameof(tolerance), tolerance);

        const string messageTemplate = "{paramName} must be approximately '{Target}'.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(TargetToken, target.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsApproximately(parsed, target, tolerance);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(TargetToken, target.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> MultipleOf(this IMustClause _,
        string? value,
        decimal factor,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (factor == 0)
            return MustResult<decimal>.Fail("{paramName} requires a non-zero factor.", nameof(factor), factor);

        const string messageTemplate = "{paramName} must be a multiple of '{Factor}'.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(FactorToken, factor.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = NumberRules.IsMultipleOf(parsed, factor);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(FactorToken, factor.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<int> Even(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<int>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be even.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles))
            return MustResult<int>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsEven(parsed);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<int> Odd(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<int>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be odd.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles))
            return MustResult<int>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsOdd(parsed);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<double> Finite(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<double>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be finite.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed, styles))
            return MustResult<double>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsFinite(parsed);
        return MustResult<double>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> NotZero(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must not be zero.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate, paramName, value);

        var ok = NumberRules.IsNotZero<decimal>(parsed);
        return MustResult<decimal>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="target">The target value to compare against.</param>
    /// <param name="tolerance">The tolerance for approximate comparison.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> NotApproximately(this IMustClause _,
        string? value,
        decimal target,
        decimal? tolerance,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (tolerance is null)
            return MustResult<decimal>.Fail("{paramName} requires a non-null tolerance.", nameof(tolerance), tolerance);

        const string messageTemplate = "{paramName} must not be approximately '{Target}'.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(TargetToken, target.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = !NumberRules.IsApproximately(parsed, target, tolerance);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(TargetToken, target.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<decimal> NotMultipleOf(this IMustClause _,
        string? value,
        decimal factor,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail("{paramName} must not be null.", paramName, value);

        if (factor == 0)
            return MustResult<decimal>.Fail("{paramName} requires a non-zero factor.", nameof(factor), factor);

        const string messageTemplate = "{paramName} must not be a multiple of '{Factor}'.";

        if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed, styles))
            return MustResult<decimal>.FromBool(false, messageTemplate.Replace(FactorToken, factor.ToString(CultureInfo.InvariantCulture)), paramName, value);

        var ok = !NumberRules.IsMultipleOf(parsed, factor);
        return MustResult<decimal>.FromBool(ok, messageTemplate.Replace(FactorToken, factor.ToString(CultureInfo.InvariantCulture)), paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<double> NotFinite(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<double>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must not be finite.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed, styles))
            return MustResult<double>.FromBool(false, messageTemplate, paramName, value);

        var ok = !NumberRules.IsFinite(parsed);
        return MustResult<double>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must not be null.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-numbers">String Numbers Must Clauses documentation</seealso>
    public static MustResult<double> NotNaN(this IMustClause _,
        string? value,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<double>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must not be NaN.";

        if (!StringUtility.NumberTypes.TryParseDouble(value, out var parsed, styles))
            return MustResult<double>.FromBool(false, messageTemplate, paramName, value);

        var ok = !NumberRules.IsNaN(parsed);
        return MustResult<double>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }
}
#endif
