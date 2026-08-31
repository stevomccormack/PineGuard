using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate version strings,
/// delegating to <see cref="VersionRules"/> for core validation logic.
/// </summary>
/// <seealso cref="VersionRules"/>
/// <seealso href="https://pineguard.ai/docs/must/version">Version Must Clauses documentation</seealso>
public static class MustVersionClauses
{
    /// <summary>
    /// Validates that the specified string is a Semantic Versioning 2.0.0 version.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a semantic version.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid SemVer 2.0.0 string, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="VersionRules.IsSemVer"/>, so all three numeric components are required and a
    /// leading <c>v</c> is rejected as a packaging convention rather than part of the specification. The
    /// failure message follows the pattern <c>"{paramName} must be a valid semantic version."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.SemVer(packageVersion);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="VersionRules.IsSemVer"/>
    /// <seealso href="https://pineguard.ai/docs/must/version">Version Must Clauses documentation</seealso>
    public static MustResult<string> SemVer(
        this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Version.Semver.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a valid semantic version.";

        var ok = VersionRules.IsSemVer(value);
        return MustResult<string>.FromBool(ok, MustCodes.Version.Semver.Invalid, messageTemplate, paramName, value, value);
    }
}
