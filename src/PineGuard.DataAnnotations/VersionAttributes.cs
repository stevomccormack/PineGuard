using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a Semantic Versioning 2.0.0
/// version.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustVersionClauses.SemVer"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// The specification's own grammar is applied, so <c>major.minor.patch</c> are all required, leading zeros
/// are rejected, and an optional pre-release and build-metadata suffix are honoured. A leading <c>v</c> is
/// not part of the grammar and fails. If the value is <see langword="null"/>, validation is skipped by the
/// base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PackageModel
/// {
///     [SemVer]
///     public string Version { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustVersionClauses.SemVer"/>
/// <seealso href="https://pineguard.ai/docs/annotations/version">Version Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SemVerAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Version.Semver.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.SemVer(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
