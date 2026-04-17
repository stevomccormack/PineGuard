using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

#pragma warning disable CS8795 // Partial method must have an implementation part (source generator provides it)

namespace PineGuard.Utils;

/// <summary>
/// Provides email address parsing and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/email">Email Utility documentation</seealso>
public static partial class EmailUtility
{
    /// <summary>
    /// The maximum total length of an email address (254 characters per RFC 5321).
    /// </summary>
    public const int MaxEmailLength = 254;

    /// <summary>
    /// The maximum length of the local part of an email address (64 characters per RFC 5321).
    /// </summary>
    public const int MaxLocalPartLength = 64;

    /// <summary>
    /// The maximum length of the domain part of an email address (255 characters).
    /// </summary>
    public const int MaxDomainLength = 255;

    /// <summary>
    /// The domain separator character (<c>@</c>).
    /// </summary>
    public const string DomainSeparator = "@";

    /// <summary>
    /// The domain dot character (<c>.</c>).
    /// </summary>
    public const char DomainDotChar = '.';

    /// <summary>
    /// The domain dot string (<c>.</c>).
    /// </summary>
    public const string DomainDot = ".";

    /// <summary>
    /// The alias separator character (<c>+</c>) used in plus-addressing.
    /// </summary>
    public const char AliasSeparatorChar = '+';

    /// <summary>
    /// The regex pattern for normalizing the domain portion of an email address.
    /// </summary>
    public const string DomainNormalizationPattern = "(@)(.+)$";
    private const int DomainNormalizationRegexTimeoutMilliseconds = 200;

    /// <summary>
    /// Gets a compiled regex for domain normalization.
    /// </summary>
    /// <returns>A <see cref="Regex"/> compiled from <see cref="DomainNormalizationPattern"/>.</returns>
#if NET8_0_OR_GREATER
    [GeneratedRegex(DomainNormalizationPattern, RegexOptions.None,
        matchTimeoutMilliseconds: DomainNormalizationRegexTimeoutMilliseconds)]
    public static partial Regex DomainNormalizationRegex();
#else
    public static Regex DomainNormalizationRegex() => CompiledDomainNormalizationRegex;
    private static readonly Regex CompiledDomainNormalizationRegex = new(DomainNormalizationPattern, RegexOptions.None | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(DomainNormalizationRegexTimeoutMilliseconds));
#endif

    /// <summary>
    /// Attempts to create a <see cref="MailAddress"/> from the specified string using lenient parsing.
    /// </summary>
    /// <param name="value">The email string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="email">When this method returns, contains the parsed address if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the email was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(string? value, out MailAddress? email)
    {
        email = null;

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

#if NET8_0_OR_GREATER
        return MailAddress.TryCreate(trimmed, out email);
#else
        try { email = new MailAddress(trimmed); return true; }
        catch (FormatException) { return false; }
#endif
    }

    /// <summary>
    /// Attempts to create a <see cref="MailAddress"/> from the specified string using strict RFC-compliant parsing.
    /// </summary>
    /// <param name="value">The email string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="email">When this method returns, contains the parsed address if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the email passed strict validation; otherwise, <see langword="false"/>.</returns>
    public static bool TryStrictCreate(string? value, out MailAddress? email)
    {
        email = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (trimmed.Length > MaxEmailLength)
            return false;

        if (trimmed.Any(char.IsWhiteSpace))
            return false;

        var at = trimmed.IndexOf(DomainSeparator, StringComparison.Ordinal);
        if (at <= 0 || at != trimmed.LastIndexOf(DomainSeparator, StringComparison.Ordinal) || at == trimmed.Length - 1)
            return false;

        var local = trimmed[..at];
        var domain = trimmed[(at + 1)..];

        if (local.Length > MaxLocalPartLength)
            return false;

        if (!domain.Contains(DomainDotChar, StringComparison.Ordinal))
            return false;

        if (domain.EndsWith(DomainDot, StringComparison.Ordinal))
            return false;

        string normalized;
        try
        {
            normalized = DomainNormalizationRegex().Replace(
                trimmed,
                static m =>
                {
                    var idn = new IdnMapping();
                    var asciiDomain = idn.GetAscii(m.Groups[2].Value);
                    return m.Groups[1].Value + asciiDomain;
                });
        }
        catch (ArgumentException)
        {
            return false;
        }

#if NET8_0_OR_GREATER
        if (!MailAddress.TryCreate(normalized, out var parsed))
            return false;
#else
        MailAddress? parsed;
        try { parsed = new MailAddress(normalized); }
        catch (FormatException) { return false; }
#endif

        if (!string.Equals(parsed.Address, normalized, StringComparison.Ordinal))
            return false;

        email = parsed;
        return true;
    }

    /// <summary>
    /// Attempts to extract the "alias" (plus-addressing) portion of an email address.
    /// Example: "user+alias@example.com" => "alias".
    /// Returns false if the address is invalid/unsupported or contains no alias.
    /// </summary>
    public static bool TryGetAlias(string? value, out string alias)
    {
        alias = string.Empty;

        if (!TryStrictCreate(value, out var email) || email is null)
            return false;

        var address = email.Address;
        var at = address.IndexOf(DomainSeparator, StringComparison.Ordinal);
        // `TryStrictCreate` guarantees a single '@' with a non-empty local-part.

        var local = address[..at];
        var plus = local.IndexOf(AliasSeparatorChar);
        if (plus < 0 || plus == local.Length - 1)
            return false;

        alias = local[(plus + 1)..];
        return alias.Length > 0;
    }
}
