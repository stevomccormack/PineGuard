using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate how many grapheme clusters — the
/// characters a reader sees — a string holds, delegating to <see cref="StringRules.Graphemes"/> for core
/// validation logic.
/// </summary>
/// <remarks>
/// <see cref="string.Length"/> counts UTF-16 code units, so a family emoji reads as eleven characters and an
/// accented letter written with a combining mark reads as two. These clauses count what a length limit shown
/// to a user is actually promising. Segmentation follows the host runtime's Unicode tables.
/// </remarks>
/// <seealso cref="StringRules.Graphemes"/>
/// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
public static class MustStringGraphemesClauses
{
    private const string NullMessage = "{paramName} must not be null.";
    private const string NonNegativeCountMessage = "{paramName} requires a non-negative count.";
    private const string NonNegativeMinMessage = "{paramName} requires a non-negative minimum count.";
    private const string NonNegativeMaxMessage = "{paramName} requires a non-negative maximum count.";
    private const string ValidRangeMessage = "{paramName} requires a valid count range.";

    /// <summary>
    /// Validates that the specified string holds exactly the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="count">The required number of characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> holds exactly <paramref name="count"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="count"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasExactCount"/>.
    /// The failure message follows the pattern <c>"{paramName} must have the expected number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> HasExactGraphemeCount(this IMustClause _,
        string? value,
        int count,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.Mismatch, NullMessage, paramName, value);

        if (count < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.Mismatch, NonNegativeCountMessage, nameof(count), count);

        const string messageTemplate = "{paramName} must have the expected number of characters.";

        var ok = StringRules.Graphemes.HasExactCount(value, count);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.Mismatch, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not hold exactly the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="count">The number of characters that must not match.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not hold exactly <paramref name="count"/> characters, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="count"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasExactCount"/>.
    /// The failure message follows the pattern <c>"{paramName} must not have the expected number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> NotHasExactGraphemeCount(this IMustClause _,
        string? value,
        int count,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.Match, NullMessage, paramName, value);

        if (count < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.Match, NonNegativeCountMessage, nameof(count), count);

        const string messageTemplate = "{paramName} must not have the expected number of characters.";

        var ok = !StringRules.Graphemes.HasExactCount(value, count);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.Match, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string holds at least the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="min">The minimum required number of characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> holds at least <paramref name="min"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="min"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasMinCount"/>.
    /// The failure message follows the pattern <c>"{paramName} must have at least the minimum number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> HasMinGraphemeCount(this IMustClause _,
        string? value,
        int min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooFew, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooFew, NonNegativeMinMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must have at least the minimum number of characters.";

        var ok = StringRules.Graphemes.HasMinCount(value, min);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.TooFew, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not hold at least the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="min">The minimum number of characters that must not be reached.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> holds fewer than <paramref name="min"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="min"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasMinCount"/>.
    /// The failure message follows the pattern <c>"{paramName} must not have at least the minimum number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> NotHasMinGraphemeCount(this IMustClause _,
        string? value,
        int min,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooMany, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooMany, NonNegativeMinMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must not have at least the minimum number of characters.";

        var ok = !StringRules.Graphemes.HasMinCount(value, min);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.TooMany, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string holds at most the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="max">The maximum allowed number of characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> holds at most <paramref name="max"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="max"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasMaxCount"/>. This is the clause a "your name is too long"
    /// limit wants: a family emoji costs eleven code units and one character.
    /// The failure message follows the pattern <c>"{paramName} must have at most the maximum number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> HasMaxGraphemeCount(this IMustClause _,
        string? value,
        int max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooMany, NullMessage, paramName, value);

        if (max < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooMany, NonNegativeMaxMessage, nameof(max), max);

        const string messageTemplate = "{paramName} must have at most the maximum number of characters.";

        var ok = StringRules.Graphemes.HasMaxCount(value, max);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.TooMany, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not hold at most the given number of grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="max">The maximum number of characters that must be exceeded.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> holds more than <paramref name="max"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="max"/> if it is negative.
    /// Delegates to <see cref="StringRules.Graphemes.HasMaxCount"/>.
    /// The failure message follows the pattern <c>"{paramName} must not have at most the maximum number of characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> NotHasMaxGraphemeCount(this IMustClause _,
        string? value,
        int max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooFew, NullMessage, paramName, value);

        if (max < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.TooFew, NonNegativeMaxMessage, nameof(max), max);

        const string messageTemplate = "{paramName} must not have at most the maximum number of characters.";

        var ok = !StringRules.Graphemes.HasMaxCount(value, max);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.TooFew, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the number of grapheme clusters in the specified string falls within the given range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="min">The lower bound of the acceptable number of characters.</param>
    /// <param name="max">The upper bound of the acceptable number of characters.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if the number of characters in <paramref name="value"/> is within the range, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="min"/> or <paramref name="max"/> if either is negative or the range is inverted.
    /// Delegates to <see cref="StringRules.Graphemes.HasCountBetween"/>.
    /// The failure message follows the pattern <c>"{paramName} must have a number of characters within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> HasGraphemeCountBetween(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.OutOfRange, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.OutOfRange, NonNegativeMinMessage, nameof(min), min);

        if (max < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.OutOfRange, NonNegativeMaxMessage, nameof(max), max);

        if (min > max)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.OutOfRange, ValidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must have a number of characters within the expected range.";

        var ok = StringRules.Graphemes.HasCountBetween(value, min, max, inclusion);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.OutOfRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the number of grapheme clusters in the specified string falls outside the given range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="min">The lower bound of the forbidden number of characters.</param>
    /// <param name="max">The upper bound of the forbidden number of characters.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive. Defaults to <see cref="Inclusion.Inclusive"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if the number of characters in <paramref name="value"/> is outside the range, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>, or attributed to
    /// <paramref name="min"/> or <paramref name="max"/> if either is negative or the range is inverted.
    /// Delegates to <see cref="StringRules.Graphemes.HasCountBetween"/>.
    /// The failure message follows the pattern <c>"{paramName} must not have a number of characters within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-graphemes">String Graphemes Must Clauses documentation</seealso>
    public static MustResult<string> NotHasGraphemeCountBetween(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.InRange, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.InRange, NonNegativeMinMessage, nameof(min), min);

        if (max < 0)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.InRange, NonNegativeMaxMessage, nameof(max), max);

        if (min > max)
            return MustResult<string>.Fail(MustCodes.Text.Graphemes.InRange, ValidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must not have a number of characters within the expected range.";

        var ok = !StringRules.Graphemes.HasCountBetween(value, min, max, inclusion);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Graphemes.InRange, messageTemplate, paramName, value, value);
    }
}
