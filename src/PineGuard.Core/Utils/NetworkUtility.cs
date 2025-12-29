using System.Net;
using System.Net.Sockets;

namespace PineGuard.Utils;

public static class NetworkUtility
{
    private const char Ipv4Separator = '.';
    private const int Ipv4SegmentCount = 4;
    private const int Ipv4SegmentMinLength = 1;
    private const int Ipv4SegmentMaxLength = 3;

    public static bool TryParseIpAddress(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return IPAddress.TryParse(trimmed, out ipAddress);
    }

    public static bool TryParseIpv4(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        // Strict dotted-quad segments 0..255
        var parts = trimmed.Split(Ipv4Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != Ipv4SegmentCount)
            return false;

        foreach (var part in parts)
        {
            if (part.Length is < Ipv4SegmentMinLength or > Ipv4SegmentMaxLength)
                return false;

            if (!byte.TryParse(part, out _))
                return false;
        }

        return IPAddress.TryParse(trimmed, out ipAddress) && ipAddress.AddressFamily == AddressFamily.InterNetwork;
    }

    public static bool TryParseIpv6(string? value, out IPAddress? ipAddress)
    {
        ipAddress = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return IPAddress.TryParse(trimmed, out ipAddress) && ipAddress.AddressFamily == AddressFamily.InterNetworkV6;
    }
}
