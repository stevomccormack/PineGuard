using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate <see cref="Guid"/> values.
/// </summary>
/// <seealso cref="GuidRules"/>
/// <seealso href="https://pineguard.ai/docs/must/guid">GUID Must Clauses documentation</seealso>
public static class MustGuidClauses
{
    /// <summary>
    /// Validates that the specified <see cref="Guid"/> value is not the empty GUID (<see cref="Guid.Empty"/>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="Guid"/> value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not <see cref="Guid.Empty"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="GuidRules.IsEmpty"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be an empty GUID."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotEmpty(entityId);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GuidRules.IsEmpty"/>
    /// <seealso href="https://pineguard.ai/docs/must/guid">GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> NotEmpty(
        this IMustClause _,
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be an empty GUID.";

        var ok = !GuidRules.IsEmpty(value);
        return MustResult<Guid>.FromBool(ok, MustCodes.Guid.Emptiness.Empty, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified <see cref="Guid"/> value carries the given UUID version.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The <see cref="Guid"/> value to validate.</param>
    /// <param name="version">
    /// The expected version, from <see cref="GuidRules.MinVersion"/> to <see cref="GuidRules.MaxVersion"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> carries version <paramref name="version"/>, or <see langword="false"/>
    /// with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failure attributed to <paramref name="version"/> when it falls outside
    /// <see cref="GuidRules.MinVersion"/>–<see cref="GuidRules.MaxVersion"/>, since asking for a version
    /// no UUID layout defines is programmer misuse rather than bad input. Delegates to
    /// <see cref="GuidRules.HasVersion"/>, so <see cref="Guid.Empty"/> is versionless rather than version 0.
    /// The failure message follows the pattern <c>"{paramName} must have the specified GUID version."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HasGuidVersion(entityId, 4);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GuidRules.HasVersion"/>
    /// <seealso href="https://pineguard.ai/docs/must/guid">GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> HasGuidVersion(
        this IMustClause _,
        Guid value,
        int version,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (version is < GuidRules.MinVersion or > GuidRules.MaxVersion)
            return MustResult<Guid>.Fail(MustCodes.Guid.Version.Mismatch, "{paramName} requires a value between 1 and 8.", nameof(version), version);

        const string messageTemplate = "{paramName} must have the specified GUID version.";

        var ok = GuidRules.HasVersion(value, version);
        return MustResult<Guid>.FromBool(ok, MustCodes.Guid.Version.Mismatch, messageTemplate, paramName, value, value);
    }
}
