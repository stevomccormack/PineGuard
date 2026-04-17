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

    public static bool TryParseIpAddress(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        return StringUtility.TryGetTrimmed(value, out var trimmed) && IPAddress.TryParse(trimmed, out ipAddress);
    }

    public static bool TryParseIpv4(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        // Strict dotted-quad segments 0..255
#if NET8_0_OR_GREATER
        var parts = trimmed.Split(Ipv4Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
        var parts = trimmed.Split(Ipv4Separator, StringSplitOptions.RemoveEmptyEntries);
#endif
        if (parts.Length != Ipv4SegmentCount)
            return false;

        foreach (var part in parts)
        {
            if (part.Length is < Ipv4SegmentMinLength or > Ipv4SegmentMaxLength)
                return false;

            if (!byte.TryParse(part, out _))
                return false;
        }

        if (!IPAddress.TryParse(trimmed, out var parsed) || parsed.AddressFamily != AddressFamily.InterNetwork)
            return false;

        ipAddress = parsed;
        return true;
    }

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

        if (!int.TryParse(prefixPart, out var prefix))
            return false;

        var maxPrefix = parsed.AddressFamily == AddressFamily.InterNetworkV6 ? 128 : 32;
        if (prefix < 0 || prefix > maxPrefix)
            return false;

        var bytes = parsed.GetAddressBytes();
        ApplyPrefixMask(bytes, prefix);

        network = new IPAddress(bytes);
        prefixLength = prefix;
        return true;
    }

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
            if ((addressBytes[i] & mask) != (networkBytes[i] & mask))
                return false;

            break;
        }

        return true;
    }

    public static bool TryGetAsciiHostname(string? value, out string? asciiHostname)
    {
        asciiHostname = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        trimmed = trimmed.TrimEnd('.');
        if (trimmed.Length == 0)
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

#if NET8_0_OR_GREATER
        var labels = asciiHostname.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
        var labels = asciiHostname.Split('.', StringSplitOptions.RemoveEmptyEntries);
#endif

        return labels.All(IsValidHostnameLabel);
    }

    private static bool IsValidHostnameLabel(string label)
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
