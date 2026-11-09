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

    private sealed class LongerThanValidator : AbstractValidator<Model>
    {
        public LongerThanValidator(TimeSpan threshold, Inclusion inclusion) =>
            RuleFor(x => x.Value).LongerThan(threshold, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.LongerThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = new LongerThanValidator(threshold, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }

    private sealed class ShorterThanValidator : AbstractValidator<Model>
    {
        public ShorterThanValidator(TimeSpan threshold, Inclusion inclusion) =>
            RuleFor(x => x.Value).ShorterThan(threshold, inclusion);
    }

    [Theory]
    [MemberData(nameof(FluentStringTimeSpanExtensionsTestData.ShorterThan.Cases), MemberType = typeof(FluentStringTimeSpanExtensionsTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(FluentCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = new ShorterThanValidator(threshold, inclusion).Validate(new Model { Value = value });

        // Assert
        AssertResult(tc, result);
    }
}
