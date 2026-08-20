using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace PineGuard.Utils;

/// <summary>
/// Provides network address parsing and validation utility methods (IPv4, IPv6, CIDR, MAC).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/network">Network Utility documentation</seealso>
public static class NetworkUtility
{
    private const char Ipv4Separator = '.';
    private const int Ipv4SegmentCount = 4;
    private const int Ipv4SegmentMinLength = 1;
    private const int Ipv4SegmentMaxLength = 3;

    /// <summary>
    /// Attempts to parse the specified string as an <see cref="IPAddress"/> of either family (IPv4 or IPv6).
    /// </summary>
    /// <param name="value">
    /// The string to parse. If <see langword="null"/> or whitespace, or if it is not a strict dotted-quad IPv4
    /// address or a standard IPv6 address, returns <see langword="false"/>. Non-canonical inet_aton shorthand
    /// (e.g. <c>"1"</c>, <c>"192.168.1"</c>) is rejected even though <c>IPAddress.TryParse</c> accepts it.
    /// </param>
    /// <param name="ipAddress">
    /// When this method returns, contains the parsed <see cref="IPAddress"/> if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseIpAddress(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (TryParseIpv4(trimmed, out ipAddress))
            return true;

        if (!IPAddress.TryParse(trimmed, out var parsed) || parsed.AddressFamily != AddressFamily.InterNetworkV6)
            return false;

        ipAddress = parsed;
        return true;
    }

    /// <summary>
    /// Attempts to parse the specified string as a strict dotted-quad IPv4 <see cref="IPAddress"/>.
    /// </summary>
    /// <param name="value">
    /// The string to parse. If <see langword="null"/> or whitespace, or if it does not consist of exactly
    /// four numeric segments in the range 0-255, returns <see langword="false"/>.
    /// </param>
    /// <param name="ipAddress">
    /// When this method returns, contains the parsed IPv4 <see cref="IPAddress"/> if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid dotted-quad IPv4 address; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseIpv4(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        // Strict dotted-quad segments 0..255
        var parts = trimmed.Split(Ipv4Separator, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != Ipv4SegmentCount)
            return false;

        foreach (var part in parts)
        {
            if (!IsValidIpv4Segment(part))
                return false;
        }

        if (!IPAddress.TryParse(trimmed, out var parsed) || parsed.AddressFamily != AddressFamily.InterNetwork)
            return false;

        ipAddress = parsed;
        return true;
    }

    /// <summary>
    /// Determines whether a single dotted-quad segment is a well-formed 0-255 value.
    /// </summary>
    /// <param name="part">The segment to validate.</param>
    /// <returns><see langword="true"/> if the segment is 1-3 digits and parses as a <see cref="byte"/>; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Internal rather than private so the zero-length guard can be exercised directly.
    /// <see cref="TryParseIpv4"/> splits with <see cref="StringSplitOptions.RemoveEmptyEntries"/>,
    /// so an empty segment is unreachable through that path.
    /// </remarks>
    internal static bool IsValidIpv4Segment(string part)
    {
        if (part.Length is < Ipv4SegmentMinLength or > Ipv4SegmentMaxLength)
            return false;

        return byte.TryParse(part, out _);
    }

    /// <summary>
    /// Attempts to parse the specified string as an IPv6 <see cref="IPAddress"/>.
    /// </summary>
    /// <param name="value">
    /// The string to parse. If <see langword="null"/> or whitespace, or if it does not represent an IPv6 address, returns <see langword="false"/>.
    /// </param>
    /// <param name="ipAddress">
    /// When this method returns, contains the parsed IPv6 <see cref="IPAddress"/> if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid IPv6 address; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseIpv6(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!TryParseIpAddress(value, out var parsed) || parsed is null)
            return false;

        if (parsed.AddressFamily != AddressFamily.InterNetworkV6)
            return false;

        ipAddress = parsed;
        return true;
    }

    /// <summary>
    /// Attempts to parse the specified string as CIDR notation (an IP address followed by a slash and a prefix length),
    /// masking the address down to its network portion.
    /// </summary>
    /// <param name="value">
    /// The string to parse, in the form <c>address/prefixLength</c>. If <see langword="null"/> or whitespace, malformed,
    /// or if the prefix length is out of range for the address family, returns <see langword="false"/>.
    /// </param>
    /// <param name="network">
    /// When this method returns, contains the network <see cref="IPAddress"/> with host bits masked to zero if successful;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <param name="prefixLength">
    /// When this method returns, contains the parsed prefix length if successful; otherwise, <c>0</c>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed as CIDR notation; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseCidr(string? value, out IPAddress? network, out int prefixLength)
    {
        network = null;
        prefixLength = 0;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        var slashIndex = trimmed.IndexOf('/');
        if (slashIndex <= 0 || slashIndex == trimmed.Length - 1)
            return false;

        var addressPart = trimmed[..slashIndex];
        var prefixPart = trimmed[(slashIndex + 1)..];

        if (!TryParseIpAddress(addressPart, out var parsed) || parsed is null)
            return false;

        if (!int.TryParse(prefixPart, NumberStyles.None, CultureInfo.InvariantCulture, out var prefix))
            return false;

        // NumberStyles.None disallows a leading sign, so `prefix` can never be negative here.
        var maxPrefix = parsed.AddressFamily == AddressFamily.InterNetworkV6 ? 128 : 32;
        if (prefix > maxPrefix)
            return false;

        var bytes = parsed.GetAddressBytes();
        ApplyPrefixMask(bytes, prefix);

        network = new IPAddress(bytes);
        prefixLength = prefix;
        return true;
    }

    /// <summary>
    /// Determines whether <paramref name="address"/> falls within the CIDR block described by <paramref name="network"/>
    /// and <paramref name="prefixLength"/>.
    /// </summary>
    /// <param name="address">The address to test.</param>
    /// <param name="network">The network address to test against. Must share the same <see cref="AddressFamily"/> as <paramref name="address"/>.</param>
    /// <param name="prefixLength">The CIDR prefix length, in bits. Must be within the valid range for the address family (0-32 for IPv4, 0-128 for IPv6).</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="address"/> and <paramref name="network"/> share the same address family,
    /// <paramref name="prefixLength"/> is in range, and <paramref name="address"/> lies within the network block; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsInCidr(IPAddress address, IPAddress network, int prefixLength)
    {
        if (address.AddressFamily != network.AddressFamily)
            return false;

        var maxPrefix = address.AddressFamily == AddressFamily.InterNetworkV6 ? 128 : 32;
        if (prefixLength < 0 || prefixLength > maxPrefix)
            return false;

        var addressBytes = address.GetAddressBytes();
        var networkBytes = network.GetAddressBytes();

        var bitsRemaining = prefixLength;

        for (var i = 0; i < addressBytes.Length; i++)
        {
            if (bitsRemaining <= 0)
                break;

            if (bitsRemaining >= 8)
            {
                if (addressBytes[i] != networkBytes[i])
                    return false;

                bitsRemaining -= 8;
                continue;
            }

            var mask = (byte)~((1 << (8 - bitsRemaining)) - 1);
            if ((addressBytes[i] & mask) == (networkBytes[i] & mask))
                break;

            return false;
        }

        return true;
    }

    /// <summary>
    /// Attempts to convert the specified hostname to its ASCII-compatible encoding (Punycode), applying IDN mapping
    /// for internationalized domain names.
    /// </summary>
    /// <param name="value">
    /// The hostname to convert. If <see langword="null"/> or whitespace, or if it reduces to an empty string after
    /// trimming a trailing root-domain dot, returns <see langword="false"/>.
    /// </param>
    /// <param name="asciiHostname">
    /// When this method returns, contains the ASCII-compatible hostname if successful; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully converted to ASCII; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAsciiHostname(string? value, out string? asciiHostname)
    {
        asciiHostname = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        // Only a single trailing root-domain dot is valid FQDN notation; consecutive trailing
        // dots denote an empty label and must be rejected rather than silently trimmed away.
        if (trimmed.EndsWith('.'))
            trimmed = trimmed[..^1];

        if (trimmed.Length == 0 || trimmed.EndsWith('.'))
            return false;

        try
        {
            var idn = new IdnMapping();
            asciiHostname = idn.GetAscii(trimmed);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    internal static bool ValidateHostnameLabels(string asciiHostname)
    {
        if (asciiHostname.Length > 253)
            return false;

        var labels = asciiHostname.Split('.', StringSplitOptions.RemoveEmptyEntries);

        return labels.All(IsValidHostnameLabel);
    }

    /// <remarks>
    /// Internal rather than private so the empty-label guard can be exercised directly.
    /// <see cref="ValidateHostnameLabels"/> splits with <see cref="StringSplitOptions.RemoveEmptyEntries"/>,
    /// so a zero-length label is unreachable through that path.
    /// </remarks>
    internal static bool IsValidHostnameLabel(string label)
    {
        if (label.Length is < 1 or > 63)
            return false;

        if (label.StartsWith('-') || label.EndsWith('-'))
            return false;

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var ch in label)
        {
            if (ch is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or '-')
                continue;

            return false;
        }

        return true;
    }

    private static void ApplyPrefixMask(byte[] bytes, int prefixLength)
    {
        var bitsRemaining = prefixLength;

        for (var i = 0; i < bytes.Length; i++)
        {
            switch (bitsRemaining)
            {
                case >= 8:
                    bitsRemaining -= 8;
                    continue;
                case <= 0:
                    bytes[i] = 0;
                    continue;
            }

            var mask = (byte)~((1 << (8 - bitsRemaining)) - 1);
            bytes[i] = (byte)(bytes[i] & mask);
            bitsRemaining = 0;
        }
    }
}
