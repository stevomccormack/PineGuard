using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentBoolExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public bool? Value { get; init; } }

    private sealed class TrueValidator : AbstractValidator<Model>
    {
        public TrueValidator() => RuleFor(x => x.Value).True();
    }

    private sealed class FalseValidator : AbstractValidator<Model>
    {
        public FalseValidator() => RuleFor(x => x.Value).False();
    }

    // FluentBoolExtensions.True
    [Theory]
    [MemberData(nameof(FluentBoolExtensionsTestData.True.Cases), MemberType = typeof(FluentBoolExtensionsTestData.True))]
    public void True_BehavesAsExpected(FluentCase<bool?> tc)
    {
        // Act
        var result = new TrueValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }

    // FluentBoolExtensions.False
    [Theory]
    [MemberData(nameof(FluentBoolExtensionsTestData.False.Cases), MemberType = typeof(FluentBoolExtensionsTestData.False))]
    public void False_BehavesAsExpected(FluentCase<bool?> tc)
    {
        // Act
        var result = new FalseValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
