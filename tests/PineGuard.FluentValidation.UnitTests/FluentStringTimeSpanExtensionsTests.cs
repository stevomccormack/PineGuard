using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringTimeSpanExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class DurationBetweenValidator : AbstractValidator<Model>
    {
        public DurationBetweenValidator(TimeSpan min, TimeSpan max, Inclusion inclusion) =>
            RuleFor(x => x.Value).DurationBetween(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.DurationBetween.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(FluentCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new DurationBetweenValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class NotDurationBetweenValidator : AbstractValidator<Model>
    {
        public NotDurationBetweenValidator(TimeSpan min, TimeSpan max, Inclusion inclusion) =>
            RuleFor(x => x.Value).NotDurationBetween(min, max, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.NotDurationBetween.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(FluentCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = new NotDurationBetweenValidator(min, max, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class GreaterThanValidator : AbstractValidator<Model>
    {
        public GreaterThanValidator(TimeSpan threshold, Inclusion inclusion) =>
            RuleFor(x => x.Value).GreaterThan(threshold, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.GreaterThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = new GreaterThanValidator(threshold, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class LessThanValidator : AbstractValidator<Model>
    {
        public LessThanValidator(TimeSpan threshold, Inclusion inclusion) =>
            RuleFor(x => x.Value).LessThan(threshold, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.LessThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.LessThan))]
    public void LessThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = new LessThanValidator(threshold, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }
}
