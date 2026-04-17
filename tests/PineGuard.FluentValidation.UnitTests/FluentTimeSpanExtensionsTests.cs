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
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.GreaterThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).GreaterThan(tc.Value.threshold, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.LessThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.LessThan))]
    public void LessThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).LessThan(tc.Value.threshold, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
