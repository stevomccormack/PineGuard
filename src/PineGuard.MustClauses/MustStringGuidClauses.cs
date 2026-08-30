using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate GUID string representations,
/// parsing the input string before delegating to GUID rules.
/// </summary>
/// <seealso cref="GuidRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-guid">String GUID Must Clauses documentation</seealso>
public static class MustStringGuidClauses
{
    /// <summary>
    /// Validates that the specified string can be parsed as a valid <see cref="System.Guid"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to parse and validate as a GUID.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> parses as a valid GUID, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="System.Guid"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringUtility.Guid"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid GUID."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Guid(guidString);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/must/string-guid">String GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> Guid(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))]
        string? paramName = null)
    {
        if (value is null)
            return MustResult<Guid>.Fail(MustCodes.Guid.Format.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a valid GUID.";

        var ok = StringUtility.Guid.TryParse(value, out Guid parsed);
        return MustResult<Guid>.FromBool(ok, MustCodes.Guid.Format.Invalid, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified string can be parsed as a valid <see cref="System.Guid"/> and is not
    /// <see cref="System.Guid.Empty"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to parse and validate as a non-empty GUID.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> parses as a valid, non-empty GUID, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="System.Guid"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="StringUtility.Guid"/> for parsing and <see cref="GuidRules.IsNotEmpty"/>.
    /// The failure message follows the pattern <c>"{paramName} must not be an empty GUID."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotEmptyGuid(guidString);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/must/string-guid">String GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> NotEmptyGuid(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))]
        string? paramName = null)
    {
        if (value is null)
            return MustResult<Guid>.Fail(MustCodes.Guid.Emptiness.Empty, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must not be an empty GUID.";

        var ok = StringUtility.Guid.TryParse(value, out Guid parsed)
                 && GuidRules.IsNotEmpty(parsed);

        return MustResult<Guid>.FromBool(ok, MustCodes.Guid.Emptiness.Empty, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified string can be parsed as a <see cref="System.Guid"/> carrying the given
    /// UUID version.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to parse and validate.</param>
    /// <param name="version">
    /// The expected version, from <see cref="GuidRules.MinVersion"/> to <see cref="GuidRules.MaxVersion"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> parses as a GUID carrying version <paramref name="version"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>. On success,
    /// <see cref="MustResult{T}.Result"/> contains the parsed <see cref="System.Guid"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failure attributed to <paramref name="version"/> when it falls outside
    /// <see cref="GuidRules.MinVersion"/>–<see cref="GuidRules.MaxVersion"/>, and a failed result
    /// immediately if <paramref name="value"/> is <see langword="null"/>. Delegates to
    /// <see cref="StringUtility.Guid"/> for parsing and <see cref="GuidRules.HasVersion"/> for the version,
    /// so every form <c>Guid.TryParse</c> accepts reads the same version. The failure message follows the
    /// pattern <c>"{paramName} must have the specified GUID version."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HasGuidVersion(idHeader, 4);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="GuidRules.HasVersion"/>
    /// <seealso href="https://pineguard.ai/docs/must/string-guid">String GUID Must Clauses documentation</seealso>
    public static MustResult<Guid> HasGuidVersion(this IMustClause _,
        string? value,
        int version,
        [CallerArgumentExpression(nameof(value))]
        string? paramName = null)
    {
        if (version is < GuidRules.MinVersion or > GuidRules.MaxVersion)
            return MustResult<Guid>.Fail(MustCodes.Guid.Version.Mismatch, "{paramName} requires a value between 1 and 8.", nameof(version), version);

        if (value is null)
            return MustResult<Guid>.Fail(MustCodes.Guid.Version.Mismatch, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must have the specified GUID version.";

        var ok = StringUtility.Guid.TryParse(value, out Guid parsed)
                 && GuidRules.HasVersion(parsed, version);

        return MustResult<Guid>.FromBool(ok, MustCodes.Guid.Version.Mismatch, messageTemplate, paramName, value, parsed);
    }
}
