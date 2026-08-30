using System.Text.RegularExpressions;
using PineGuard.Utils;

#pragma warning disable CS8795 // Partial method must have an implementation part (source generator provides it)

namespace PineGuard.Rules;

/// <summary>
/// Provides pure version-string validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/version">Version Rules documentation</seealso>
public static partial class VersionRules
{
    /// <summary>
    /// The Semantic Versioning 2.0.0 reference pattern published at
    /// <see href="https://semver.org/#is-there-a-suggested-regular-expression-regex-to-check-a-semver-string">semver.org</see>,
    /// with every <c>\d</c> written as <c>[0-9]</c> so that .NET's Unicode-aware digit class cannot
    /// admit non-ASCII digits into a version number.
    /// </summary>
    public const string SemVerPattern =
        @"^(?<major>0|[1-9][0-9]*)\.(?<minor>0|[1-9][0-9]*)\.(?<patch>0|[1-9][0-9]*)(?:-(?<prerelease>(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$";

    /// <summary>
    /// Gets a compiled regex that matches a Semantic Versioning 2.0.0 string.
    /// </summary>
    /// <returns>A <see cref="Regex"/> compiled from <see cref="SemVerPattern"/>.</returns>
#if NET8_0_OR_GREATER
    [GeneratedRegex(SemVerPattern, RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 250)]
    public static partial Regex SemVerRegex();
#else
    public static Regex SemVerRegex() => CompiledSemVerRegex;
    private static readonly Regex CompiledSemVerRegex = new(SemVerPattern, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
#endif

    /// <summary>
    /// Determines whether the specified value is a Semantic Versioning 2.0.0 version string.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid SemVer 2.0.0 string; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Requires all three numeric components, each without leading zeros, and accepts the optional
    /// <c>-prerelease</c> and <c>+build</c> parts. A leading <c>v</c> (as in <c>v1.2.3</c>) is a packaging
    /// convention, not part of the specification, and is rejected. Leading and trailing whitespace is
    /// trimmed before validation.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = VersionRules.IsSemVer("1.0.0-alpha.1+build.7"); // true
    /// bool invalid = VersionRules.IsSemVer("1.0");                 // false
    /// </code>
    /// </example>
    public static bool IsSemVer(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && SemVerRegex().IsMatch(trimmed);
}
