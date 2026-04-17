using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentDateTimeOffsetExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public DateTimeOffset? Value { get; init; } }
    private sealed record NonNullableModel { public DateTimeOffset Value { get; init; } }

    private sealed record ExpressionModel { public DateTimeOffset Start { get; init; } public DateTimeOffset End { get; init; } }
    private sealed record OverlapExpressionModel { public DateTimeOffset Start1 { get; init; } public DateTimeOffset End1 { get; init; } public DateTimeOffset Start2 { get; init; } public DateTimeOffset End2 { get; init; } }

    private sealed class PastValidator : AbstractValidator<Model> { public PastValidator() => RuleFor(x => x.Value).Past(); }
    private sealed class FutureValidator : AbstractValidator<Model> { public FutureValidator() => RuleFor(x => x.Value).Future(); }
    private sealed class PastOrPresentValidator : AbstractValidator<Model> { public PastOrPresentValidator() => RuleFor(x => x.Value).PastOrPresent(); }
    private sealed class FutureOrPresentValidator : AbstractValidator<Model> { public FutureOrPresentValidator() => RuleFor(x => x.Value).FutureOrPresent(); }
    private sealed class BetweenValidator : AbstractValidator<Model> { public BetweenValidator(DateTimeOffset min, DateTimeOffset max) => RuleFor(x => x.Value).Between(min, max); }
    private sealed class NotBetweenValidator : AbstractValidator<Model> { public NotBetweenValidator(DateTimeOffset min, DateTimeOffset max) => RuleFor(x => x.Value).NotBetween(min, max); }
    private sealed class BeforeValidator : AbstractValidator<Model> { public BeforeValidator(DateTimeOffset other) => RuleFor(x => x.Value).Before(other); }
    private sealed class OnOrBeforeValidator : AbstractValidator<Model> { public OnOrBeforeValidator(DateTimeOffset other) => RuleFor(x => x.Value).OnOrBefore(other); }
    private sealed class AfterValidator : AbstractValidator<Model> { public AfterValidator(DateTimeOffset other) => RuleFor(x => x.Value).After(other); }
    private sealed class OnOrAfterValidator : AbstractValidator<Model> { public OnOrAfterValidator(DateTimeOffset other) => RuleFor(x => x.Value).OnOrAfter(other); }
    private sealed class SameValidator : AbstractValidator<Model> { public SameValidator(DateTimeOffset other) => RuleFor(x => x.Value).Same(other); }
    private sealed class NotSameValidator : AbstractValidator<Model> { public NotSameValidator(DateTimeOffset other) => RuleFor(x => x.Value).NotSame(other); }
    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator(DateTimeOffset end) => RuleFor(x => x.Value).Chronological(end); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator(DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2) => RuleFor(x => x.Value).Overlapping(end1, start2, end2); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator(DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2) => RuleFor(x => x.Value).NotOverlapping(end1, start2, end2); }
    private sealed class WithinValidator : AbstractValidator<Model> { public WithinValidator(DateTimeOffset reference, TimeSpan window) => RuleFor(x => x.Value).Within(reference, window); }
    private sealed class NotWithinValidator : AbstractValidator<Model> { public NotWithinValidator(DateTimeOffset reference, TimeSpan window) => RuleFor(x => x.Value).NotWithin(reference, window); }
    private sealed class WithinCalendarMonthsValidator : AbstractValidator<Model> { public WithinCalendarMonthsValidator(DateTimeOffset reference, int months) => RuleFor(x => x.Value).WithinCalendarMonths(reference, months); }
    private sealed class NotWithinCalendarMonthsValidator : AbstractValidator<Model> { public NotWithinCalendarMonthsValidator(DateTimeOffset reference, int months) => RuleFor(x => x.Value).NotWithinCalendarMonths(reference, months); }
    private sealed class ChronologicalExpressionValidator : AbstractValidator<ExpressionModel> { public ChronologicalExpressionValidator() => RuleFor(x => x.Start).Chronological(m => m.End).WithName("Value"); }
    private sealed class OverlappingExpressionValidator : AbstractValidator<OverlapExpressionModel> { public OverlappingExpressionValidator() => RuleFor(x => x.Start1).Overlapping(m => m.End1, m => m.Start2, m => m.End2).WithName("Value"); }
    private sealed class NotOverlappingExpressionValidator : AbstractValidator<OverlapExpressionModel> { public NotOverlappingExpressionValidator() => RuleFor(x => x.Start1).NotOverlapping(m => m.End1, m => m.Start2, m => m.End2).WithName("Value"); }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Past.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Past))]
    public void Past_BehavesAsExpected(FluentCase<DateTimeOffset?> tc)
    {
        var result = new PastValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Future.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Future))]
    public void Future_BehavesAsExpected(FluentCase<DateTimeOffset?> tc)
    {
        var result = new FutureValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.PastOrPresent.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(FluentCase<DateTimeOffset?> tc)
    {
        var result = new PastOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.FutureOrPresent.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(FluentCase<DateTimeOffset?> tc)
    {
        var result = new FutureOrPresentValidator().Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Between.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Between))]
    public void Between_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max)> tc)
    {
        var result = new BetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotBetween.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max)> tc)
    {
        var result = new NotBetweenValidator(tc.Value.min, tc.Value.max).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Before.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Before))]
    public void Before_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new BeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OnOrBefore.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new OnOrBeforeValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.After.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.After))]
    public void After_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new AfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OnOrAfter.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new OnOrAfterValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Same.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Same))]
    public void Same_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new SameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotSame.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotSame))]
    public void NotSame_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset other)> tc)
    {
        var result = new NotSameValidator(tc.Value.other).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset end)> tc)
    {
        var result = new ChronologicalValidator(tc.Value.end).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<(DateTimeOffset? start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new OverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<(DateTimeOffset? start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new NotOverlappingValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new Model { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.Within.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.Within))]
    public void Within_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset reference, TimeSpan window)> tc)
    {
        var result = new WithinValidator(tc.Value.reference, tc.Value.window).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotWithin.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotWithin))]
    public void NotWithin_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset reference, TimeSpan window)> tc)
    {
        var result = new NotWithinValidator(tc.Value.reference, tc.Value.window).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.WithinCalendarMonths.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotWithinCalendarMonths.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_BehavesAsExpected(FluentCase<(DateTimeOffset? value, DateTimeOffset reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsValidator(tc.Value.reference, tc.Value.months).Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.ChronologicalExpression.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.ChronologicalExpression))]
    public void ChronologicalExpression_BehavesAsExpected(FluentCase<(DateTimeOffset start, DateTimeOffset end)> tc)
    {
        var result = new ChronologicalExpressionValidator().Validate(new ExpressionModel { Start = tc.Value.start, End = tc.Value.end });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OverlappingExpression.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OverlappingExpression))]
    public void OverlappingExpression_BehavesAsExpected(FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new OverlappingExpressionValidator().Validate(new OverlapExpressionModel { Start1 = tc.Value.start1, End1 = tc.Value.end1, Start2 = tc.Value.start2, End2 = tc.Value.end2 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotOverlappingExpression.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotOverlappingExpression))]
    public void NotOverlappingExpression_BehavesAsExpected(FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new NotOverlappingExpressionValidator().Validate(new OverlapExpressionModel { Start1 = tc.Value.start1, End1 = tc.Value.end1, Start2 = tc.Value.start2, End2 = tc.Value.end2 });
        AssertResult(tc, result);
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private sealed class PastNonNullableValidator : AbstractValidator<NonNullableModel> { public PastNonNullableValidator() => RuleFor(x => x.Value).Past(); }
    private sealed class PastOrPresentNonNullableValidator : AbstractValidator<NonNullableModel> { public PastOrPresentNonNullableValidator() => RuleFor(x => x.Value).PastOrPresent(); }
    private sealed class FutureNonNullableValidator : AbstractValidator<NonNullableModel> { public FutureNonNullableValidator() => RuleFor(x => x.Value).Future(); }
    private sealed class FutureOrPresentNonNullableValidator : AbstractValidator<NonNullableModel> { public FutureOrPresentNonNullableValidator() => RuleFor(x => x.Value).FutureOrPresent(); }
    private sealed class BetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public BetweenNonNullableValidator(DateTimeOffset min, DateTimeOffset max) => RuleFor(x => x.Value).Between(min, max); }
    private sealed class NotBetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public NotBetweenNonNullableValidator(DateTimeOffset min, DateTimeOffset max) => RuleFor(x => x.Value).NotBetween(min, max); }
    private sealed class BeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public BeforeNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).Before(other); }
    private sealed class OnOrBeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrBeforeNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).OnOrBefore(other); }
    private sealed class AfterNonNullableValidator : AbstractValidator<NonNullableModel> { public AfterNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).After(other); }
    private sealed class OnOrAfterNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrAfterNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).OnOrAfter(other); }
    private sealed class SameNonNullableValidator : AbstractValidator<NonNullableModel> { public SameNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).Same(other); }
    private sealed class NotSameNonNullableValidator : AbstractValidator<NonNullableModel> { public NotSameNonNullableValidator(DateTimeOffset other) => RuleFor(x => x.Value).NotSame(other); }
    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator(DateTimeOffset end) => RuleFor(x => x.Value).Chronological(end); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator(DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2) => RuleFor(x => x.Value).Overlapping(end1, start2, end2); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator(DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2) => RuleFor(x => x.Value).NotOverlapping(end1, start2, end2); }
    private sealed class WithinNonNullableValidator : AbstractValidator<NonNullableModel> { public WithinNonNullableValidator(DateTimeOffset reference, TimeSpan window) => RuleFor(x => x.Value).Within(reference, window); }
    private sealed class NotWithinNonNullableValidator : AbstractValidator<NonNullableModel> { public NotWithinNonNullableValidator(DateTimeOffset reference, TimeSpan window) => RuleFor(x => x.Value).NotWithin(reference, window); }
    private sealed class WithinCalendarMonthsNonNullableValidator : AbstractValidator<NonNullableModel> { public WithinCalendarMonthsNonNullableValidator(DateTimeOffset reference, int months) => RuleFor(x => x.Value).WithinCalendarMonths(reference, months); }
    private sealed class NotWithinCalendarMonthsNonNullableValidator : AbstractValidator<NonNullableModel> { public NotWithinCalendarMonthsNonNullableValidator(DateTimeOffset reference, int months) => RuleFor(x => x.Value).NotWithinCalendarMonths(reference, months); }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.PastNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.PastNonNullable))]
    public void PastNonNullable_BehavesAsExpected(FluentCase<DateTimeOffset> tc)
    {
        var result = new PastNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.PastOrPresentNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.PastOrPresentNonNullable))]
    public void PastOrPresentNonNullable_BehavesAsExpected(FluentCase<DateTimeOffset> tc)
    {
        var result = new PastOrPresentNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.FutureNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.FutureNonNullable))]
    public void FutureNonNullable_BehavesAsExpected(FluentCase<DateTimeOffset> tc)
    {
        var result = new FutureNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.FutureOrPresentNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.FutureOrPresentNonNullable))]
    public void FutureOrPresentNonNullable_BehavesAsExpected(FluentCase<DateTimeOffset> tc)
    {
        var result = new FutureOrPresentNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.BetweenNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.BetweenNonNullable))]
    public void BetweenNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max)> tc)
    {
        var result = new BetweenNonNullableValidator(tc.Value.min, tc.Value.max).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotBetweenNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotBetweenNonNullable))]
    public void NotBetweenNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max)> tc)
    {
        var result = new NotBetweenNonNullableValidator(tc.Value.min, tc.Value.max).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.BeforeNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.BeforeNonNullable))]
    public void BeforeNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new BeforeNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OnOrBeforeNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OnOrBeforeNonNullable))]
    public void OnOrBeforeNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new OnOrBeforeNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.AfterNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.AfterNonNullable))]
    public void AfterNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new AfterNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OnOrAfterNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OnOrAfterNonNullable))]
    public void OnOrAfterNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new OnOrAfterNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.SameNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.SameNonNullable))]
    public void SameNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new SameNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotSameNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotSameNonNullable))]
    public void NotSameNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset other)> tc)
    {
        var result = new NotSameNonNullableValidator(tc.Value.other).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset end)> tc)
    {
        var result = new ChronologicalNonNullableValidator(tc.Value.end).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new OverlappingNonNullableValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new NonNullableModel { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2)> tc)
    {
        var result = new NotOverlappingNonNullableValidator(tc.Value.end1, tc.Value.start2, tc.Value.end2).Validate(new NonNullableModel { Value = tc.Value.start1 });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.WithinNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.WithinNonNullable))]
    public void WithinNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)> tc)
    {
        var result = new WithinNonNullableValidator(tc.Value.reference, tc.Value.window).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotWithinNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotWithinNonNullable))]
    public void NotWithinNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)> tc)
    {
        var result = new NotWithinNonNullableValidator(tc.Value.reference, tc.Value.window).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.WithinCalendarMonthsNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.WithinCalendarMonthsNonNullable))]
    public void WithinCalendarMonthsNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset reference, int months)> tc)
    {
        var result = new WithinCalendarMonthsNonNullableValidator(tc.Value.reference, tc.Value.months).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentDateTimeOffsetExtensionsTestData.NotWithinCalendarMonthsNonNullable.Cases), MemberType = typeof(FluentDateTimeOffsetExtensionsTestData.NotWithinCalendarMonthsNonNullable))]
    public void NotWithinCalendarMonthsNonNullable_BehavesAsExpected(FluentCase<(DateTimeOffset value, DateTimeOffset reference, int months)> tc)
    {
        var result = new NotWithinCalendarMonthsNonNullableValidator(tc.Value.reference, tc.Value.months).Validate(new NonNullableModel { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
