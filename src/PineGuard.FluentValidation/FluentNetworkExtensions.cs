using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for network property validation including
/// IP addresses, CIDR ranges, hostnames, and port numbers.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/network">Fluent Network Extensions documentation</seealso>
public static class FluentNetworkExtensions
{
    /// <summary>Validates that the string value is a valid IP address (IPv4 or IPv6) that parses to <see cref="System.Net.IPAddress"/>.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.ServerIp).IpAddress();</code></example>
    public static IRuleBuilderOptions<TModel, string?> IpAddress<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.IpAddress(val, paramName: null),
            message, MustCodes.Network.Address.Invalid);

    /// <summary>Validates that the string value is not a valid IP address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpAddress<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpAddress(val, paramName: null),
            message, MustCodes.Network.Address.WellFormed);

    /// <summary>Validates that the string value is a valid IPv4 address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.Ipv4).Ipv4();</code></example>
    public static IRuleBuilderOptions<TModel, string?> Ipv4<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Ipv4(val, paramName: null),
            message, MustCodes.Network.Address.NotIpv4);

    /// <summary>Validates that the string value is not a valid IPv4 address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpv4<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpv4(val, paramName: null),
            message, MustCodes.Network.Address.Ipv4);

    /// <summary>Validates that the string value is a valid IPv6 address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.Ipv6Addr).Ipv6();</code></example>
    public static IRuleBuilderOptions<TModel, string?> Ipv6<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Ipv6(val, paramName: null),
            message, MustCodes.Network.Address.NotIpv6);

    /// <summary>Validates that the string value is not a valid IPv6 address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpv6<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpv6(val, paramName: null),
            message, MustCodes.Network.Address.Ipv6);

    /// <summary>Validates that the string value is a valid IP address string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> IpAddressString<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.IpAddressString(val, paramName: null),
            message, MustCodes.Network.Address.Invalid);

    /// <summary>Validates that the string value is not a valid IP address string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpAddressString<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpAddressString(val, paramName: null),
            message, MustCodes.Network.Address.WellFormed);

    /// <summary>Validates that the string value is a valid IPv4 string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> Ipv4String<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Ipv4String(val, paramName: null),
            message, MustCodes.Network.Address.NotIpv4);

    /// <summary>Validates that the string value is not a valid IPv4 string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpv4String<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpv4String(val, paramName: null),
            message, MustCodes.Network.Address.Ipv4);

    /// <summary>Validates that the string value is a valid IPv6 string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> Ipv6String<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Ipv6String(val, paramName: null),
            message, MustCodes.Network.Address.NotIpv6);

    /// <summary>Validates that the string value is not a valid IPv6 string representation.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotIpv6String<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotIpv6String(val, paramName: null),
            message, MustCodes.Network.Address.Ipv6);

    /// <summary>Validates that the IP address string falls within the specified CIDR range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="cidr">The CIDR notation range (e.g., "192.168.1.0/24").</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.ClientIp).InCidrRange("10.0.0.0/8");</code></example>
    public static IRuleBuilderOptions<TModel, string?> InCidrRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string cidr,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.InCidrRange(val, cidr, paramName: null),
            message, MustCodes.Network.Cidr.OutOfRange);

    /// <summary>Validates that the IP address string does not fall within the specified CIDR range.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="cidr">The CIDR notation range to exclude.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotInCidrRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string cidr,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotInCidrRange(val, cidr, paramName: null),
            message, MustCodes.Network.Cidr.InRange);

    /// <summary>Validates that the string value is a valid hostname.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.Host).Hostname();</code></example>
    public static IRuleBuilderOptions<TModel, string?> Hostname<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Hostname(val, paramName: null),
            message, MustCodes.Network.Hostname.Invalid);

    /// <summary>Validates that the string value is not a valid hostname.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, string?> NotHostname<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHostname(val, paramName: null),
            message, MustCodes.Network.Hostname.WellFormed);

    /// <summary>Validates that the nullable <see cref="int"/> value is a valid network port number (0-65535).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    /// <example><code>RuleFor(x => x.Port).PortNumber();</code></example>
    public static IRuleBuilderOptions<TModel, int?> PortNumber<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.PortNumber(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Network.Port.Invalid);

    /// <summary>Validates that the <see cref="int"/> value is a valid network port number (0-65535).</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, int> PortNumber<TModel>(this IRuleBuilder<TModel, int> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.PortNumber(val, paramName: null),
            message, MustCodes.Network.Port.Invalid);

    /// <summary>Validates that the nullable <see cref="int"/> value is not a valid network port number.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>If the value is <see langword="null"/>, validation passes.</remarks>
    public static IRuleBuilderOptions<TModel, int?> NotPortNumber<TModel>(this IRuleBuilder<TModel, int?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotPortNumber(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Network.Port.WellFormed);

    /// <summary>Validates that the <see cref="int"/> value is not a valid network port number.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    public static IRuleBuilderOptions<TModel, int> NotPortNumber<TModel>(this IRuleBuilder<TModel, int> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotPortNumber(val, paramName: null),
            message, MustCodes.Network.Port.WellFormed);

    /// <summary>Validates that the string value is a valid MAC address.</summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNetworkClauses.MacAddress"/>, which accepts the colon-separated,
    /// hyphen-separated and Cisco dotted forms in either case, but requires the separators to be consistent.
    /// If the value is <see langword="null"/>, validation passes.
    /// </remarks>
    /// <example><code>RuleFor(x => x.HardwareAddress).MacAddress();</code></example>
    /// <seealso cref="MustNetworkClauses.MacAddress"/>
    public static IRuleBuilderOptions<TModel, string?> MacAddress<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.MacAddress(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Network.Mac.Invalid);
}
