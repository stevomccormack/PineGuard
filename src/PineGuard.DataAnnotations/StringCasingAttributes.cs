using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field conforms to the specified casing
/// style (e.g., camelCase, PascalCase, snake_case).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.CaseStyle"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ApiModel
/// {
///     [CaseStyle(StringCasing.CamelCase)]
///     public string FieldName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotCaseStyleAttribute"/>
/// <seealso cref="MustStringCasingClauses.CaseStyle"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CaseStyleAttribute(StringCasing casing) : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Mismatch)
{
    /// <summary>Gets the required casing style.</summary>
    public StringCasing Casing { get; } = casing;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CaseStyle(strValue, Casing, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not conform to the specified
/// casing style.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotCaseStyle"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NamingModel
/// {
///     [NotCaseStyle(StringCasing.SnakeCase)]
///     public string PropertyName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CaseStyleAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotCaseStyle"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotCaseStyleAttribute(StringCasing casing) : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Match)
{
    /// <summary>Gets the casing style that the value must not conform to.</summary>
    public StringCasing Casing { get; } = casing;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotCaseStyle(strValue, Casing, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in camelCase format (e.g.,
/// <c>"myVariableName"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.CamelCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class JsonModel
/// {
///     [CamelCase]
///     public string PropertyName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotCamelCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.CamelCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CamelCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotCamel)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.CamelCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in PascalCase format (e.g.,
/// <c>"MyClassName"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.PascalCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TypeModel
/// {
///     [PascalCase]
///     public string TypeName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotPascalCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.PascalCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PascalCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotPascal)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PascalCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in snake_case format (e.g.,
/// <c>"my_variable_name"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.SnakeCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DbColumnModel
/// {
///     [SnakeCase]
///     public string ColumnName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotSnakeCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.SnakeCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SnakeCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotSnake)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SnakeCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in UPPER_SNAKE_CASE format
/// (e.g., <c>"MY_CONSTANT_NAME"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.UpperSnakeCase"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConstantModel
/// {
///     [UpperSnakeCase]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotUpperSnakeCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.UpperSnakeCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UpperSnakeCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotUpperSnake)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.UpperSnakeCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in kebab-case format (e.g.,
/// <c>"my-url-slug"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.KebabCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RouteModel
/// {
///     [KebabCase]
///     public string Slug { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotKebabCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.KebabCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class KebabCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotKebab)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.KebabCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in Train-Case format (e.g.,
/// <c>"My-Component-Name"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.TrainCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HeaderModel
/// {
///     [TrainCase]
///     public string HeaderName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotTrainCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.TrainCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TrainCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotTrain)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.TrainCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in <c>dot.case</c> format (e.g.,
/// <c>"my.property.name"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.DotCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConfigKeyModel
/// {
///     [DotCase]
///     public string Key { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotDotCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.DotCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DotCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotDot)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.DotCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is in space case format (e.g.,
/// <c>"my property name"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.SpaceCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LabelModel
/// {
///     [SpaceCase]
///     public string Label { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotSpaceCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.SpaceCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SpaceCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotSpace)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SpaceCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is invariant uppercase (all letters
/// are uppercase, using <see cref="System.Globalization.CultureInfo.InvariantCulture"/>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.UpperInvariant"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CurrencyModel
/// {
///     [UpperInvariant]
///     public string CurrencyCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LowerInvariantAttribute"/>
/// <seealso cref="MustStringCasingClauses.UpperInvariant"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UpperInvariantAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotUpperInvariant)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.UpperInvariant(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is invariant lowercase (all letters
/// are lowercase, using <see cref="System.Globalization.CultureInfo.InvariantCulture"/>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.LowerInvariant"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SlugModel
/// {
///     [LowerInvariant]
///     public string Slug { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UpperInvariantAttribute"/>
/// <seealso cref="MustStringCasingClauses.LowerInvariant"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LowerInvariantAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.NotLowerInvariant)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LowerInvariant(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in camelCase format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotCamelCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CssModel
/// {
///     [NotCamelCase]
///     public string ClassName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CamelCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotCamelCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotCamelCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Camel)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotCamelCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in PascalCase format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotPascalCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UrlSegmentModel
/// {
///     [NotPascalCase]
///     public string Segment { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PascalCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotPascalCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotPascalCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Pascal)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotPascalCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in snake_case format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotSnakeCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ApiPropertyModel
/// {
///     [NotSnakeCase]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SnakeCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotSnakeCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotSnakeCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Snake)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotSnakeCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in UPPER_SNAKE_CASE format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotUpperSnakeCase"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class VariableModel
/// {
///     [NotUpperSnakeCase]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UpperSnakeCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotUpperSnakeCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotUpperSnakeCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.UpperSnake)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotUpperSnakeCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in kebab-case format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotKebabCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NamingModel
/// {
///     [NotKebabCase]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="KebabCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotKebabCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotKebabCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Kebab)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotKebabCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in Train-Case format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotTrainCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NamingModel
/// {
///     [NotTrainCase]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TrainCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotTrainCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotTrainCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Train)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotTrainCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in <c>dot.case</c> format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotDotCase"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class KeyModel
/// {
///     [NotDotCase]
///     public string Key { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="DotCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotDotCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDotCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Dot)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotDotCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not in space case format.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotSpaceCase"/>. Supported on properties, fields, and
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
///     [NotSpaceCase]
///     public string Tag { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SpaceCaseAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotSpaceCase"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotSpaceCaseAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.Space)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotSpaceCase(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not invariant uppercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotUpperInvariant"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MixedCaseModel
/// {
///     [NotUpperInvariant]
///     public string Name { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UpperInvariantAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotUpperInvariant"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotUpperInvariantAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.UpperInvariant)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotUpperInvariant(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is not invariant lowercase.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringCasingClauses.NotLowerInvariant"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TitleModel
/// {
///     [NotLowerInvariant]
///     public string Title { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LowerInvariantAttribute"/>
/// <seealso cref="MustStringCasingClauses.NotLowerInvariant"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotLowerInvariantAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Text.Casing.LowerInvariant)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotLowerInvariant(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
