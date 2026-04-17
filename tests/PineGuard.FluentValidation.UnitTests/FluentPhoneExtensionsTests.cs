using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentPhoneExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class PhoneNumberValidator : AbstractValidator<Model>
    {
        public PhoneNumberValidator() => RuleFor(x => x.Value).PhoneNumber();
    }

    private sealed class PhoneNumberStringValidator : AbstractValidator<Model>
    {
        public PhoneNumberStringValidator() => RuleFor(x => x.Value).PhoneNumberString();
    }

    [Theory]
    [MemberData(nameof(FluentPhoneExtensionsTestData.PhoneNumber.Cases), MemberType = typeof(FluentPhoneExtensionsTestData.PhoneNumber))]
    public void PhoneNumber_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new PhoneNumberValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentPhoneExtensionsTestData.PhoneNumberString.Cases), MemberType = typeof(FluentPhoneExtensionsTestData.PhoneNumberString))]
    public void PhoneNumberString_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new PhoneNumberStringValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
