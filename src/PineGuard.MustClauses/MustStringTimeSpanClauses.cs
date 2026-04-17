using System.Runtime.CompilerServices;
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
    public static MustResult<TimeSpan> DurationBetween(this IMustClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeSpan>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration within the expected range.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeSpanRules.IsDurationBetween(parsedValue, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a duration greater than the threshold.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a duration greater than the threshold."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> GreaterThan(this IMustClause _,
        string? value,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeSpan>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration greater than the threshold.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeSpanRules.IsGreaterThan(parsedValue, threshold, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, messageTemplate, paramName, value, parsedValue);
    }

    /// <summary>
    /// Validates that the specified value must be a duration less than the threshold.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="threshold">The value to validate.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a duration less than the threshold."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-time-span">String Time Span Must Clauses documentation</seealso>
    public static MustResult<TimeSpan> LessThan(this IMustClause _,
        string? value,
        TimeSpan threshold,
        Inclusion inclusion = Inclusion.Exclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeSpan>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration less than the threshold.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = TimeSpanRules.IsLessThan(parsedValue, threshold, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, messageTemplate, paramName, value, parsedValue);
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
    public static MustResult<TimeSpan> NotDurationBetween(this IMustClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<TimeSpan>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a duration not within the expected range.";

        if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
            return MustResult<TimeSpan>.FromBool(false, messageTemplate, paramName, value);

        var parsedValue = parsed.GetValueOrDefault();
        var ok = !TimeSpanRules.IsDurationBetween(parsedValue, min, max, inclusion);
        return MustResult<TimeSpan>.FromBool(ok, messageTemplate, paramName, value, parsedValue);
    }
}
