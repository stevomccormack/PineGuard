using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentBufferExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class HexValidator : AbstractValidator<Model>
    {
        public HexValidator() => RuleFor(x => x.Value).Hex();
    }

    private sealed class NotHexValidator : AbstractValidator<Model>
    {
        public NotHexValidator() => RuleFor(x => x.Value).NotHex();
    }

    private sealed class Base64Validator : AbstractValidator<Model>
    {
        public Base64Validator() => RuleFor(x => x.Value).Base64();
    }

    private sealed class NotBase64Validator : AbstractValidator<Model>
    {
        public NotBase64Validator() => RuleFor(x => x.Value).NotBase64();
    }

    private sealed class Base64UrlValidator : AbstractValidator<Model>
    {
        public Base64UrlValidator() => RuleFor(x => x.Value).Base64Url();
    }

    [Theory]
    [MemberData(nameof(FluentBufferExtensionsTestData.Hex.Cases), MemberType = typeof(FluentBufferExtensionsTestData.Hex))]
    public void Hex_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new HexValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBufferExtensionsTestData.NotHex.Cases), MemberType = typeof(FluentBufferExtensionsTestData.NotHex))]
    public void NotHex_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotHexValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBufferExtensionsTestData.Base64.Cases), MemberType = typeof(FluentBufferExtensionsTestData.Base64))]
    public void Base64_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new Base64Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentBufferExtensionsTestData.NotBase64.Cases), MemberType = typeof(FluentBufferExtensionsTestData.NotBase64))]
    public void NotBase64_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new NotBase64Validator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
