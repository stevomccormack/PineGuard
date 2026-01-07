using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace PineGuard.Utils;

public static partial class EmailUtility
{
    public const int MaxEmailLength = 254;
    public const int MaxLocalPartLength = 64;
    public const int MaxDomainLength = 255;

    public const string DomainSeparator = "@";
    public const char DomainDotChar = '.';
    public const string DomainDot = ".";

    public const char AliasSeparatorChar = '+';

    public const string DomainNormalizationPattern = @"(@)(.+)$";
    private const int DomainNormalizationRegexTimeoutMilliseconds = 200;

    [GeneratedRegex(DomainNormalizationPattern, RegexOptions.None, matchTimeoutMilliseconds: DomainNormalizationRegexTimeoutMilliseconds)]
    public static partial Regex DomainNormalizationRegex();

    public static bool TryCreate(string? value, out MailAddress? email)
    {
        email = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return MailAddress.TryCreate(trimmed, out email);
    }

    /// <summary>
    /// Strict policy parser:
    /// - rejects display-name/angle-bracket forms (input must be address-only)
    /// - normalizes Unicode domain names to ASCII (punycode)
    /// - applies conservative length/whitespace and basic structural checks
    /// </summary>
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

        if (!MailAddress.TryCreate(normalized, out var parsed) || parsed is null)
            return false;

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
