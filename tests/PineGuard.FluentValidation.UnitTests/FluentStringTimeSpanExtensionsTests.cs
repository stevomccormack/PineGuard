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

    private sealed class LongerThanValidator : AbstractValidator<Model>
    {
        public LongerThanValidator(TimeSpan other, Inclusion inclusion) =>
            RuleFor(x => x.Value).LongerThanTimeSpan(other, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.LongerThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = new LongerThanValidator(other, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ShorterThanValidator : AbstractValidator<Model>
    {
        public ShorterThanValidator(TimeSpan other, Inclusion inclusion) =>
            RuleFor(x => x.Value).ShorterThanTimeSpan(other, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.ShorterThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = new ShorterThanValidator(other, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }
}
