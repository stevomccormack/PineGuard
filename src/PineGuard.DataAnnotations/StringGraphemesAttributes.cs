using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field holds exactly the specified number of
/// grapheme clusters — the characters a reader sees.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// <see cref="string.Length"/> counts UTF-16 code units, so a family emoji reads as eleven characters and an
/// accented letter written with a combining mark reads as two. Segmentation follows the host runtime's
/// Unicode tables. Validation fails if <see cref="Count"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AddressModel
/// {
///     [HasExactGraphemeCount(2)]
///     public string CountryCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasExactGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasExactGraphemeCountAttribute(int count) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.Mismatch, allowNull: false)
{
    /// <summary>Gets the required number of characters.</summary>
    public int Count { get; } = count;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.HasExactGraphemeCount(strValue, Count, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not hold exactly the specified
/// number of grapheme clusters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if <see cref="Count"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [NotHasExactGraphemeCount(1)]
///     public string Nickname { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasExactGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasExactGraphemeCountAttribute(int count) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.Match, allowNull: false)
{
    /// <summary>Gets the number of characters that must not match.</summary>
    public int Count { get; } = count;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotHasExactGraphemeCount(strValue, Count, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field holds at least the specified number of
/// grapheme clusters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if <see cref="Min"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [HasMinGraphemeCount(3)]
///     public string DisplayName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasMinGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasMinGraphemeCountAttribute(int min) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.TooFew, allowNull: false)
{
    /// <summary>Gets the minimum required number of characters.</summary>
    public int Min { get; } = min;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.HasMinGraphemeCount(strValue, Min, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not hold at least the specified
/// number of grapheme clusters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if <see cref="Min"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LabelModel
/// {
///     [NotHasMinGraphemeCount(5)]
///     public string Abbreviation { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasMinGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasMinGraphemeCountAttribute(int min) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.TooMany, allowNull: false)
{
    /// <summary>Gets the minimum number of characters that must not be reached.</summary>
    public int Min { get; } = min;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotHasMinGraphemeCount(strValue, Min, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field holds at most the specified number of
/// grapheme clusters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// This is the attribute a "your name is too long" limit wants: a family emoji costs eleven code units and
/// one character, so a <see cref="MaxLengthAttribute"/> rejects a name the user can see is short enough.
/// Validation fails if <see cref="Max"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [HasMaxGraphemeCount(50)]
///     public string DisplayName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasMaxGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasMaxGraphemeCountAttribute(int max) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.TooMany, allowNull: false)
{
    /// <summary>Gets the maximum allowed number of characters.</summary>
    public int Max { get; } = max;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.HasMaxGraphemeCount(strValue, Max, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not hold at most the specified
/// number of grapheme clusters.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if <see cref="Max"/> is negative.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ArticleModel
/// {
///     [NotHasMaxGraphemeCount(10)]
///     public string Summary { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasMaxGraphemeCountAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasMaxGraphemeCountAttribute(int max) : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.TooFew, allowNull: false)
{
    /// <summary>Gets the maximum number of characters that must be exceeded.</summary>
    public int Max { get; } = max;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotHasMaxGraphemeCount(strValue, Max, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the number of grapheme clusters in the annotated <see cref="string"/> property or field
/// falls within the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.HasGraphemeCountInRange"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if either bound is negative or the range is inverted.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [HasGraphemeCountInRange(3, 50)]
///     public string DisplayName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasGraphemeCountInRangeAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.HasGraphemeCountInRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasGraphemeCountInRangeAttribute(int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.OutOfRange, allowNull: false)
{
    /// <summary>Gets the lower bound of the acceptable number of characters.</summary>
    public int Min { get; } = min;

    /// <summary>Gets the upper bound of the acceptable number of characters.</summary>
    public int Max { get; } = max;

    /// <summary>Gets whether the bounds are included or excluded in the acceptable range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.HasGraphemeCountInRange(strValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the number of grapheme clusters in the annotated <see cref="string"/> property or field
/// falls outside the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringGraphemesClauses.NotHasGraphemeCountInRange"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Validation fails if either bound is negative or the range is inverted.
/// If the value is <see langword="null"/>, input validation is skipped after configuration has been checked.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [NotHasGraphemeCountInRange(1, 2)]
///     public string Handle { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasGraphemeCountInRangeAttribute"/>
/// <seealso cref="MustStringGraphemesClauses.NotHasGraphemeCountInRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string-graphemes">String Graphemes Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasGraphemeCountInRangeAttribute(int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Text.Graphemes.InRange, allowNull: false)
{
    /// <summary>Gets the lower bound of the forbidden number of characters.</summary>
    public int Min { get; } = min;

    /// <summary>Gets the upper bound of the forbidden number of characters.</summary>
    public int Max { get; } = max;

    /// <summary>Gets whether the bounds are included or excluded in the forbidden range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotHasGraphemeCountInRange(strValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
