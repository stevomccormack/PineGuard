using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate string values,
/// delegating to <see cref="StringRules"/> for core validation logic.
/// </summary>
/// <seealso cref="StringRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
public static class MustStringClauses
{
    private const string NullMessage = "{paramName} must not be null.";
    private const string NonNegativeLengthMessage = "{paramName} requires a non-negative length.";

    /// <summary>
    /// Validates that the specified string is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <see langword="null"/> or empty, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="string.IsNullOrEmpty"/>.
    /// The failure message follows the pattern <c>"{paramName} must be null or empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NullOrEmpty(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be null or empty.";
        var ok = string.IsNullOrEmpty(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotNullOrEmpty, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string is not <see langword="null"/> or empty.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <see langword="null"/> or empty, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="string.IsNullOrEmpty"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be null or empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotNullOrEmpty(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be null or empty.";
        var ok = !string.IsNullOrEmpty(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NullOrEmpty, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string is <see langword="null"/> or whitespace.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is <see langword="null"/> or whitespace, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="string.IsNullOrWhiteSpace"/>.
    /// The failure message follows the pattern <c>"{paramName} must be null or whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NullOrWhiteSpace(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be null or whitespace.";
        var ok = string.IsNullOrWhiteSpace(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotBlank, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string is not <see langword="null"/> or whitespace.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <see langword="null"/> or whitespace, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="string.IsNullOrWhiteSpace"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be null or whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotNullOrWhiteSpace(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be null or whitespace.";
        var ok = !string.IsNullOrWhiteSpace(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.Blank, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string is empty (equal to <see cref="string.Empty"/>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is empty, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Empty(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be empty.";
        // Note: Empty implies not null? Or just value == ""?
        // Usually Empty means not null and length 0.
        // If value is null, it's NOT empty string.
        var ok = value == string.Empty;
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotEmpty, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string is not empty (not equal to <see cref="string.Empty"/>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not empty, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be empty."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotEmpty(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be empty.";
        var ok = value != string.Empty;
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.Empty, messageTemplate, paramName, value, value ?? string.Empty);
    }

    /// <summary>
    /// Validates that the specified string has exactly the expected length.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="length">The exact length required.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> has the specified <paramref name="length"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsExactLength"/>.
    /// The failure message follows the pattern <c>"{paramName} must be the expected length."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ExactLength(this IMustClause _,
        string? value,
        int length,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.Mismatch, NullMessage, paramName, value);

        if (length < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.Mismatch, NonNegativeLengthMessage, nameof(length), length);

        const string messageTemplate = "{paramName} must be the expected length.";

        var ok = StringRules.IsExactLength(value, length);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.Mismatch, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string has a length within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="min">The minimum length (inclusive).</param>
    /// <param name="max">The maximum length (inclusive).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if the length of <paramref name="value"/> is between <paramref name="min"/> and <paramref name="max"/>,
    /// or <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLengthBetween"/>.
    /// The failure message follows the pattern <c>"{paramName} must have a length within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> LengthBetween(this IMustClause _,
        string? value,
        int min,
        int max,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.OutOfRange, NullMessage, paramName, value);

        if (min < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.OutOfRange, "{paramName} requires a non-negative min.", nameof(min), min);

        if (max < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.OutOfRange, "{paramName} requires a non-negative max.", nameof(max), max);

        if (min > max)
            return MustResult<string>.Fail(MustCodes.Text.Length.OutOfRange, "{paramName} requires a valid length range.", nameof(min), min);

        const string messageTemplate = "{paramName} must have a length within the expected range.";

        var ok = StringRules.IsLengthBetween(value, min, max);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.OutOfRange, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is longer than the specified length.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="length">The minimum length (exclusive).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is strictly longer than <paramref name="length"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLongerThan"/> with <see cref="Inclusion.Exclusive"/>.
    /// The failure message follows the pattern <c>"{paramName} must be longer than the specified length."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> LongerThan(this IMustClause _,
        string? value,
        int length,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooShort, NullMessage, paramName, value);

        if (length < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooShort, NonNegativeLengthMessage, nameof(length), length);

        const string messageTemplate = "{paramName} must be longer than the specified length.";

        var ok = StringRules.IsLongerThan(value, length, Inclusion.Exclusive);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.TooShort, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="length">The minimum length (inclusive).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is at least <paramref name="length"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLongerThan"/> with <see cref="Inclusion.Inclusive"/>.
    /// The failure message follows the pattern <c>"{paramName} must be longer than or equal to the specified length."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> LongerThanOrEqual(this IMustClause _,
        string? value,
        int length,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooShort, NullMessage, paramName, value);

        if (length < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooShort, NonNegativeLengthMessage, nameof(length), length);

        const string messageTemplate = "{paramName} must be longer than or equal to the specified length.";

        var ok = StringRules.IsLongerThan(value, length, Inclusion.Inclusive);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.TooShort, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is shorter than the specified length.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="length">The maximum length (exclusive).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is strictly shorter than <paramref name="length"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsShorterThan"/> with <see cref="Inclusion.Exclusive"/>.
    /// The failure message follows the pattern <c>"{paramName} must be shorter than the specified length."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ShorterThan(this IMustClause _,
        string? value,
        int length,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooLong, NullMessage, paramName, value);

        if (length < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooLong, NonNegativeLengthMessage, nameof(length), length);

        const string messageTemplate = "{paramName} must be shorter than the specified length.";

        var ok = StringRules.IsShorterThan(value, length, Inclusion.Exclusive);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.TooLong, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="length">The maximum length (inclusive).</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is at most <paramref name="length"/> characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsShorterThan"/> with <see cref="Inclusion.Inclusive"/>.
    /// The failure message follows the pattern <c>"{paramName} must be shorter than or equal to the specified length."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ShorterThanOrEqual(this IMustClause _,
        string? value,
        int length,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooLong, NullMessage, paramName, value);

        if (length < 0)
            return MustResult<string>.Fail(MustCodes.Text.Length.TooLong, NonNegativeLengthMessage, nameof(length), length);

        const string messageTemplate = "{paramName} must be shorter than or equal to the specified length.";

        var ok = StringRules.IsShorterThan(value, length, Inclusion.Inclusive);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Length.TooLong, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string matches the given regular expression pattern.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="pattern">The <see cref="Regex"/> pattern to match against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> matches <paramref name="pattern"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="pattern"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsMatch"/>.
    /// The failure message follows the pattern <c>"{paramName} must match the specified pattern."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Match(this IMustClause _,
        string? value,
        Regex pattern,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Pattern.NoMatch, NullMessage, paramName, value);

        if (pattern is null)
            return MustResult<string>.Fail(MustCodes.Text.Pattern.NoMatch, NullMessage, nameof(pattern), pattern);

        const string messageTemplate = "{paramName} must match the specified pattern.";

        var ok = StringRules.IsMatch(value, pattern);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Pattern.NoMatch, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not match the given regular expression pattern.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="pattern">The <see cref="Regex"/> pattern that must not match.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not match <paramref name="pattern"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="pattern"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsMatch"/>.
    /// The failure message follows the pattern <c>"{paramName} must not match the specified pattern."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotMatch(this IMustClause _,
        string? value,
        Regex pattern,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Pattern.Match, NullMessage, paramName, value);

        if (pattern is null)
            return MustResult<string>.Fail(MustCodes.Text.Pattern.Match, NullMessage, nameof(pattern), pattern);

        const string messageTemplate = "{paramName} must not match the specified pattern.";

        var ok = !StringRules.IsMatch(value, pattern);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Pattern.Match, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only alphabetic characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is alphabetic, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAlphabetic"/>.
    /// The failure message follows the pattern <c>"{paramName} must be alphabetic."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Alphabetic(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotAlpha, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be alphabetic.";

        var ok = StringRules.IsAlphabetic(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotAlpha, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only alphabetic characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not alphabetic, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAlphabetic"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be alphabetic."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotAlphabetic(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Alpha, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be alphabetic.";

        var ok = !StringRules.IsAlphabetic(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Alpha, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only numeric characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is numeric, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsNumeric"/>.
    /// The failure message follows the pattern <c>"{paramName} must be numeric."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Numeric(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotNumeric, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be numeric.";

        var ok = StringRules.IsNumeric(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotNumeric, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only numeric characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not numeric, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsNumeric"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be numeric."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotNumeric(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Numeric, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be numeric.";

        var ok = !StringRules.IsNumeric(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Numeric, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only alphanumeric characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is alphanumeric, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAlphanumeric"/>.
    /// The failure message follows the pattern <c>"{paramName} must be alphanumeric."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Alphanumeric(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotAlphanumeric, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be alphanumeric.";

        var ok = StringRules.IsAlphanumeric(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotAlphanumeric, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only alphanumeric characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="inclusions">Optional additional characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not alphanumeric, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAlphanumeric"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be alphanumeric."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotAlphanumeric(this IMustClause _,
        string? value,
        char[]? inclusions = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Alphanumeric, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be alphanumeric.";

        var ok = !StringRules.IsAlphanumeric(value, inclusions);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Alphanumeric, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only digit characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains only digits, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDigitsOnly(string)"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain digits only."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> DigitsOnly(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotDigits, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain digits only.";

        var ok = StringRules.IsDigitsOnly(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotDigits, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only digit characters, with optional allowed non-digit characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowedNonDigitChars">Optional array of non-digit characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains only digits and allowed characters, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDigitsOnly(string, char[])"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain digits only (except for allowed characters)."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> DigitsOnly(this IMustClause _,
        string? value,
        char[]? allowedNonDigitChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotDigits, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain digits only (except for allowed characters).";

        var ok = StringRules.IsDigitsOnly(value, allowedNonDigitChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotDigits, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only digit characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain only digits, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDigitsOnly(string)"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain digits only."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotDigitsOnly(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Digits, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain digits only.";

        var ok = !StringRules.IsDigitsOnly(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Digits, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only digit characters, considering allowed non-digit characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowedNonDigitChars">Optional array of non-digit characters to allow.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain only digits and allowed characters, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsDigitsOnly(string, char[])"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain digits only (considering allowed characters)."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotDigitsOnly(this IMustClause _,
        string? value,
        char[]? allowedNonDigitChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Digits, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain digits only (considering allowed characters).";

        var ok = !StringRules.IsDigitsOnly(value, allowedNonDigitChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Digits, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is entirely uppercase.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="lettersOnly">If <see langword="true"/>, only letter characters are considered.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is uppercase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUppercase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be uppercase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Uppercase(this IMustClause _,
        string? value,
        bool lettersOnly = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotUpper, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be uppercase.";

        var ok = StringRules.IsUppercase(value, lettersOnly);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotUpper, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not entirely uppercase.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="lettersOnly">If <see langword="true"/>, only letter characters are considered.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not uppercase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsUppercase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be uppercase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotUppercase(this IMustClause _,
        string? value,
        bool lettersOnly = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Upper, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be uppercase.";

        var ok = !StringRules.IsUppercase(value, lettersOnly);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Upper, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is entirely lowercase.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="lettersOnly">If <see langword="true"/>, only letter characters are considered.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is lowercase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLowercase"/>.
    /// The failure message follows the pattern <c>"{paramName} must be lowercase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Lowercase(this IMustClause _,
        string? value,
        bool lettersOnly = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.NotLower, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be lowercase.";

        var ok = StringRules.IsLowercase(value, lettersOnly);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.NotLower, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not entirely lowercase.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="lettersOnly">If <see langword="true"/>, only letter characters are considered.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not lowercase, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsLowercase"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be lowercase."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotLowercase(this IMustClause _,
        string? value,
        bool lettersOnly = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Casing.Lower, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be lowercase.";

        var ok = !StringRules.IsLowercase(value, lettersOnly);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Casing.Lower, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only ASCII characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is ASCII, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAscii"/>.
    /// The failure message follows the pattern <c>"{paramName} must be ASCII."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Ascii(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotAscii, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be ASCII.";

        var ok = StringRules.IsAscii(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotAscii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains non-ASCII characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not ASCII, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsAscii"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be ASCII."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotAscii(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Ascii, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be ASCII.";

        var ok = !StringRules.IsAscii(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Ascii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only printable ASCII characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowCommonWhitespace">If <see langword="true"/>, common whitespace characters (tab, newline) are allowed.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is printable ASCII, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsPrintableAscii"/>.
    /// The failure message follows the pattern <c>"{paramName} must be printable ASCII."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> PrintableAscii(this IMustClause _,
        string? value,
        bool allowCommonWhitespace = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotPrintable, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be printable ASCII.";

        var ok = StringRules.IsPrintableAscii(value, allowCommonWhitespace);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotPrintable, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains non-printable ASCII characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowCommonWhitespace">If <see langword="true"/>, common whitespace characters (tab, newline) are allowed.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not printable ASCII, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsPrintableAscii"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be printable ASCII."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotPrintableAscii(this IMustClause _,
        string? value,
        bool allowCommonWhitespace = false,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Printable, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be printable ASCII.";

        var ok = !StringRules.IsPrintableAscii(value, allowCommonWhitespace);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Printable, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is not entirely whitespace.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not whitespace, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.IsWhitespace"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotWhitespace(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.Whitespace, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be whitespace.";

        var ok = !StringRules.IsWhitespace(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.Whitespace, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains whitespace characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains whitespace, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsWhitespace"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ContainsWhitespace(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsWhitespace, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain whitespace.";

        var ok = StringRules.ContainsWhitespace(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotContainsWhitespace, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain whitespace characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain whitespace, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsWhitespace"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotContainsWhitespace(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.ContainsWhitespace, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain whitespace.";

        var ok = !StringRules.ContainsWhitespace(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.ContainsWhitespace, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains control characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains control characters, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsControlChars"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain control characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ContainsControlChars(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsControl, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain control characters.";

        var ok = StringRules.ContainsControlChars(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotContainsControl, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain control characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain control characters, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.NotContainsControlChars"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain control characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotContainsControlChars(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.ContainsControl, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain control characters.";

        var ok = StringRules.NotContainsControlChars(value);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.ContainsControl, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains only the allowed characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowedChars">The set of allowed characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains only allowed characters, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="allowedChars"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsAllowedOnly"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain only allowed characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ContainsAllowedOnly(this IMustClause _,
        string? value,
        char[] allowedChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotSubset, NullMessage, paramName, value);

        if (allowedChars is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotSubset, NullMessage, nameof(allowedChars), allowedChars);

        const string messageTemplate = "{paramName} must contain only allowed characters.";

        var ok = StringRules.ContainsAllowedOnly(value, allowedChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotSubset, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains at least one disallowed character.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="disallowedChars">The set of disallowed characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains a disallowed character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="disallowedChars"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsDisallowed"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain a disallowed character."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ContainsDisallowed(this IMustClause _,
        string? value,
        char[] disallowedChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsDisallowed, NullMessage, paramName, value);

        if (disallowedChars is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsDisallowed, NullMessage, nameof(disallowedChars), disallowedChars);

        const string messageTemplate = "{paramName} must contain a disallowed character.";

        var ok = StringRules.ContainsDisallowed(value, disallowedChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotContainsDisallowed, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain any disallowed characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="disallowedChars">The set of disallowed characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain any disallowed characters, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="disallowedChars"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsDisallowed"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain any disallowed characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotContainsDisallowed(this IMustClause _,
        string? value,
        char[] disallowedChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.ContainsDisallowed, NullMessage, paramName, value);

        if (disallowedChars is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.ContainsDisallowed, NullMessage, nameof(disallowedChars), disallowedChars);

        const string messageTemplate = "{paramName} must not contain any disallowed characters.";

        var ok = !StringRules.ContainsDisallowed(value, disallowedChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.ContainsDisallowed, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain only the allowed characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="allowedChars">The set of allowed characters.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains at least one character outside the allowed set, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="allowedChars"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsAllowedOnly"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain only allowed characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotContainsAllowedOnly(this IMustClause _,
        string? value,
        char[] allowedChars,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Subset, NullMessage, paramName, value);

        if (allowedChars is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.Subset, NullMessage, nameof(allowedChars), allowedChars);

        const string messageTemplate = "{paramName} must not contain only allowed characters.";

        // Not "ContainsAllowedOnly" means it must contain at least one disallowed char.
        var ok = !StringRules.ContainsAllowedOnly(value, allowedChars);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.Subset, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains at least one of the expected characters.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="characters">The set of characters to search for.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains at least one of <paramref name="characters"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="characters"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.ContainsDisallowed"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain at least one of the expected characters."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> ContainsAny(this IMustClause _,
        string? value,
        char[] characters,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsAny, NullMessage, paramName, value);

        if (characters is null)
            return MustResult<string>.Fail(MustCodes.Text.Charset.NotContainsAny, NullMessage, nameof(characters), characters);

        const string messageTemplate = "{paramName} must contain at least one of the expected characters.";

        var ok = StringRules.ContainsDisallowed(value, characters);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Charset.NotContainsAny, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string contains the given substring.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="substring">The substring to search for. An empty substring is always contained.</param>
    /// <param name="comparison">The comparison rule used to locate <paramref name="substring"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains <paramref name="substring"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="substring"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.Contains"/>.
    /// The failure message follows the pattern <c>"{paramName} must contain the specified substring."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> Contains(this IMustClause _,
        string? value,
        string substring,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotContains, NullMessage, paramName, value);

        if (substring is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotContains, NullMessage, nameof(substring), substring);

        const string messageTemplate = "{paramName} must contain the specified substring.";

        var ok = StringRules.Contains(value, substring, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotContains, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not contain the given substring.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="substring">The substring that must be absent. An empty substring is always contained.</param>
    /// <param name="comparison">The comparison rule used to locate <paramref name="substring"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain <paramref name="substring"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="substring"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.Contains"/>.
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified substring."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotContains(this IMustClause _,
        string? value,
        string substring,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.Contains, NullMessage, paramName, value);

        if (substring is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.Contains, NullMessage, nameof(substring), substring);

        const string messageTemplate = "{paramName} must not contain the specified substring.";

        var ok = !StringRules.Contains(value, substring, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.Contains, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string starts with the given prefix.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="prefix">The prefix to test for. An empty prefix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="prefix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> starts with <paramref name="prefix"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="prefix"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.StartsWith"/>.
    /// The failure message follows the pattern <c>"{paramName} must start with the specified prefix."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> StartsWith(this IMustClause _,
        string? value,
        string prefix,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotStartsWith, NullMessage, paramName, value);

        if (prefix is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotStartsWith, NullMessage, nameof(prefix), prefix);

        const string messageTemplate = "{paramName} must start with the specified prefix.";

        var ok = StringRules.StartsWith(value, prefix, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotStartsWith, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not start with the given prefix.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="prefix">The prefix that must be absent. An empty prefix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="prefix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not start with <paramref name="prefix"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="prefix"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.StartsWith"/>.
    /// The failure message follows the pattern <c>"{paramName} must not start with the specified prefix."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotStartsWith(this IMustClause _,
        string? value,
        string prefix,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.StartsWith, NullMessage, paramName, value);

        if (prefix is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.StartsWith, NullMessage, nameof(prefix), prefix);

        const string messageTemplate = "{paramName} must not start with the specified prefix.";

        var ok = !StringRules.StartsWith(value, prefix, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.StartsWith, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string ends with the given suffix.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="suffix">The suffix to test for. An empty suffix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="suffix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> ends with <paramref name="suffix"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="suffix"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.EndsWith"/>.
    /// The failure message follows the pattern <c>"{paramName} must end with the specified suffix."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> EndsWith(this IMustClause _,
        string? value,
        string suffix,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotEndsWith, NullMessage, paramName, value);

        if (suffix is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.NotEndsWith, NullMessage, nameof(suffix), suffix);

        const string messageTemplate = "{paramName} must end with the specified suffix.";

        var ok = StringRules.EndsWith(value, suffix, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.NotEndsWith, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string does not end with the given suffix.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="suffix">The suffix that must be absent. An empty suffix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="suffix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not end with <paramref name="suffix"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="suffix"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringRules.EndsWith"/>.
    /// The failure message follows the pattern <c>"{paramName} must not end with the specified suffix."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string">String Must Clauses documentation</seealso>
    public static MustResult<string> NotEndsWith(this IMustClause _,
        string? value,
        string suffix,
        StringComparison comparison = StringComparison.Ordinal,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.EndsWith, NullMessage, paramName, value);

        if (suffix is null)
            return MustResult<string>.Fail(MustCodes.Text.Content.EndsWith, NullMessage, nameof(suffix), suffix);

        const string messageTemplate = "{paramName} must not end with the specified suffix.";

        var ok = !StringRules.EndsWith(value, suffix, comparison);
        return MustResult<string>.FromBool(ok, MustCodes.Text.Content.EndsWith, messageTemplate, paramName, value, value);
    }
}
