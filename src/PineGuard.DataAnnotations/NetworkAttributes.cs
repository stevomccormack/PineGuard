using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// IPAddress object validators

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid IP address
/// (either IPv4 or IPv6).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.IpAddress"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// For type-specific validation, use <see cref="Ipv4Attribute"/> or <see cref="Ipv6Attribute"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [IpAddress]
///     public string RemoteAddress { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Ipv4Attribute"/>
/// <seealso cref="Ipv6Attribute"/>
/// <seealso cref="MustNetworkClauses.IpAddress"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IpAddressAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Network.Address.Invalid, allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.IpAddress(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid IPv4 address
/// (e.g., <c>"192.168.1.1"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.Ipv4"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [Ipv4]
///     public string Ipv4Address { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Ipv6Attribute"/>
/// <seealso cref="IpAddressAttribute"/>
/// <seealso cref="MustNetworkClauses.Ipv4"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Ipv4Attribute() : ValidationAttributeBase(typeof(string), MustCodes.Network.Address.NotIpv4)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Ipv4(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid IPv6 address
/// (e.g., <c>"::1"</c>, <c>"2001:db8::1"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.Ipv6"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [Ipv6]
///     public string Ipv6Address { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Ipv4Attribute"/>
/// <seealso cref="IpAddressAttribute"/>
/// <seealso cref="MustNetworkClauses.Ipv6"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Ipv6Attribute() : ValidationAttributeBase(typeof(string), MustCodes.Network.Address.NotIpv6)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Ipv6(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

// Cidr, Hostname, Port

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is an IP address that falls
/// within the specified CIDR range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.InCidrRange"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [InCidrRange("192.168.0.0/24")]
///     public string AllowedIp { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustNetworkClauses.InCidrRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InCidrRangeAttribute(string cidr) : ValidationAttributeBase(typeof(string), MustCodes.Network.Cidr.OutOfRange)
{
    /// <summary>Gets the CIDR notation string (e.g., <c>"192.168.0.0/24"</c>) used to validate the IP range.</summary>
    public string Cidr { get; } = cidr;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.InCidrRange(strValue, Cidr, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid DNS hostname.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.Hostname"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [Hostname]
///     public string ServerName { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustNetworkClauses.Hostname"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HostnameAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Network.Hostname.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Hostname(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> property or field is a valid TCP/UDP port number
/// (1–65535).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNetworkClauses.PortNumber"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NetworkModel
/// {
///     [PortNumber]
///     public int ListenPort { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustNetworkClauses.PortNumber"/>
/// <seealso href="https://pineguard.ai/docs/annotations/network">Network Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PortNumberAttribute() : ValidationAttributeBase(typeof(int), MustCodes.Network.Port.Invalid)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var intValue = (int)value!;
        var result = Must.Be.PortNumber(intValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
