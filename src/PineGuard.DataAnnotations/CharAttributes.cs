using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is an ASCII character (code point 0–127).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Ascii"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharAscii]
///     public char Separator { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNonAsciiAttribute"/>
/// <seealso cref="MustCharClauses.Ascii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharAsciiAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Ascii(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a decimal digit (0–9).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Digit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharDigit]
///     public char Pin { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNotDigitAttribute"/>
/// <seealso cref="MustCharClauses.Digit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Digit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a Unicode letter.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Letter"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharLetter]
///     public char Initial { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNotLetterAttribute"/>
/// <seealso cref="MustCharClauses.Letter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharLetterAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Letter(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a Unicode letter or decimal digit.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.LetterOrDigit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharLetterOrDigit]
///     public char Code { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNonLetterOrDigitAttribute"/>
/// <seealso cref="MustCharClauses.LetterOrDigit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharLetterOrDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.LetterOrDigit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a lowercase letter.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Lowercase"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharLowercase]
///     public char Suffix { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharUppercaseAttribute"/>
/// <seealso cref="MustCharClauses.Lowercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharLowercaseAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Lowercase(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is an uppercase letter.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Uppercase"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharUppercase]
///     public char Prefix { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharLowercaseAttribute"/>
/// <seealso cref="MustCharClauses.Uppercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharUppercaseAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Uppercase(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a hexadecimal digit.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotHexDigit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNotHexDigit]
///     public char Marker { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharHexDigitAttribute"/>
/// <seealso cref="MustCharClauses.NotHexDigit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNotHexDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotHexDigit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a Unicode letter.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotLetter"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNotLetter]
///     public char Delimiter { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharLetterAttribute"/>
/// <seealso cref="MustCharClauses.NotLetter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNotLetterAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotLetter(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a decimal digit.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotDigit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNotDigit]
///     public char Symbol { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharDigitAttribute"/>
/// <seealso cref="MustCharClauses.NotDigit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNotDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotDigit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is neither a Unicode letter nor a decimal digit.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotLetterOrDigit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNonLetterOrDigit]
///     public char SpecialChar { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharLetterOrDigitAttribute"/>
/// <seealso cref="MustCharClauses.NotLetterOrDigit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNonLetterOrDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotLetterOrDigit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not an ASCII character (code point > 127).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotAscii"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNonAscii]
///     public char UnicodeGlyph { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharAsciiAttribute"/>
/// <seealso cref="MustCharClauses.NotAscii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNonAsciiAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotAscii(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a printable ASCII character
/// (code points 32–126).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotPrintableAscii"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNonPrintableAscii]
///     public char ControlCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharPrintableAsciiAttribute"/>
/// <seealso cref="MustCharClauses.NotPrintableAscii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNonPrintableAsciiAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotPrintableAscii(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a printable ASCII character
/// (code points 32–126).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.PrintableAscii"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharPrintableAscii]
///     public char DisplayChar { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNonPrintableAsciiAttribute"/>
/// <seealso cref="MustCharClauses.PrintableAscii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharPrintableAsciiAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.PrintableAscii(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a whitespace character.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotWhitespace"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNonWhitespace]
///     public char Token { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustCharClauses.NotWhitespace"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNonWhitespaceAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotWhitespace(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a Unicode control character.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.Control"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharControl]
///     public char ControlChar { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNotControlAttribute"/>
/// <seealso cref="MustCharClauses.Control"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharControlAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.Control(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is not a Unicode control character.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.NotControl"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharNotControl]
///     public char SafeChar { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharControlAttribute"/>
/// <seealso cref="MustCharClauses.NotControl"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharNotControlAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.NotControl(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="char"/> property or field is a hexadecimal digit (0–9, A–F, a–f).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCharClauses.HexDigit"/>. Supported on properties, fields, and parameters
/// of type <see cref="char"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InputModel
/// {
///     [CharHexDigit]
///     public char HexNibble { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CharNotHexDigitAttribute"/>
/// <seealso cref="MustCharClauses.HexDigit"/>
/// <seealso href="https://pineguard.ai/docs/annotations/char">Char Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CharHexDigitAttribute() : ValidationAttributeBase(typeof(char))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var charValue = (char)value!;
        var result = Must.Be.HexDigit(charValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
