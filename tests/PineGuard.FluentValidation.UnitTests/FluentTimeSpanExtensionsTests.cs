using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTimeSpanExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public TimeSpan Value { get; init; } }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.InRange.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.InRange))]
    public void InRange_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).InRange(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.OutOfRange.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).OutOfRange(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.LongerThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).LongerThan(tc.Value.other, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentTimeSpanExtensionsTestData.ShorterThan.Cases), MemberType = typeof(FluentTimeSpanExtensionsTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(FluentCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).ShorterThan(tc.Value.other, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
