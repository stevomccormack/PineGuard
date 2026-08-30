using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure network address and hostname validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/network">Network Rules documentation</seealso>
public static class NetworkRules
{
    private const int MacOctetCount = 6;
    private const int MacOctetHexLength = 2;
    private const int MacCiscoGroupCount = 3;
    private const int MacCiscoGroupHexLength = 4;
    private const char MacColonSeparator = ':';
    private const char MacHyphenSeparator = '-';
    private const char MacDotSeparator = '.';

    /// <summary>
    /// Determines whether the specified value is a valid IP address (IPv4 or IPv6).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid IP address; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsIpAddress("192.168.1.1");        // true (IPv4)
    /// bool valid = NetworkRules.IsIpAddress("::1");                 // true (IPv6)
    /// bool invalid = NetworkRules.IsIpAddress("not-an-ip");        // false
    /// </code>
    /// </example>
    public static bool IsIpAddress(string? value) =>
        NetworkUtility.TryParseIpAddress(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid IPv4 address.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid IPv4 address; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsIpv4("192.168.1.1"); // true
    /// bool invalid = NetworkRules.IsIpv4("::1");        // false
    /// </code>
    /// </example>
    public static bool IsIpv4(string? value) =>
        NetworkUtility.TryParseIpv4(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid IPv6 address.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid IPv6 address; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsIpv6("::1");          // true
    /// bool invalid = NetworkRules.IsIpv6("192.168.1.1"); // false
    /// </code>
    /// </example>
    public static bool IsIpv6(string? value) =>
        NetworkUtility.TryParseIpv6(value, out _);

    /// <summary>
    /// Determines whether the specified IP address falls within the given CIDR block.
    /// </summary>
    /// <param name="ip">The IP address to check. If <see langword="null"/> or invalid, returns <see langword="false"/>.</param>
    /// <param name="cidr">The CIDR notation block (e.g., <c>"192.168.1.0/24"</c>).</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="ip"/> falls within the <paramref name="cidr"/> range;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool inRange = NetworkRules.IsInCidr("192.168.1.100", "192.168.1.0/24"); // true
    /// </code>
    /// </example>
    public static bool IsInCidr(string? ip, string cidr)
    {
        if (!NetworkUtility.TryParseIpAddress(ip, out var address) || address is null)
            return false;

        if (!NetworkUtility.TryParseCidr(cidr, out var network, out var prefixLength) || network is null)
            return false;

        return NetworkUtility.IsInCidr(address, network, prefixLength);
    }

    /// <summary>
    /// Determines whether the specified value is a valid hostname (DNS label syntax).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a syntactically valid hostname
    /// that passes IDN conversion and DNS label rules; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsValidHostname("example.com");   // true
    /// bool invalid = NetworkRules.IsValidHostname("-invalid-.com"); // false
    /// </code>
    /// </example>
    public static bool IsValidHostname(string? value)
    {
        var idnConversionSucceeded = NetworkUtility.TryGetAsciiHostname(value, out var ascii) && ascii is not null;

        return idnConversionSucceeded && NetworkUtility.ValidateHostnameLabels(ascii!);
    }

    /// <summary>
    /// Determines whether the specified value is a valid TCP/UDP port number (1–65535).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is between 1 and 65535 inclusive;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsPortNumber(8080); // true
    /// bool invalid = NetworkRules.IsPortNumber(0);  // false
    /// </code>
    /// </example>
    public static bool IsPortNumber(int? value) =>
        value is >= 1 and <= 65535;

    /// <summary>
    /// Determines whether the specified value is a MAC (media access control) address.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a MAC address in one of the three conventional
    /// notations; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Accepts the IEEE colon and hyphen notations (<c>xx:xx:xx:xx:xx:xx</c>, <c>xx-xx-xx-xx-xx-xx</c>) and the
    /// Cisco dotted-triplet notation (<c>xxxx.xxxx.xxxx</c>), in either hex case. The separator must be used
    /// consistently, so a mixture such as <c>01:23-45:67:89:AB</c> is rejected. Leading and trailing whitespace
    /// is trimmed before validation.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = NetworkRules.IsMacAddress("01:23:45:67:89:AB"); // true
    /// bool valid = NetworkRules.IsMacAddress("0123.4567.89ab");    // true
    /// bool invalid = NetworkRules.IsMacAddress("01:23:45:67:89");  // false
    /// </code>
    /// </example>
    public static bool IsMacAddress(string? value)
    {
        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return IsGroupedHex(trimmed, MacOctetCount, MacOctetHexLength, MacColonSeparator)
            || IsGroupedHex(trimmed, MacOctetCount, MacOctetHexLength, MacHyphenSeparator)
            || IsGroupedHex(trimmed, MacCiscoGroupCount, MacCiscoGroupHexLength, MacDotSeparator);
    }

    private static bool IsGroupedHex(string value, int groupCount, int groupLength, char separator)
    {
        if (value.Length != groupCount * groupLength + groupCount - 1)
            return false;

        for (var index = 0; index < value.Length; index++)
        {
            var isSeparatorPosition = (index + 1) % (groupLength + 1) == 0;

            if (isSeparatorPosition
                ? value[index] != separator
                : !CharRules.IsHexDigit(value[index]))
            {
                return false;
            }
        }

        return true;
    }
}
