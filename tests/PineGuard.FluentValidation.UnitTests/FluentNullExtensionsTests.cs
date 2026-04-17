using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentNullExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public object? Value { get; init; } }

    private sealed class NotRequiredValidator : AbstractValidator<Model>
    {
        public NotRequiredValidator() => RuleFor(x => x.Value).NotRequired();
    }

    private sealed class RequiredValidator : AbstractValidator<Model>
    {
        public RequiredValidator() => RuleFor(x => x.Value).Required();
    }

    // FluentNullExtensions.NotRequired
    [Theory]
    [MemberData(nameof(FluentNullExtensionsTestData.NotRequired.Cases), MemberType = typeof(FluentNullExtensionsTestData.NotRequired))]
    public void NotRequired_BehavesAsExpected(FluentCase<object?> tc)
    {
        // Act
        var result = new NotRequiredValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentNullExtensions.Required
    [Theory]
    [MemberData(nameof(FluentNullExtensionsTestData.Required.Cases), MemberType = typeof(FluentNullExtensionsTestData.Required))]
    public void Required_BehavesAsExpected(FluentCase<object?> tc)
    {
        // Act
        var result = new RequiredValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
