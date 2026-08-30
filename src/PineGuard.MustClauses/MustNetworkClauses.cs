using System.Net;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate network-related values such as IP addresses, hostnames, and ports,
/// delegating to <see cref="NetworkRules"/> for core validation logic.
/// </summary>
/// <seealso cref="NetworkRules"/>
/// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
public static class MustNetworkClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be a valid IP address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IP address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<IPAddress> IpAddress(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IPAddress>.Fail(MustCodes.Network.Address.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IP address.";

        var ok = NetworkUtility.TryParseIpAddress(value, out var ipAddress) && ipAddress is not null;
        return MustResult<IPAddress>.FromBool(ok, MustCodes.Network.Address.Invalid, messageTemplate, paramName, value, result: ipAddress!);
    }

    /// <summary>
    /// Validates that the specified value must be a valid IPv4 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IPv4 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<IPAddress> Ipv4(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IPAddress>.Fail(MustCodes.Network.Address.NotIpv4, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IPv4 address.";

        var ok = NetworkUtility.TryParseIpv4(value, out var ipAddress) && ipAddress is not null;
        return MustResult<IPAddress>.FromBool(ok, MustCodes.Network.Address.NotIpv4, messageTemplate, paramName, value, result: ipAddress!);
    }

    /// <summary>
    /// Validates that the specified value must be a valid IPv6 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IPv6 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<IPAddress> Ipv6(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IPAddress>.Fail(MustCodes.Network.Address.NotIpv6, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IPv6 address.";

        var ok = NetworkUtility.TryParseIpv6(value, out var ipAddress) && ipAddress is not null;
        return MustResult<IPAddress>.FromBool(ok, MustCodes.Network.Address.NotIpv6, messageTemplate, paramName, value, result: ipAddress!);
    }

    /// <summary>
    /// Validates that the specified value must be a valid IP address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IP address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> IpAddressString(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IP address.";

        var ok = NetworkRules.IsIpAddress(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.Invalid, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must be a valid IPv4 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IPv4 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> Ipv4String(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.NotIpv4, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IPv4 address.";

        var ok = NetworkRules.IsIpv4(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.NotIpv4, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must be a valid IPv6 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid IPv6 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> Ipv6String(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.NotIpv6, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid IPv6 address.";

        var ok = NetworkRules.IsIpv6(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.NotIpv6, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be null or whitespace.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="cidr">The CIDR notation range to check against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null or whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<IPAddress> InCidrRange(this IMustClause _,
        string? value,
        string cidr,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<IPAddress>.Fail(MustCodes.Network.Cidr.OutOfRange, NullMessage, paramName, value);

        if (string.IsNullOrWhiteSpace(cidr))
            return MustResult<IPAddress>.Fail(MustCodes.Network.Cidr.OutOfRange, "{paramName} must not be null or whitespace.", nameof(cidr), cidr);

        const string messageTemplate = "{paramName} must be within the specified CIDR range.";

        if (!NetworkUtility.TryParseIpAddress(value, out var address) || address is null)
            return MustResult<IPAddress>.FromBool(false, MustCodes.Network.Cidr.OutOfRange, messageTemplate, paramName, value, result: default);

        if (!NetworkUtility.TryParseCidr(cidr, out var network, out var prefixLength) || network is null)
            return MustResult<IPAddress>.FromBool(false, MustCodes.Network.Cidr.OutOfRange, messageTemplate, paramName, value, result: default);

        var ok = NetworkUtility.IsInCidr(address, network, prefixLength);
        return MustResult<IPAddress>.FromBool(ok, MustCodes.Network.Cidr.OutOfRange, messageTemplate, paramName, value, result: address);
    }

    /// <summary>
    /// Validates that the specified value must be a valid hostname.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid hostname."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> Hostname(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Hostname.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid hostname.";

        var ok = NetworkUtility.TryGetAsciiHostname(value, out var ascii) && NetworkRules.IsValidHostname(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Hostname.Invalid, messageTemplate, paramName, value, result: ascii!);
    }

    /// <summary>
    /// Validates that the specified value must be a valid port number.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid port number."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<int> PortNumber(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid port number.";

        var ok = NetworkRules.IsPortNumber(value);
        return MustResult<int>.FromBool(ok, MustCodes.Network.Port.Invalid, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a valid MAC address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="NetworkRules.IsMacAddress"/>, which accepts the colon-, hyphen- and
    /// Cisco dot-separated notations in either case. The failure message follows the pattern
    /// <c>"{paramName} must be a valid MAC address."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.MacAddress(adapterAddress);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="NetworkRules.IsMacAddress"/>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> MacAddress(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Mac.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid MAC address.";

        var ok = NetworkRules.IsMacAddress(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Mac.Invalid, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IP address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IP address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpAddress(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.WellFormed, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IP address.";

        var ok = !NetworkUtility.TryParseIpAddress(value, out var unused);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.WellFormed, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IPv4 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IPv4 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpv4(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.Ipv4, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IPv4 address.";

        var ok = !NetworkUtility.TryParseIpv4(value, out var unused);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.Ipv4, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IPv6 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IPv6 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpv6(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.Ipv6, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IPv6 address.";

        var ok = !NetworkUtility.TryParseIpv6(value, out var unused);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.Ipv6, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IP address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IP address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpAddressString(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.WellFormed, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IP address.";

        var ok = !NetworkRules.IsIpAddress(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.WellFormed, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IPv4 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IPv4 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpv4String(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.Ipv4, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IPv4 address.";

        var ok = !NetworkRules.IsIpv4(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.Ipv4, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid IPv6 address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid IPv6 address."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotIpv6String(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Address.Ipv6, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid IPv6 address.";

        var ok = !NetworkRules.IsIpv6(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Address.Ipv6, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be null or whitespace.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="cidr">The CIDR notation range to check against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be null or whitespace."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotInCidrRange(this IMustClause _,
        string? value,
        string cidr,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Cidr.InRange, NullMessage, paramName, value);

        if (string.IsNullOrWhiteSpace(cidr))
            return MustResult<string>.Fail(MustCodes.Network.Cidr.InRange, "{paramName} must not be null or whitespace.", nameof(cidr), cidr);

        const string messageTemplate = "{paramName} must not be within the specified CIDR range.";

        var ok = !NetworkRules.IsInCidr(value, cidr);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Cidr.InRange, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid hostname.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid hostname."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<string> NotHostname(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Network.Hostname.WellFormed, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid hostname.";

        var ok = !NetworkRules.IsValidHostname(value);
        return MustResult<string>.FromBool(ok, MustCodes.Network.Hostname.WellFormed, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid port number.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid port number."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/network">Network Must Clauses documentation</seealso>
    public static MustResult<int> NotPortNumber(this IMustClause _,
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a valid port number.";

        var ok = !NetworkRules.IsPortNumber(value);
        return MustResult<int>.FromBool(ok, MustCodes.Network.Port.WellFormed, messageTemplate, paramName, value, value);
    }
}
