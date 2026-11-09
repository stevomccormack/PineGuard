using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate time-span string representations,
/// parsing the input string before delegating to time-span rules.
/// </summary>
/// <seealso cref="TimeSpanRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
public static class MustStringTimeSpanClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be a duration within the expected range.
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
    /// The failure message follows the pattern <c>"{paramName} must be a duration within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> InRangeTimeSpan(this IMustClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, "{paramName} requires a valid range.", nameof(min), min);

        if (value is null)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.OutOfRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration within the expected range.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed;
        var ok = TimeSpanRules.IsInRange(parsedValue, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.OutOfRange, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a duration longer than the other.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a duration longer than the other."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> LongerThanTimeSpan(this IMustClause _,
        string? value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotLonger, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (value is null)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotLonger, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration longer than the other.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed;
        var ok = TimeSpanRules.IsLongerThan(parsedValue, other, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotLonger, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a duration shorter than the other.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="other">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a duration shorter than the other."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> ShorterThanTimeSpan(this IMustClause _,
        string? value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotShorter, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (value is null)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.NotShorter, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration shorter than the other.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed;
        var ok = TimeSpanRules.IsShorterThan(parsedValue, other, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.NotShorter, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a duration not within the expected range.
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
    /// The failure message follows the pattern <c>"{paramName} must be a duration not within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> OutOfRangeTimeSpan(this IMustClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (inclusion is not Inclusion.Inclusive and not Inclusion.Exclusive)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (min > max)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, "{paramName} requires a valid range.", nameof(min), min);

        if (value is null)
            return MustResult<TimeSpan>.Fail(MustCodes.Time.Duration.InRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration not within the expected range.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, MustCodes.Time.Format.Invalid, messageTemplate, paramName, value, result: default);

        var parsedValue = parsed;
        var ok = !TimeSpanRules.IsInRange(parsedValue, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, MustCodes.Time.Duration.InRange, messageTemplate, paramName, value, parsedValue);
    }
}
