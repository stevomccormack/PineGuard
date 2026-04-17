using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateOnlyExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateOnly? Value { get; init; } }
    private sealed record NonNullableModel { public DateOnly Value { get; init; } }

    private sealed class PastValidator : AbstractValidator<Model> { public PastValidator() => RuleFor(x => x.Value).Past(); }
    private sealed class PastOrPresentValidator : AbstractValidator<Model> { public PastOrPresentValidator() => RuleFor(x => x.Value).PastOrPresent(); }
    private sealed class FutureValidator : AbstractValidator<Model> { public FutureValidator() => RuleFor(x => x.Value).Future(); }
    private sealed class FutureOrPresentValidator : AbstractValidator<Model> { public FutureOrPresentValidator() => RuleFor(x => x.Value).FutureOrPresent(); }
    private sealed class BetweenValidator : AbstractValidator<Model> { public BetweenValidator(DateOnly min, DateOnly max) => RuleFor(x => x.Value).Between(min, max); }
    private sealed class NotBetweenValidator : AbstractValidator<Model> { public NotBetweenValidator(DateOnly min, DateOnly max) => RuleFor(x => x.Value).NotBetween(min, max); }
    private sealed class BeforeValidator : AbstractValidator<Model> { public BeforeValidator(DateOnly other) => RuleFor(x => x.Value).Before(other); }
    private sealed class OnOrBeforeValidator : AbstractValidator<Model> { public OnOrBeforeValidator(DateOnly other) => RuleFor(x => x.Value).OnOrBefore(other); }
    private sealed class AfterValidator : AbstractValidator<Model> { public AfterValidator(DateOnly other) => RuleFor(x => x.Value).After(other); }
    private sealed class OnOrAfterValidator : AbstractValidator<Model> { public OnOrAfterValidator(DateOnly other) => RuleFor(x => x.Value).OnOrAfter(other); }
    private sealed class SameValidator : AbstractValidator<Model> { public SameValidator(DateOnly other) => RuleFor(x => x.Value).Same(other); }
    private sealed class NotSameValidator : AbstractValidator<Model> { public NotSameValidator(DateOnly other) => RuleFor(x => x.Value).NotSame(other); }
    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator(DateOnly end) => RuleFor(x => x.Value).Chronological(end); }
    private sealed class NotChronologicalValidator : AbstractValidator<Model> { public NotChronologicalValidator(DateOnly end) => RuleFor(x => x.Value).NotChronological(end); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateOnly end1, DateOnly start2, DateOnly end2) => RuleFor(x => x.Value).Overlapping(end1, start2, end2); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateOnly end1, DateOnly start2, DateOnly end2) => RuleFor(x => x.Value).NotOverlapping(end1, start2, end2); }
    private sealed class WithinDaysValidator : AbstractValidator<Model> { public WithinDaysValidator(DateOnly reference, int days) => RuleFor(x => x.Value).WithinDays(reference, days); }
    private sealed class NotWithinDaysValidator : AbstractValidator<Model> { public NotWithinDaysValidator(DateOnly reference, int days) => RuleFor(x => x.Value).NotWithinDays(reference, days); }
    private sealed class WithinCalendarMonthsValidator : AbstractValidator<Model> { public WithinCalendarMonthsValidator(DateOnly reference, int months) => RuleFor(x => x.Value).WithinCalendarMonths(reference, months); }
    private sealed class NotWithinCalendarMonthsValidator : AbstractValidator<Model> { public NotWithinCalendarMonthsValidator(DateOnly reference, int months) => RuleFor(x => x.Value).NotWithinCalendarMonths(reference, months); }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Past.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Past))]
    public void Past_BehavesAsExpected(FluentCase<DateOnly?> tc)
    {
        var result = new PastValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.PastOrPresent.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(FluentCase<DateOnly?> tc)
    {
        var result = new PastOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Future.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Future))]
    public void Future_BehavesAsExpected(FluentCase<DateOnly?> tc)
    {
        var result = new FutureValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.FutureOrPresent.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(FluentCase<DateOnly?> tc)
    {
        var result = new FutureOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Between.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Between))]
    public void Between_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly min, DateOnly max)> tc)
    {
        var result = new BetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotBetween.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly min, DateOnly max)> tc)
    {
        var result = new NotBetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Before.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Before))]
    public void Before_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new BeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.OnOrBefore.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new OnOrBeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.After.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.After))]
    public void After_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new AfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.OnOrAfter.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new OnOrAfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Same.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Same))]
    public void Same_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new SameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotSame.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotSame))]
    public void NotSame_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly other)> tc)
    {
        var result = new NotSameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly end)> tc)
    {
        var result = new ChronologicalValidator(tc.Value.end).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotChronological.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotChronological))]
    public void NotChronological_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly end)> tc)
    {
        var result = new NotChronologicalValidator(tc.Value.end).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)> tc)
    {
        var result = new OverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly end1, DateOnly start2, DateOnly end2)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.WithinDays.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.WithinDays))]
    public void WithinDays_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly reference, int days)> tc)
    {
        var result = new WithinDaysValidator(tc.Value.reference, tc.Value.days).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotWithinDays.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotWithinDays))]
    public void NotWithinDays_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly reference, int days)> tc)
    {
        var result = new NotWithinDaysValidator(tc.Value.reference, tc.Value.days).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.WithinCalendarMonths.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotWithinCalendarMonths.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_BehavesAsExpected(FluentCase<(DateOnly? value, DateOnly reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private sealed class PastNonNullableValidator : AbstractValidator<NonNullableModel> { public PastNonNullableValidator() => RuleFor(x => x.Value).Past(); }
    private sealed class PastOrPresentNonNullableValidator : AbstractValidator<NonNullableModel> { public PastOrPresentNonNullableValidator() => RuleFor(x => x.Value).PastOrPresent(); }
    private sealed class FutureNonNullableValidator : AbstractValidator<NonNullableModel> { public FutureNonNullableValidator() => RuleFor(x => x.Value).Future(); }
    private sealed class FutureOrPresentNonNullableValidator : AbstractValidator<NonNullableModel> { public FutureOrPresentNonNullableValidator() => RuleFor(x => x.Value).FutureOrPresent(); }
    private sealed class BetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public BetweenNonNullableValidator(DateOnly min, DateOnly max) => RuleFor(x => x.Value).Between(min, max); }
    private sealed class NotBetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public NotBetweenNonNullableValidator(DateOnly min, DateOnly max) => RuleFor(x => x.Value).NotBetween(min, max); }
    private sealed class BeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public BeforeNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).Before(other); }
    private sealed class OnOrBeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrBeforeNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).OnOrBefore(other); }
    private sealed class AfterNonNullableValidator : AbstractValidator<NonNullableModel> { public AfterNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).After(other); }
    private sealed class OnOrAfterNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrAfterNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).OnOrAfter(other); }
    private sealed class SameNonNullableValidator : AbstractValidator<NonNullableModel> { public SameNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).Same(other); }
    private sealed class NotSameNonNullableValidator : AbstractValidator<NonNullableModel> { public NotSameNonNullableValidator(DateOnly other) => RuleFor(x => x.Value).NotSame(other); }
    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator(DateOnly end) => RuleFor(x => x.Value).Chronological(end); }
    private sealed class NotChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public NotChronologicalNonNullableValidator(DateOnly end) => RuleFor(x => x.Value).NotChronological(end); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(DateOnly end1, DateOnly start2, DateOnly end2) => RuleFor(x => x.Value).Overlapping(end1, start2, end2); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(DateOnly end1, DateOnly start2, DateOnly end2) => RuleFor(x => x.Value).NotOverlapping(end1, start2, end2); }
    private sealed class WithinDaysNonNullableValidator : AbstractValidator<NonNullableModel> { public WithinDaysNonNullableValidator(DateOnly reference, int days) => RuleFor(x => x.Value).WithinDays(reference, days); }
    private sealed class NotWithinDaysNonNullableValidator : AbstractValidator<NonNullableModel> { public NotWithinDaysNonNullableValidator(DateOnly reference, int days) => RuleFor(x => x.Value).NotWithinDays(reference, days); }
    private sealed class WithinCalendarMonthsNonNullableValidator : AbstractValidator<NonNullableModel> { public WithinCalendarMonthsNonNullableValidator(DateOnly reference, int months) => RuleFor(x => x.Value).WithinCalendarMonths(reference, months); }
    private sealed class NotWithinCalendarMonthsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotWithinCalendarMonthsNonNullableValidator(DateOnly reference, int months) => RuleFor(x => x.Value).NotWithinCalendarMonths(reference, months); }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.PastNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.PastNonNullable))]
    public void PastNonNullable_BehavesAsExpected(FluentCase<DateOnly> tc)
    {
        var result = new PastNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.PastOrPresentNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.PastOrPresentNonNullable))]
    public void PastOrPresentNonNullable_BehavesAsExpected(FluentCase<DateOnly> tc)
    {
        var result = new PastOrPresentNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.FutureNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.FutureNonNullable))]
    public void FutureNonNullable_BehavesAsExpected(FluentCase<DateOnly> tc)
    {
        var result = new FutureNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.FutureOrPresentNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.FutureOrPresentNonNullable))]
    public void FutureOrPresentNonNullable_BehavesAsExpected(FluentCase<DateOnly> tc)
    {
        var result = new FutureOrPresentNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.BetweenNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.BetweenNonNullable))]
    public void BetweenNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly min, DateOnly max)> tc)
    {
        var result = new BetweenNonNullableValidator(tc.Value.min, tc.Value.max).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotBetweenNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotBetweenNonNullable))]
    public void NotBetweenNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly min, DateOnly max)> tc)
    {
        var result = new NotBetweenNonNullableValidator(tc.Value.min, tc.Value.max).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.BeforeNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.BeforeNonNullable))]
    public void BeforeNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new BeforeNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.OnOrBeforeNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.OnOrBeforeNonNullable))]
    public void OnOrBeforeNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new OnOrBeforeNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.AfterNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.AfterNonNullable))]
    public void AfterNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new AfterNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.OnOrAfterNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.OnOrAfterNonNullable))]
    public void OnOrAfterNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new OnOrAfterNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.SameNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.SameNonNullable))]
    public void SameNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new SameNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotSameNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotSameNonNullable))]
    public void NotSameNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly other)> tc)
    {
        var result = new NotSameNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly end)> tc)
    {
        var result = new ChronologicalNonNullableValidator(tc.Value.end).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotChronologicalNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotChronologicalNonNullable))]
    public void NotChronologicalNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly end)> tc)
    {
        var result = new NotChronologicalNonNullableValidator(tc.Value.end).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly end1, DateOnly start2, DateOnly end2)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.WithinDaysNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.WithinDaysNonNullable))]
    public void WithinDaysNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly reference, int days)> tc)
    {
        var result = new WithinDaysNonNullableValidator(tc.Value.reference, tc.Value.days).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotWithinDaysNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotWithinDaysNonNullable))]
    public void NotWithinDaysNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly reference, int days)> tc)
    {
        var result = new NotWithinDaysNonNullableValidator(tc.Value.reference, tc.Value.days).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.WithinCalendarMonthsNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.WithinCalendarMonthsNonNullable))]
    public void WithinCalendarMonthsNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsNonNullableValidator(tc.Value.reference, tc.Value.months).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateOnlyExtensionsTestData.NotWithinCalendarMonthsNonNullable.Cases), MemberType = typeof(FluentDateOnlyExtensionsTestData.NotWithinCalendarMonthsNonNullable))]
    public void NotWithinCalendarMonthsNonNullable_BehavesAsExpected(FluentCase<(DateOnly value, DateOnly reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsNonNullableValidator(tc.Value.reference, tc.Value.months).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
