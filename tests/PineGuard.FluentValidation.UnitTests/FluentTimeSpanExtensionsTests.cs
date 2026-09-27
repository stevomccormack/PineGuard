using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTimeSpanExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public TimeSpan Value { get; init; } }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.DurationBetween.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).DurationBetween(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.NotDurationBetween.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotDurationBetween(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.LongerThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).LongerThan(tc.Value.threshold, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.ShorterThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).ShorterThan(tc.Value.threshold, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
