using PineGuard.Utils;

namespace PineGuard.Rules;

public static class NetworkRules
{
    public static bool IsIpAddress(string? value) =>
        NetworkUtility.TryParseIpAddress(value, out _);

    public static bool IsIpv4(string? value) =>
        NetworkUtility.TryParseIpv4(value, out _);

    public static bool IsIpv6(string? value) =>
        NetworkUtility.TryParseIpv6(value, out _);
}
