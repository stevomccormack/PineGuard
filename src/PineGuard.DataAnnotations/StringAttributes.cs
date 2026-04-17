using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// Core String Attributes (No suffix needed as they are string-specific/unique)

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has exactly the specified length.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ExactLength"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CodeModel
/// {
///     [ExactLength(6)]
///     public string PinCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LengthBetweenAttribute"/>
/// <seealso cref="MustStringClauses.ExactLength"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ExactLengthAttribute(int length) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the exact character length required.</summary>
    public int Length { get; } = length;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ExactLength(strValue, Length, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has a length between the specified
/// minimum and maximum (inclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.LengthBetween"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UsernameModel
/// {
///     [LengthBetween(3, 20)]
///     public string Username { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ExactLengthAttribute"/>
/// <seealso cref="MustStringClauses.LengthBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LengthBetweenAttribute(int min, int max) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the minimum character length (inclusive).</summary>
    public int Min { get; } = min;

    /// <summary>Gets the maximum character length (inclusive).</summary>
    public int Max { get; } = max;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LengthBetween(strValue, Min, Max, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has more than the specified number
/// of characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.LongerThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PasswordModel
/// {
///     [LongerThan(8)]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ShorterThanAttribute"/>
/// <seealso cref="MustStringClauses.LongerThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongerThanAttribute(int length) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the minimum character count that the value must exceed (exclusive).</summary>
    public int Length { get; } = length;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LongerThan(strValue, Length, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has fewer than the specified number
/// of characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ShorterThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TitleModel
/// {
///     [ShorterThan(100)]
///     public string Title { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongerThanAttribute"/>
/// <seealso cref="MustStringClauses.ShorterThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ShorterThanAttribute(int length) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the maximum character count that the value must stay below (exclusive).</summary>
    public int Length { get; } = length;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ShorterThan(strValue, Length, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field matches the specified regular
/// expression pattern.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Match"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The regex is compiled with <see cref="RegexOptions.None"/> and a 1-second timeout.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PostalModel
/// {
///     [Match(@"^\d{5}$")]
///     public string ZipCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotMatchAttribute"/>
/// <seealso cref="MustStringClauses.Match"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MatchAttribute(string pattern) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the regular expression pattern the value must match.</summary>
    public string Pattern { get; } = pattern;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var regex = new Regex(Pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        var result = Must.Be.Match(strValue, regex, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only alphabetic characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Alphabetic"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NameModel
/// {
///     [Alphabetic]
///     public string FirstName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotAlphabeticAttribute"/>
/// <seealso cref="MustStringClauses.Alphabetic"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AlphabeticAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Alphabetic(strValue, inclusions: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only numeric characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Numeric"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AccountModel
/// {
///     [NumericString]
///     public string AccountNumber { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNumericStringAttribute"/>
/// <seealso cref="MustStringClauses.Numeric"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NumericStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Numeric(strValue, inclusions: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only alphanumeric
/// characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Alphanumeric"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TokenModel
/// {
///     [Alphanumeric]
///     public string Token { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotAlphanumericAttribute"/>
/// <seealso cref="MustStringClauses.Alphanumeric"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AlphanumericAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Alphanumeric(strValue, inclusions: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only decimal digit
/// characters (<c>0</c>–<c>9</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.DigitsOnly(IMustClause, string, string)"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PinModel
/// {
///     [DigitsOnly]
///     public string Pin { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotDigitsOnlyAttribute"/>
/// <seealso cref="MustStringClauses.DigitsOnly(IMustClause, string, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DigitsOnlyAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.DigitsOnly(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not consist exclusively of
/// decimal digit characters, optionally allowing specified extra characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotDigitsOnly(IMustClause, string, string)"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TagModel
/// {
///     [NotDigitsOnly]
///     public string Tag { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="DigitsOnlyAttribute"/>
/// <seealso cref="MustStringClauses.NotDigitsOnly(IMustClause, string, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDigitsOnlyAttribute(char[]? inclusions = null) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets additional characters that are allowed alongside non-digit characters.</summary>
    public char[]? Inclusions { get; } = inclusions;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotDigitsOnly(strValue, Inclusions, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is an empty string
/// (<see cref="string.Empty"/>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Empty"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ClearModel
/// {
///     [EmptyString]
///     public string Buffer { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNullOrEmptyStringAttribute"/>
/// <seealso cref="MustStringClauses.Empty"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmptyStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Empty(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is <see langword="null"/> or an
/// empty string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NullOrEmpty"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Null values are passed through to the must clause rather than short-circuited by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ClearModel
/// {
///     [NullOrEmptyString]
///     public string? Optional { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNullOrEmptyStringAttribute"/>
/// <seealso cref="MustStringClauses.NullOrEmpty"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NullOrEmptyStringAttribute() : ValidationAttributeBase(typeof(string), allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NullOrEmpty(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not <see langword="null"/> and
/// not an empty string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotNullOrEmpty"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Null values are forwarded to the must clause to produce an appropriate failure message.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [NotNullOrEmptyString]
///     public string DisplayName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NullOrEmptyStringAttribute"/>
/// <seealso cref="MustStringClauses.NotNullOrEmpty"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNullOrEmptyStringAttribute() : ValidationAttributeBase(typeof(string), allowNull: false)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            var result = Must.Be.NotNullOrEmpty(null, paramName: null);
            return FromMustResult(result, validationContext);
        }

        var strValue = (string)value;
        var result2 = Must.Be.NotNullOrEmpty(strValue, paramName: null);
        return FromMustResult(result2, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is <see langword="null"/> or
/// consists only of white-space characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NullOrWhiteSpace"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Null values are passed through to the must clause rather than short-circuited by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BlankModel
/// {
///     [NullOrWhiteSpaceString]
///     public string? Spacer { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNullOrWhiteSpaceStringAttribute"/>
/// <seealso cref="MustStringClauses.NullOrWhiteSpace"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NullOrWhiteSpaceStringAttribute() : ValidationAttributeBase(typeof(string), allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NullOrWhiteSpace(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not <see langword="null"/> and
/// does not consist only of white-space characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotNullOrWhiteSpace"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Null values are forwarded to the must clause to produce an appropriate failure message.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CommentModel
/// {
///     [NotNullOrWhiteSpaceString]
///     public string Body { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NullOrWhiteSpaceStringAttribute"/>
/// <seealso cref="MustStringClauses.NotNullOrWhiteSpace"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNullOrWhiteSpaceStringAttribute() : ValidationAttributeBase(typeof(string), allowNull: false)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            var result = Must.Be.NotNullOrWhiteSpace(null, paramName: null);
            return FromMustResult(result, validationContext);
        }

        var strValue = (string)value;
        var result2 = Must.Be.NotNullOrWhiteSpace(strValue, paramName: null);
        return FromMustResult(result2, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has at least the specified number
/// of characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.LongerThanOrEqual"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PasswordModel
/// {
///     [LongerThanOrEqual(8)]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ShorterThanOrEqualAttribute"/>
/// <seealso cref="MustStringClauses.LongerThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongerThanOrEqualAttribute(int length) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the minimum character length (inclusive).</summary>
    public int Length { get; } = length;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LongerThanOrEqual(strValue, Length, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field has at most the specified number
/// of characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ShorterThanOrEqual"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TweetModel
/// {
///     [ShorterThanOrEqual(280)]
///     public string Text { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongerThanOrEqualAttribute"/>
/// <seealso cref="MustStringClauses.ShorterThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ShorterThanOrEqualAttribute(int length) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the maximum character length (inclusive).</summary>
    public int Length { get; } = length;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ShorterThanOrEqual(strValue, Length, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is entirely uppercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Uppercase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// When <see cref="LettersOnly"/> is <see langword="true"/>, only alphabetic characters are checked for
/// casing; non-letter characters are ignored.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CodeModel
/// {
///     [UppercaseString]
///     public string CountryCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LowercaseStringAttribute"/>
/// <seealso cref="MustStringClauses.Uppercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UppercaseStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <summary>
    /// Gets or sets a value indicating whether only alphabetic characters are checked for uppercase
    /// casing. Defaults to <see langword="false"/>.
    /// </summary>
    public bool LettersOnly { get; set; }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Uppercase(strValue, LettersOnly, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not entirely uppercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotUppercase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// When <see cref="LettersOnly"/> is <see langword="true"/>, only alphabetic characters are checked for
/// casing; non-letter characters are ignored.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SentenceModel
/// {
///     [NotUppercaseString]
///     public string Sentence { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UppercaseStringAttribute"/>
/// <seealso cref="MustStringClauses.NotUppercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotUppercaseStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <summary>
    /// Gets or sets a value indicating whether only alphabetic characters are checked for uppercase
    /// casing. Defaults to <see langword="false"/>.
    /// </summary>
    public bool LettersOnly { get; set; }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotUppercase(strValue, LettersOnly, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is entirely lowercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Lowercase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// When <see cref="LettersOnly"/> is <see langword="true"/>, only alphabetic characters are checked for
/// casing; non-letter characters are ignored.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SlugModel
/// {
///     [LowercaseString]
///     public string Slug { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UppercaseStringAttribute"/>
/// <seealso cref="MustStringClauses.Lowercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LowercaseStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <summary>
    /// Gets or sets a value indicating whether only alphabetic characters are checked for lowercase
    /// casing. Defaults to <see langword="false"/>.
    /// </summary>
    public bool LettersOnly { get; set; }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Lowercase(strValue, LettersOnly, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not entirely lowercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotLowercase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// When <see cref="LettersOnly"/> is <see langword="true"/>, only alphabetic characters are checked for
/// casing; non-letter characters are ignored.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PasswordModel
/// {
///     [NotLowercaseString]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LowercaseStringAttribute"/>
/// <seealso cref="MustStringClauses.NotLowercase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotLowercaseStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <summary>
    /// Gets or sets a value indicating whether only alphabetic characters are checked for lowercase
    /// casing. Defaults to <see langword="false"/>.
    /// </summary>
    public bool LettersOnly { get; set; }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotLowercase(strValue, LettersOnly, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only ASCII characters
/// (code points 0–127).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.Ascii"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HeaderModel
/// {
///     [AsciiString]
///     public string Value { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotAsciiStringAttribute"/>
/// <seealso cref="MustStringClauses.Ascii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AsciiStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Ascii(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains at least one non-ASCII
/// character (code point greater than 127).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotAscii"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UnicodeModel
/// {
///     [NotAsciiString]
///     public string Content { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AsciiStringAttribute"/>
/// <seealso cref="MustStringClauses.NotAscii"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAsciiStringAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotAscii(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not match the specified
/// regular expression pattern.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotMatch"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The regex is compiled with <see cref="RegexOptions.None"/> and a 1-second timeout.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class CommentModel
/// {
///     [NotMatch(@"<script.*?>")]
///     public string Comment { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="MatchAttribute"/>
/// <seealso cref="MustStringClauses.NotMatch"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotMatchAttribute(string pattern) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the regular expression pattern the value must not match.</summary>
    public string Pattern { get; } = pattern;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var regex = new Regex(Pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
        var result = Must.Be.NotMatch(strValue, regex, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains characters that are not
/// all alphabetic, optionally permitting specified extra characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotAlphabetic"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HandleModel
/// {
///     [NotAlphabetic]
///     public string Handle { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AlphabeticAttribute"/>
/// <seealso cref="MustStringClauses.NotAlphabetic"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAlphabeticAttribute(char[]? inclusions = null) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets additional characters that are allowed alongside non-alphabetic content.</summary>
    public char[]? Inclusions { get; } = inclusions;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotAlphabetic(strValue, Inclusions, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains characters that are not
/// all alphanumeric, optionally permitting specified extra characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotAlphanumeric"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DescriptionModel
/// {
///     [NotAlphanumeric]
///     public string Description { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AlphanumericAttribute"/>
/// <seealso cref="MustStringClauses.NotAlphanumeric"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAlphanumericAttribute(char[]? inclusions = null) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets additional characters that are allowed alongside non-alphanumeric content.</summary>
    public char[]? Inclusions { get; } = inclusions;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotAlphanumeric(strValue, Inclusions, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains characters that are not
/// all numeric, optionally permitting specified extra characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotNumeric"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CodeModel
/// {
///     [NotNumericString]
///     public string ProductCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NumericStringAttribute"/>
/// <seealso cref="MustStringClauses.NotNumeric"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNumericStringAttribute(char[]? inclusions = null) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets additional characters that are allowed alongside non-numeric content.</summary>
    public char[]? Inclusions { get; } = inclusions;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotNumeric(strValue, Inclusions, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains at least one white-space
/// character.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ContainsWhitespace"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FullNameModel
/// {
///     [ContainsWhitespace]
///     public string FullName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsWhitespaceAttribute"/>
/// <seealso cref="MustStringClauses.ContainsWhitespace"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsWhitespaceAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ContainsWhitespace(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain any white-space
/// characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotContainsWhitespace"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UsernameModel
/// {
///     [NotContainsWhitespace]
///     public string Username { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsWhitespaceAttribute"/>
/// <seealso cref="MustStringClauses.NotContainsWhitespace"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsWhitespaceAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotContainsWhitespace(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains at least one control
/// character (Unicode category Control).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ContainsControlChars"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BinaryModel
/// {
///     [ContainsControlChars]
///     public string RawData { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsControlCharsAttribute"/>
/// <seealso cref="MustStringClauses.ContainsControlChars"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsControlCharsAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ContainsControlChars(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain any control
/// characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotContainsControlChars"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UserInputModel
/// {
///     [NotContainsControlChars]
///     public string Input { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsControlCharsAttribute"/>
/// <seealso cref="MustStringClauses.NotContainsControlChars"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsControlCharsAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotContainsControlChars(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains only characters from the
/// specified allowed character set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ContainsAllowedOnly"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HexModel
/// {
///     [ContainsAllowedOnly(new[] { '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f' })]
///     public string HexValue { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsAllowedOnlyAttribute"/>
/// <seealso cref="MustStringClauses.ContainsAllowedOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsAllowedOnlyAttribute(char[] allowedChars) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the set of characters that are the only ones permitted in the value.</summary>
    public char[] AllowedChars { get; } = allowedChars;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ContainsAllowedOnly(strValue, AllowedChars, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains characters outside the
/// specified allowed character set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotContainsAllowedOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RichTextModel
/// {
///     [NotContainsAllowedOnly(new[] { 'a', 'b', 'c' })]
///     public string Content { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsAllowedOnlyAttribute"/>
/// <seealso cref="MustStringClauses.NotContainsAllowedOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsAllowedOnlyAttribute(char[] allowedChars) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the allowed character set; the value must contain at least one character outside it.</summary>
    public char[] AllowedChars { get; } = allowedChars;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotContainsAllowedOnly(strValue, AllowedChars, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains at least one disallowed
/// character from the specified set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ContainsDisallowed"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SpecialCharModel
/// {
///     [ContainsDisallowed(new[] { '!', '@', '#' })]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsDisallowedAttribute"/>
/// <seealso cref="MustStringClauses.ContainsDisallowed"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsDisallowedAttribute(char[] disallowedChars) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the set of disallowed characters; at least one must be present in the value.</summary>
    public char[] DisallowedChars { get; } = disallowedChars;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ContainsDisallowed(strValue, DisallowedChars, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not contain any of the
/// specified disallowed characters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.NotContainsDisallowed"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code><![CDATA[
/// public class SafeInputModel
/// {
///     [NotContainsDisallowed(new[] { '<', '>', '&' })]
///     public string Input { get; set; }
/// }
/// ]]></code>
/// </example>
/// <seealso cref="ContainsDisallowedAttribute"/>
/// <seealso cref="MustStringClauses.NotContainsDisallowed"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsDisallowedAttribute(char[] disallowedChars) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the set of characters that must not appear in the value.</summary>
    public char[] DisallowedChars { get; } = disallowedChars;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotContainsDisallowed(strValue, DisallowedChars, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field contains at least one character
/// from the specified set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringClauses.ContainsAny"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PasswordModel
/// {
///     [ContainsAny(new[] { '!', '@', '#', '$' })]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustStringClauses.ContainsAny"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsAnyAttribute(char[] characters) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the set of characters; at least one must appear in the value.</summary>
    public char[] Characters { get; } = characters;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ContainsAny(strValue, Characters, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
