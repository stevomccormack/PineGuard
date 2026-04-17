using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringBoolExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class TrueValidator : AbstractValidator<Model>
    {
        public TrueValidator() => RuleFor(x => x.Value).True();
    }

    private sealed class FalseValidator : AbstractValidator<Model>
    {
        public FalseValidator() => RuleFor(x => x.Value).False();
    }

    // FluentStringBoolExtensions.True
    [Theory]
    [MemberData(nameof(FluentStringBoolExtensionsTestData.True.Cases), MemberType = typeof(FluentStringBoolExtensionsTestData.True))]
    public void True_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new TrueValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentStringBoolExtensions.False
    [Theory]
    [MemberData(nameof(FluentStringBoolExtensionsTestData.False.Cases), MemberType = typeof(FluentStringBoolExtensionsTestData.False))]
    public void False_BehavesAsExpected(FluentCase<string?> tc)
    {
        // Act
        var result = new FalseValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
