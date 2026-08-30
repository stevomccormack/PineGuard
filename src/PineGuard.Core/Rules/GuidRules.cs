namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="System.Guid"/> validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
public static class GuidRules
{
    /// <summary>
    /// The lowest version number defined for a UUID (<c>1</c>, the time-based layout).
    /// </summary>
    public const int MinVersion = 1;

    /// <summary>
    /// The highest version number defined for a UUID (<c>8</c>, the custom layout added by RFC 9562).
    /// </summary>
    public const int MaxVersion = 8;

    private const int VersionByteIndex = 7;
    private const int VersionNibbleShift = 4;

    /// <summary>
    /// Determines whether the specified value is empty (equal to <see cref="System.Guid.Empty"/>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/> (null is absent, not empty).</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and equal to
    /// <see cref="System.Guid.Empty"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool empty = GuidRules.IsEmpty(Guid.Empty);   // true
    /// bool empty = GuidRules.IsEmpty(Guid.NewGuid()); // false
    /// bool empty = GuidRules.IsEmpty(null); // false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
    public static bool IsEmpty(Guid? value) => value is not null && value.Value == Guid.Empty;

    /// <summary>
    /// Determines whether the specified value is not empty (not equal to <see cref="System.Guid.Empty"/>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and not equal to
    /// <see cref="System.Guid.Empty"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool notEmpty = GuidRules.IsNotEmpty(Guid.NewGuid()); // true
    /// bool notEmpty = GuidRules.IsNotEmpty(Guid.Empty);     // false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
    public static bool IsNotEmpty(Guid? value) => value is not null && value.Value != Guid.Empty;

    /// <summary>
    /// Determines whether the specified value carries the given UUID version.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="version">The expected version, from <see cref="MinVersion"/> to <see cref="MaxVersion"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and its version
    /// nibble equals <paramref name="version"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The version occupies the four high bits of the seventh byte — the first hexadecimal digit of the
    /// third group in the canonical form, so <c>…-4ec7-…</c> is version 4. The nibble is read from
    /// <see cref="System.Guid.ToByteArray()"/> rather than from a <c>Version</c> property, which is
    /// unavailable on the older target frameworks. A <paramref name="version"/> outside
    /// <see cref="MinVersion"/>–<see cref="MaxVersion"/> names no defined layout and never matches, so
    /// <see cref="System.Guid.Empty"/> — whose nibble is <c>0</c> — is reported as versionless rather
    /// than as version 0.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool v4 = GuidRules.HasVersion(Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), 4); // true
    /// bool v7 = GuidRules.HasVersion(Guid.Parse("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"), 7); // false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
    public static bool HasVersion(Guid? value, int version)
    {
        if (value is null || version < MinVersion || version > MaxVersion)
            return false;

        return value.Value.ToByteArray()[VersionByteIndex] >> VersionNibbleShift == version;
    }
}
