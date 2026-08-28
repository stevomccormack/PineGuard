using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid JSON string
/// (any JSON value type).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustJsonClauses.Json"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Accepts any valid JSON value including objects, arrays, strings, numbers, booleans, and <see langword="null"/>.
/// For stricter type requirements, use <see cref="JsonObjectAttribute"/> or <see cref="JsonArrayAttribute"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConfigModel
/// {
///     [Json]
///     public string RawJson { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="JsonObjectAttribute"/>
/// <seealso cref="JsonArrayAttribute"/>
/// <seealso cref="MustJsonClauses.Json"/>
/// <seealso href="https://pineguard.ai/docs/annotations/json">JSON Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class JsonAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Json.Document.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Json(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid JSON object
/// (starts with <c>{</c> and ends with <c>}</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustJsonClauses.JsonObject"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConfigModel
/// {
///     [JsonObject]
///     public string Settings { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="JsonAttribute"/>
/// <seealso cref="JsonArrayAttribute"/>
/// <seealso cref="MustJsonClauses.JsonObject"/>
/// <seealso href="https://pineguard.ai/docs/annotations/json">JSON Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class JsonObjectAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Json.Root.NotObject)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.JsonObject(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid JSON array
/// (starts with <c>[</c> and ends with <c>]</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustJsonClauses.JsonArray"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BatchModel
/// {
///     [JsonArray]
///     public string Items { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="JsonAttribute"/>
/// <seealso cref="JsonObjectAttribute"/>
/// <seealso cref="MustJsonClauses.JsonArray"/>
/// <seealso href="https://pineguard.ai/docs/annotations/json">JSON Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class JsonArrayAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Json.Root.NotArray)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.JsonArray(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
