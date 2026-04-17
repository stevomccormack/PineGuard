using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentNetworkExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class IpAddressValidator : AbstractValidator<Model>
    {
        public IpAddressValidator() => RuleFor(x => x.Value).IpAddress();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.IpAddress.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.IpAddress))]
    public void IpAddress_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new IpAddressValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpAddressValidator : AbstractValidator<Model>
    {
        public NotIpAddressValidator() => RuleFor(x => x.Value).NotIpAddress();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpAddress.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpAddress))]
    public void NotIpAddress_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpAddressValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class Ipv4Validator : AbstractValidator<Model>
    {
        public Ipv4Validator() => RuleFor(x => x.Value).Ipv4();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.Ipv4.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.Ipv4))]
    public void Ipv4_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new Ipv4Validator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpv4Validator : AbstractValidator<Model>
    {
        public NotIpv4Validator() => RuleFor(x => x.Value).NotIpv4();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpv4.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpv4))]
    public void NotIpv4_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpv4Validator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class Ipv6Validator : AbstractValidator<Model>
    {
        public Ipv6Validator() => RuleFor(x => x.Value).Ipv6();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.Ipv6.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.Ipv6))]
    public void Ipv6_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new Ipv6Validator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpv6Validator : AbstractValidator<Model>
    {
        public NotIpv6Validator() => RuleFor(x => x.Value).NotIpv6();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpv6.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpv6))]
    public void NotIpv6_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpv6Validator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class IpAddressStringValidator : AbstractValidator<Model>
    {
        public IpAddressStringValidator() => RuleFor(x => x.Value).IpAddressString();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.IpAddressString.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.IpAddressString))]
    public void IpAddressString_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new IpAddressStringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpAddressStringValidator : AbstractValidator<Model>
    {
        public NotIpAddressStringValidator() => RuleFor(x => x.Value).NotIpAddressString();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpAddressString.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpAddressString))]
    public void NotIpAddressString_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpAddressStringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class Ipv4StringValidator : AbstractValidator<Model>
    {
        public Ipv4StringValidator() => RuleFor(x => x.Value).Ipv4String();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.Ipv4String.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.Ipv4String))]
    public void Ipv4String_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new Ipv4StringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpv4StringValidator : AbstractValidator<Model>
    {
        public NotIpv4StringValidator() => RuleFor(x => x.Value).NotIpv4String();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpv4String.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpv4String))]
    public void NotIpv4String_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpv4StringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class Ipv6StringValidator : AbstractValidator<Model>
    {
        public Ipv6StringValidator() => RuleFor(x => x.Value).Ipv6String();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.Ipv6String.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.Ipv6String))]
    public void Ipv6String_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new Ipv6StringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotIpv6StringValidator : AbstractValidator<Model>
    {
        public NotIpv6StringValidator() => RuleFor(x => x.Value).NotIpv6String();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotIpv6String.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotIpv6String))]
    public void NotIpv6String_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotIpv6StringValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class HostnameValidator : AbstractValidator<Model>
    {
        public HostnameValidator() => RuleFor(x => x.Value).Hostname();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.Hostname.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.Hostname))]
    public void Hostname_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new HostnameValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotHostnameValidator : AbstractValidator<Model>
    {
        public NotHostnameValidator() => RuleFor(x => x.Value).NotHostname();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotHostname.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotHostname))]
    public void NotHostname_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotHostnameValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class InCidrRangeValidator : AbstractValidator<Model>
    {
        public InCidrRangeValidator(string cidr) => RuleFor(x => x.Value).InCidrRange(cidr);
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.InCidrRange.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.InCidrRange))]
    public void InCidrRange_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new InCidrRangeValidator(FluentNetworkExtensionsTestData.InCidrRange.Cidr).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotInCidrRangeValidator : AbstractValidator<Model>
    {
        public NotInCidrRangeValidator(string cidr) => RuleFor(x => x.Value).NotInCidrRange(cidr);
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotInCidrRange.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotInCidrRange))]
    public void NotInCidrRange_BehavesAsExpected(FluentCase<string?> tc)
    {
        var result = new NotInCidrRangeValidator(FluentNetworkExtensionsTestData.NotInCidrRange.Cidr).Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed record IntModel { public int Value { get; init; } }

    private sealed class PortNumberValidator : AbstractValidator<IntModel>
    {
        public PortNumberValidator() => RuleFor(x => x.Value).PortNumber();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.PortNumber.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.PortNumber))]
    public void PortNumber_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new PortNumberValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotPortNumberValidator : AbstractValidator<IntModel>
    {
        public NotPortNumberValidator() => RuleFor(x => x.Value).NotPortNumber();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotPortNumber.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotPortNumber))]
    public void NotPortNumber_BehavesAsExpected(FluentCase<int> tc)
    {
        var result = new NotPortNumberValidator().Validate(new IntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed record NullableIntModel { public int? Value { get; init; } }

    private sealed class PortNumberNullableValidator : AbstractValidator<NullableIntModel>
    {
        public PortNumberNullableValidator() => RuleFor(x => x.Value).PortNumber();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.PortNumberNullable.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.PortNumberNullable))]
    public void PortNumberNullable_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new PortNumberNullableValidator().Validate(new NullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    private sealed class NotPortNumberNullableValidator : AbstractValidator<NullableIntModel>
    {
        public NotPortNumberNullableValidator() => RuleFor(x => x.Value).NotPortNumber();
    }

    [Theory]
    [MemberData(nameof(FluentNetworkExtensionsTestData.NotPortNumberNullable.Cases), MemberType = typeof(FluentNetworkExtensionsTestData.NotPortNumberNullable))]
    public void NotPortNumberNullable_BehavesAsExpected(FluentCase<int?> tc)
    {
        var result = new NotPortNumberNullableValidator().Validate(new NullableIntModel { Value = tc.Value });
        AssertResult(tc, result);
    }
}
