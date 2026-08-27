using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate individual <see cref="char"/> values.
/// </summary>
/// <seealso cref="CharRules"/>
/// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
public static class MustCharClauses
{
    /// <summary>
    /// Validates that the specified character is a Unicode letter.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a letter, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsLetter"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a letter."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Letter(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsLetter"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Letter(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a letter.";
        var ok = CharRules.IsLetter(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotLetter, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a Unicode letter.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a letter, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsLetter"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a letter."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotLetter(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsLetter"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotLetter(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a letter.";
        var ok = !CharRules.IsLetter(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.Letter, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a decimal digit (0–9).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Digit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Digit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a digit.";
        var ok = CharRules.IsDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotDigit, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a decimal digit (0–9).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotDigit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotDigit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a digit.";
        var ok = !CharRules.IsDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.Digit, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a Unicode letter or decimal digit.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a letter or digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsLetterOrDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a letter or digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.LetterOrDigit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsLetterOrDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> LetterOrDigit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a letter or digit.";
        var ok = CharRules.IsLetterOrDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotLetterOrDigit, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a Unicode letter or decimal digit.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a letter or digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsLetterOrDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a letter or digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotLetterOrDigit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsLetterOrDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotLetterOrDigit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a letter or digit.";
        var ok = !CharRules.IsLetterOrDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.LetterOrDigit, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character falls within the ASCII range (U+0000–U+007F).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is an ASCII character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsAscii"/>. The failure message follows the pattern
    /// <c>"{paramName} must be an ASCII character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Ascii(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsAscii"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Ascii(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be an ASCII character.";
        var ok = CharRules.IsAscii(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotAscii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character does not fall within the ASCII range (U+0000–U+007F).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not an ASCII character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsAscii"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be an ASCII character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotAscii(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsAscii"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotAscii(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be an ASCII character.";
        var ok = !CharRules.IsAscii(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.Ascii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a printable ASCII character (U+0020–U+007E).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a printable ASCII character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsPrintableAscii"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a printable ASCII character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.PrintableAscii(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsPrintableAscii"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> PrintableAscii(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a printable ASCII character.";
        var ok = CharRules.IsPrintableAscii(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotPrintableAscii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a printable ASCII character (U+0020–U+007E).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a printable ASCII character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsPrintableAscii"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a printable ASCII character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotPrintableAscii(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsPrintableAscii"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotPrintableAscii(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a printable ASCII character.";
        var ok = !CharRules.IsPrintableAscii(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.PrintableAscii, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a whitespace character.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
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
    /// Delegates to <see cref="CharRules.IsWhitespace"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be whitespace."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotWhitespace(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsWhitespace"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotWhitespace(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be whitespace.";
        var ok = !CharRules.IsWhitespace(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Category.Whitespace, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a control character (Unicode category Cc).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a control character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsControl"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a control character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Control(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsControl"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Control(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a control character.";
        var ok = CharRules.IsControl(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Category.NotControl, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a control character (Unicode category Cc).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a control character, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsControl"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a control character."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotControl(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsControl"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotControl(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a control character.";
        var ok = !CharRules.IsControl(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Category.Control, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is an uppercase Unicode letter.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is an uppercase letter, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsUppercase"/>. The failure message follows the pattern
    /// <c>"{paramName} must be an uppercase letter."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Uppercase(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsUppercase"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Uppercase(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be an uppercase letter.";
        var ok = CharRules.IsUppercase(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Casing.NotUpper, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a lowercase Unicode letter.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a lowercase letter, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsLowercase"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a lowercase letter."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Lowercase(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsLowercase"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> Lowercase(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a lowercase letter.";
        var ok = CharRules.IsLowercase(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Casing.NotLower, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is a valid hexadecimal digit (0–9, A–F, a–f).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a hexadecimal digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsHexDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a hexadecimal digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HexDigit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsHexDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> HexDigit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a hexadecimal digit.";
        var ok = CharRules.IsHexDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.NotHexDigit, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified character is not a valid hexadecimal digit (0–9, A–F, a–f).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="char"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a hexadecimal digit, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="CharRules.IsHexDigit"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a hexadecimal digit."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotHexDigit(ch);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CharRules.IsHexDigit"/>
    /// <seealso href="https://pineguard.ai/docs/must/char">Char Must Clauses documentation</seealso>
    public static MustResult<char> NotHexDigit(this IMustClause _,
        char value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a hexadecimal digit.";
        var ok = !CharRules.IsHexDigit(value);
        return MustResult<char>.FromBool(ok, MustCodes.Character.Charset.HexDigit, messageTemplate, paramName, value, value);
    }
}
