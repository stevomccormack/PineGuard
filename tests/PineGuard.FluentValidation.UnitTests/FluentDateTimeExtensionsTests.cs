using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateTimeExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateTime Value { get; init; } }

    private sealed class PastValidator : AbstractValidator<Model> { public PastValidator() => RuleFor(x => x.Value).Past(); }
    private sealed class FutureValidator : AbstractValidator<Model> { public FutureValidator() => RuleFor(x => x.Value).Future(); }
    private sealed class PastOrPresentValidator : AbstractValidator<Model> { public PastOrPresentValidator() => RuleFor(x => x.Value).PastOrPresent(); }
    private sealed class FutureOrPresentValidator : AbstractValidator<Model> { public FutureOrPresentValidator() => RuleFor(x => x.Value).FutureOrPresent(); }
    private sealed class BetweenValidator : AbstractValidator<Model> { public BetweenValidator(DateTime min, DateTime max) => RuleFor(x => x.Value).Between(min, max); }
    private sealed class NotBetweenValidator : AbstractValidator<Model> { public NotBetweenValidator(DateTime min, DateTime max) => RuleFor(x => x.Value).NotBetween(min, max); }
    private sealed class BeforeValidator : AbstractValidator<Model> { public BeforeValidator(DateTime other) => RuleFor(x => x.Value).Before(other); }
    private sealed class OnOrBeforeValidator : AbstractValidator<Model> { public OnOrBeforeValidator(DateTime other) => RuleFor(x => x.Value).OnOrBefore(other); }
    private sealed class AfterValidator : AbstractValidator<Model> { public AfterValidator(DateTime other) => RuleFor(x => x.Value).After(other); }
    private sealed class OnOrAfterValidator : AbstractValidator<Model> { public OnOrAfterValidator(DateTime other) => RuleFor(x => x.Value).OnOrAfter(other); }
    private sealed class SameValidator : AbstractValidator<Model> { public SameValidator(DateTime other) => RuleFor(x => x.Value).Same(other); }
    private sealed class NotSameValidator : AbstractValidator<Model> { public NotSameValidator(DateTime other) => RuleFor(x => x.Value).NotSame(other); }
    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator(DateTime end) => RuleFor(x => x.Value).Chronological(end); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateTime end1, DateTime start2, DateTime end2) => RuleFor(x => x.Value).Overlapping(end1, start2, end2); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateTime end1, DateTime start2, DateTime end2) => RuleFor(x => x.Value).NotOverlapping(end1, start2, end2); }
    private sealed class WithinDaysFromNowValidator : AbstractValidator<Model> { public WithinDaysFromNowValidator(int days) => RuleFor(x => x.Value).WithinDaysFromNow(days); }
    private sealed class NotWithinDaysFromNowValidator : AbstractValidator<Model> { public NotWithinDaysFromNowValidator(int days) => RuleFor(x => x.Value).NotWithinDaysFromNow(days); }
    private sealed class WithinValidator : AbstractValidator<Model> { public WithinValidator(DateTime reference, TimeSpan window) => RuleFor(x => x.Value).Within(reference, window); }
    private sealed class NotWithinValidator : AbstractValidator<Model> { public NotWithinValidator(DateTime reference, TimeSpan window) => RuleFor(x => x.Value).NotWithin(reference, window); }
    private sealed class WithinCalendarMonthsValidator : AbstractValidator<Model> { public WithinCalendarMonthsValidator(DateTime reference, int months) => RuleFor(x => x.Value).WithinCalendarMonths(reference, months); }
    private sealed class NotWithinCalendarMonthsValidator : AbstractValidator<Model> { public NotWithinCalendarMonthsValidator(DateTime reference, int months) => RuleFor(x => x.Value).NotWithinCalendarMonths(reference, months); }
    private sealed class WeekdayValidator : AbstractValidator<Model> { public WeekdayValidator() => RuleFor(x => x.Value).Weekday(); }
    private sealed class WeekendValidator : AbstractValidator<Model> { public WeekendValidator() => RuleFor(x => x.Value).Weekend(); }
    private sealed class FirstDayOfMonthValidator : AbstractValidator<Model> { public FirstDayOfMonthValidator() => RuleFor(x => x.Value).FirstDayOfMonth(); }
    private sealed class NotFirstDayOfMonthValidator : AbstractValidator<Model> { public NotFirstDayOfMonthValidator() => RuleFor(x => x.Value).NotFirstDayOfMonth(); }
    private sealed class LastDayOfMonthValidator : AbstractValidator<Model> { public LastDayOfMonthValidator() => RuleFor(x => x.Value).LastDayOfMonth(); }
    private sealed class NotLastDayOfMonthValidator : AbstractValidator<Model> { public NotLastDayOfMonthValidator() => RuleFor(x => x.Value).NotLastDayOfMonth(); }
    private sealed class SameDayValidator : AbstractValidator<Model> { public SameDayValidator(DateTime other) => RuleFor(x => x.Value).SameDay(other); }
    private sealed class NotSameDayValidator : AbstractValidator<Model> { public NotSameDayValidator(DateTime other) => RuleFor(x => x.Value).NotSameDay(other); }
    private sealed class UtcValidator : AbstractValidator<Model> { public UtcValidator() => RuleFor(x => x.Value).Utc(); }
    private sealed class NotUtcValidator : AbstractValidator<Model> { public NotUtcValidator() => RuleFor(x => x.Value).NotUtc(); }
    private sealed class LocalValidator : AbstractValidator<Model> { public LocalValidator() => RuleFor(x => x.Value).Local(); }
    private sealed class NotLocalValidator : AbstractValidator<Model> { public NotLocalValidator() => RuleFor(x => x.Value).NotLocal(); }
    private sealed class UnspecifiedValidator : AbstractValidator<Model> { public UnspecifiedValidator() => RuleFor(x => x.Value).Unspecified(); }
    private sealed class NotUnspecifiedValidator : AbstractValidator<Model> { public NotUnspecifiedValidator() => RuleFor(x => x.Value).NotUnspecified(); }
    private sealed class ExplicitKindValidator : AbstractValidator<Model> { public ExplicitKindValidator() => RuleFor(x => x.Value).ExplicitKind(); }
    private sealed class NotExplicitKindValidator : AbstractValidator<Model> { public NotExplicitKindValidator() => RuleFor(x => x.Value).NotExplicitKind(); }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Past.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Past))]
    public void Past_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new PastValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Future.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Future))]
    public void Future_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new FutureValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.PastOrPresent.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new PastOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.FutureOrPresent.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new FutureOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Between.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Between))]
    public void Between_BehavesAsExpected(FluentCase<(DateTime value, DateTime min, DateTime max)> tc)
    {
        var result = new BetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotBetween.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(FluentCase<(DateTime value, DateTime min, DateTime max)> tc)
    {
        var result = new NotBetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Before.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Before))]
    public void Before_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new BeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.OnOrBefore.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new OnOrBeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.After.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.After))]
    public void After_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new AfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.OnOrAfter.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new OnOrAfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Same.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Same))]
    public void Same_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new SameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotSame.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotSame))]
    public void NotSame_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new NotSameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<(DateTime start, DateTime end)> tc)
    {
        var result = new ChronologicalValidator(tc.Value.end).Validate(new Model { Value = tc.Value.start });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)> tc)
    {
        var result = new OverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.WithinDaysFromNow.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.WithinDaysFromNow))]
    public void WithinDaysFromNow_BehavesAsExpected(FluentCase<(DateTime value, int days)> tc)
    {
        var result = new WithinDaysFromNowValidator(tc.Value.days).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotWithinDaysFromNow.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotWithinDaysFromNow))]
    public void NotWithinDaysFromNow_BehavesAsExpected(FluentCase<(DateTime value, int days)> tc)
    {
        var result = new NotWithinDaysFromNowValidator(tc.Value.days).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Within.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Within))]
    public void Within_BehavesAsExpected(FluentCase<(DateTime value, DateTime reference, TimeSpan window)> tc)
    {
        var result = new WithinValidator(tc.Value.reference, tc.Value.window).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotWithin.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotWithin))]
    public void NotWithin_BehavesAsExpected(FluentCase<(DateTime value, DateTime reference, TimeSpan window)> tc)
    {
        var result = new NotWithinValidator(tc.Value.reference, tc.Value.window).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.WithinCalendarMonths.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_BehavesAsExpected(FluentCase<(DateTime value, DateTime reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotWithinCalendarMonths.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_BehavesAsExpected(FluentCase<(DateTime value, DateTime reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Weekday.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Weekday))]
    public void Weekday_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new WeekdayValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Weekend.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Weekend))]
    public void Weekend_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new WeekendValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.FirstDayOfMonth.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.FirstDayOfMonth))]
    public void FirstDayOfMonth_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new FirstDayOfMonthValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotFirstDayOfMonth.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotFirstDayOfMonth))]
    public void NotFirstDayOfMonth_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotFirstDayOfMonthValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.LastDayOfMonth.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.LastDayOfMonth))]
    public void LastDayOfMonth_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new LastDayOfMonthValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotLastDayOfMonth.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotLastDayOfMonth))]
    public void NotLastDayOfMonth_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotLastDayOfMonthValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.SameDay.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.SameDay))]
    public void SameDay_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new SameDayValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotSameDay.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotSameDay))]
    public void NotSameDay_BehavesAsExpected(FluentCase<(DateTime value, DateTime other)> tc)
    {
        var result = new NotSameDayValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Utc.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Utc))]
    public void Utc_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new UtcValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotUtc.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotUtc))]
    public void NotUtc_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotUtcValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Local.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Local))]
    public void Local_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new LocalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotLocal.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotLocal))]
    public void NotLocal_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotLocalValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.Unspecified.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.Unspecified))]
    public void Unspecified_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new UnspecifiedValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotUnspecified.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotUnspecified))]
    public void NotUnspecified_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotUnspecifiedValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.ExplicitKind.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.ExplicitKind))]
    public void ExplicitKind_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new ExplicitKindValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeExtensionsTestData.NotExplicitKind.Cases), MemberType = typeof(FluentDateTimeExtensionsTestData.NotExplicitKind))]
    public void NotExplicitKind_BehavesAsExpected(FluentCase<DateTime> tc)
    {
        var result = new NotExplicitKindValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }
}
