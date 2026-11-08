using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringTimeSpanExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class InRangeValidator : AbstractValidator<Model>
    {
        public InRangeValidator(TimeSpan min, TimeSpan max, Inclusion inclusion) =>
            RuleFor(x => x.Value).InRangeTimeSpan(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.InRange.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.InRange))]
    public void InRange_BehavesAsExpected(FluentCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new InRangeValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class OutOfRangeValidator : AbstractValidator<Model>
    {
        public OutOfRangeValidator(TimeSpan min, TimeSpan max, Inclusion inclusion) =>
            RuleFor(x => x.Value).OutOfRangeTimeSpan(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.OutOfRange.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(FluentCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new OutOfRangeValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class GreaterThanValidator : AbstractValidator<Model>
    {
        public GreaterThanValidator(TimeSpan other, Inclusion inclusion) =>
            RuleFor(x => x.Value).GreaterThanTimeSpan(other, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.GreaterThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = new GreaterThanValidator(other, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class LessThanValidator : AbstractValidator<Model>
    {
        public LessThanValidator(TimeSpan other, Inclusion inclusion) =>
            RuleFor(x => x.Value).LessThanTimeSpan(other, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.LessThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.LessThan))]
    public void LessThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = new LessThanValidator(other, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }
}
