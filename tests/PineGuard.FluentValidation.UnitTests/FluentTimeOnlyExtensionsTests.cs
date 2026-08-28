using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentTimeOnlyExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public TimeOnly? Value { get; init; } }
    private sealed record NonNullableModel { public TimeOnly Value { get; init; } }

    private static readonly TimeOnly Ref = new(12, 0);
    private static readonly TimeOnly RefMinus1 = Ref.AddHours(-1);
    private static readonly TimeOnly RefPlus1 = Ref.AddHours(1);

    private sealed class BetweenValidator : AbstractValidator<Model> { public BetweenValidator() => RuleFor(x => x.Value).Between(RefMinus1, RefPlus1); }
    private sealed class NotBetweenValidator : AbstractValidator<Model> { public NotBetweenValidator() => RuleFor(x => x.Value).NotBetween(RefMinus1, RefPlus1); }
    private sealed class BeforeValidator : AbstractValidator<Model> { public BeforeValidator() => RuleFor(x => x.Value).Before(Ref); }
    private sealed class NotBeforeValidator : AbstractValidator<Model> { public NotBeforeValidator() => RuleFor(x => x.Value).NotBefore(Ref); }
    private sealed class OnOrBeforeValidator : AbstractValidator<Model> { public OnOrBeforeValidator() => RuleFor(x => x.Value).OnOrBefore(Ref); }
    private sealed class NotOnOrBeforeValidator : AbstractValidator<Model> { public NotOnOrBeforeValidator() => RuleFor(x => x.Value).NotOnOrBefore(Ref); }
    private sealed class AfterValidator : AbstractValidator<Model> { public AfterValidator() => RuleFor(x => x.Value).After(Ref); }
    private sealed class NotAfterValidator : AbstractValidator<Model> { public NotAfterValidator() => RuleFor(x => x.Value).NotAfter(Ref); }
    private sealed class OnOrAfterValidator : AbstractValidator<Model> { public OnOrAfterValidator() => RuleFor(x => x.Value).OnOrAfter(Ref); }
    private sealed class NotOnOrAfterValidator : AbstractValidator<Model> { public NotOnOrAfterValidator() => RuleFor(x => x.Value).NotOnOrAfter(Ref); }
    private sealed class SameValidator : AbstractValidator<Model> { public SameValidator() => RuleFor(x => x.Value).Same(Ref); }
    private sealed class NotSameValidator : AbstractValidator<Model> { public NotSameValidator() => RuleFor(x => x.Value).NotSame(Ref); }
    private sealed class WithinValidator : AbstractValidator<Model> { public WithinValidator() => RuleFor(x => x.Value).Within(Ref, TimeSpan.FromMinutes(30)); }
    private sealed class NotWithinValidator : AbstractValidator<Model> { public NotWithinValidator() => RuleFor(x => x.Value).NotWithin(Ref, TimeSpan.FromMinutes(30)); }
    private sealed class ChronologicalValidator : AbstractValidator<Model> { public ChronologicalValidator() => RuleFor(x => x.Value).Chronological(Ref); }
    private sealed class NotChronologicalValidator : AbstractValidator<Model> { public NotChronologicalValidator() => RuleFor(x => x.Value).NotChronological(Ref); }
    private sealed class OverlappingValidator : AbstractValidator<Model> { public OverlappingValidator() => RuleFor(x => x.Value).Overlapping(new TimeOnly(12, 0), new TimeOnly(8, 0), new TimeOnly(9, 0)); }
    private sealed class NotOverlappingValidator : AbstractValidator<Model> { public NotOverlappingValidator() => RuleFor(x => x.Value).NotOverlapping(new TimeOnly(12, 0), new TimeOnly(8, 0), new TimeOnly(9, 0)); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Between.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Between))]
    public void Between_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new BetweenValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotBetween.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotBetweenValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Before.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Before))]
    public void Before_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new BeforeValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotBefore.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotBefore))]
    public void NotBefore_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotBeforeValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrBefore.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new OnOrBeforeValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOnOrBefore.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOnOrBefore))]
    public void NotOnOrBefore_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotOnOrBeforeValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.After.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.After))]
    public void After_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new AfterValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotAfter.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotAfter))]
    public void NotAfter_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotAfterValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrAfter.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new OnOrAfterValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOnOrAfter.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOnOrAfter))]
    public void NotOnOrAfter_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotOnOrAfterValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Same.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Same))]
    public void Same_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new SameValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotSame.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotSame))]
    public void NotSame_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotSameValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Within.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Within))]
    public void Within_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new WithinValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotWithin.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotWithin))]
    public void NotWithin_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotWithinValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Chronological.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Chronological))]
    public void Chronological_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new ChronologicalValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotChronological.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotChronological))]
    public void NotChronological_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotChronologicalValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.Overlapping.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new OverlappingValidator().Validate(new Model { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOverlapping.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(FluentCase<TimeOnly?> tc)
    { AssertResult(tc, new NotOverlappingValidator().Validate(new Model { Value = tc.Value })); }

    // ── Non-nullable validators ────────────────────────────────────────────

    private sealed class BetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public BetweenNonNullableValidator() => RuleFor(x => x.Value).Between(RefMinus1, RefPlus1); }
    private sealed class NotBetweenNonNullableValidator : AbstractValidator<NonNullableModel> { public NotBetweenNonNullableValidator() => RuleFor(x => x.Value).NotBetween(RefMinus1, RefPlus1); }
    private sealed class BeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public BeforeNonNullableValidator() => RuleFor(x => x.Value).Before(Ref); }
    private sealed class NotBeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public NotBeforeNonNullableValidator() => RuleFor(x => x.Value).NotBefore(Ref); }
    private sealed class OnOrBeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrBeforeNonNullableValidator() => RuleFor(x => x.Value).OnOrBefore(Ref); }
    private sealed class NotOnOrBeforeNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOnOrBeforeNonNullableValidator() => RuleFor(x => x.Value).NotOnOrBefore(Ref); }
    private sealed class AfterNonNullableValidator : AbstractValidator<NonNullableModel> { public AfterNonNullableValidator() => RuleFor(x => x.Value).After(Ref); }
    private sealed class NotAfterNonNullableValidator : AbstractValidator<NonNullableModel> { public NotAfterNonNullableValidator() => RuleFor(x => x.Value).NotAfter(Ref); }
    private sealed class OnOrAfterNonNullableValidator : AbstractValidator<NonNullableModel> { public OnOrAfterNonNullableValidator() => RuleFor(x => x.Value).OnOrAfter(Ref); }
    private sealed class NotOnOrAfterNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOnOrAfterNonNullableValidator() => RuleFor(x => x.Value).NotOnOrAfter(Ref); }
    private sealed class SameNonNullableValidator : AbstractValidator<NonNullableModel> { public SameNonNullableValidator() => RuleFor(x => x.Value).Same(Ref); }
    private sealed class NotSameNonNullableValidator : AbstractValidator<NonNullableModel> { public NotSameNonNullableValidator() => RuleFor(x => x.Value).NotSame(Ref); }
    private sealed class WithinNonNullableValidator : AbstractValidator<NonNullableModel> { public WithinNonNullableValidator() => RuleFor(x => x.Value).Within(Ref, TimeSpan.FromMinutes(30)); }
    private sealed class NotWithinNonNullableValidator : AbstractValidator<NonNullableModel> { public NotWithinNonNullableValidator() => RuleFor(x => x.Value).NotWithin(Ref, TimeSpan.FromMinutes(30)); }
    private sealed class ChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public ChronologicalNonNullableValidator() => RuleFor(x => x.Value).Chronological(Ref); }
    private sealed class NotChronologicalNonNullableValidator : AbstractValidator<NonNullableModel> { public NotChronologicalNonNullableValidator() => RuleFor(x => x.Value).NotChronological(Ref); }
    private sealed class OverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public OverlappingNonNullableValidator() => RuleFor(x => x.Value).Overlapping(new TimeOnly(12, 0), new TimeOnly(8, 0), new TimeOnly(9, 0)); }
    private sealed class NotOverlappingNonNullableValidator : AbstractValidator<NonNullableModel> { public NotOverlappingNonNullableValidator() => RuleFor(x => x.Value).NotOverlapping(new TimeOnly(12, 0), new TimeOnly(8, 0), new TimeOnly(9, 0)); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.BetweenNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.BetweenNonNullable))]
    public void BetweenNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new BetweenNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotBetweenNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotBetweenNonNullable))]
    public void NotBetweenNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotBetweenNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.BeforeNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.BeforeNonNullable))]
    public void BeforeNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new BeforeNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotBeforeNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotBeforeNonNullable))]
    public void NotBeforeNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotBeforeNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrBeforeNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrBeforeNonNullable))]
    public void OnOrBeforeNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new OnOrBeforeNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOnOrBeforeNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOnOrBeforeNonNullable))]
    public void NotOnOrBeforeNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotOnOrBeforeNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.AfterNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.AfterNonNullable))]
    public void AfterNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new AfterNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotAfterNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotAfterNonNullable))]
    public void NotAfterNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotAfterNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrAfterNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrAfterNonNullable))]
    public void OnOrAfterNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new OnOrAfterNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOnOrAfterNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOnOrAfterNonNullable))]
    public void NotOnOrAfterNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotOnOrAfterNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.SameNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.SameNonNullable))]
    public void SameNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new SameNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotSameNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotSameNonNullable))]
    public void NotSameNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotSameNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.WithinNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.WithinNonNullable))]
    public void WithinNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new WithinNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotWithinNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotWithinNonNullable))]
    public void NotWithinNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotWithinNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.ChronologicalNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.ChronologicalNonNullable))]
    public void ChronologicalNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new ChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotChronologicalNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotChronologicalNonNullable))]
    public void NotChronologicalNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotChronologicalNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OverlappingNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OverlappingNonNullable))]
    public void OverlappingNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new OverlappingNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.NotOverlappingNonNullable.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.NotOverlappingNonNullable))]
    public void NotOverlappingNonNullable_BehavesAsExpected(FluentCase<TimeOnly> tc)
    { AssertResult(tc, new NotOverlappingNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value })); }

    // ── Cross-property expression overloads ──────────────────────────

    private sealed record ComparisonModel { public TimeOnly Value { get; init; } public TimeOnly Other { get; init; } }
    private sealed record NullableComparisonModel { public TimeOnly? Value { get; init; } public TimeOnly Other { get; init; } }

    private sealed class BeforeExpressionValidator : AbstractValidator<ComparisonModel> { public BeforeExpressionValidator() => RuleFor(x => x.Value).Before(m => m.Other); }
    private sealed class BeforeNullableExpressionValidator : AbstractValidator<NullableComparisonModel> { public BeforeNullableExpressionValidator() => RuleFor(x => x.Value).Before(m => m.Other); }
    private sealed class OnOrBeforeExpressionValidator : AbstractValidator<ComparisonModel> { public OnOrBeforeExpressionValidator() => RuleFor(x => x.Value).OnOrBefore(m => m.Other); }
    private sealed class OnOrBeforeNullableExpressionValidator : AbstractValidator<NullableComparisonModel> { public OnOrBeforeNullableExpressionValidator() => RuleFor(x => x.Value).OnOrBefore(m => m.Other); }
    private sealed class AfterExpressionValidator : AbstractValidator<ComparisonModel> { public AfterExpressionValidator() => RuleFor(x => x.Value).After(m => m.Other); }
    private sealed class AfterNullableExpressionValidator : AbstractValidator<NullableComparisonModel> { public AfterNullableExpressionValidator() => RuleFor(x => x.Value).After(m => m.Other); }
    private sealed class OnOrAfterExpressionValidator : AbstractValidator<ComparisonModel> { public OnOrAfterExpressionValidator() => RuleFor(x => x.Value).OnOrAfter(m => m.Other); }
    private sealed class OnOrAfterNullableExpressionValidator : AbstractValidator<NullableComparisonModel> { public OnOrAfterNullableExpressionValidator() => RuleFor(x => x.Value).OnOrAfter(m => m.Other); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.BeforeExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.BeforeExpression))]
    public void BeforeExpression_BehavesAsExpected(FluentCase<(TimeOnly value, TimeOnly other)> tc)
    { AssertResult(tc, new BeforeExpressionValidator().Validate(new ComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.BeforeNullableExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.BeforeNullableExpression))]
    public void BeforeNullableExpression_BehavesAsExpected(FluentCase<(TimeOnly? value, TimeOnly other)> tc)
    { AssertResult(tc, new BeforeNullableExpressionValidator().Validate(new NullableComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrBeforeExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrBeforeExpression))]
    public void OnOrBeforeExpression_BehavesAsExpected(FluentCase<(TimeOnly value, TimeOnly other)> tc)
    { AssertResult(tc, new OnOrBeforeExpressionValidator().Validate(new ComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrBeforeNullableExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrBeforeNullableExpression))]
    public void OnOrBeforeNullableExpression_BehavesAsExpected(FluentCase<(TimeOnly? value, TimeOnly other)> tc)
    { AssertResult(tc, new OnOrBeforeNullableExpressionValidator().Validate(new NullableComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.AfterExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.AfterExpression))]
    public void AfterExpression_BehavesAsExpected(FluentCase<(TimeOnly value, TimeOnly other)> tc)
    { AssertResult(tc, new AfterExpressionValidator().Validate(new ComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.AfterNullableExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.AfterNullableExpression))]
    public void AfterNullableExpression_BehavesAsExpected(FluentCase<(TimeOnly? value, TimeOnly other)> tc)
    { AssertResult(tc, new AfterNullableExpressionValidator().Validate(new NullableComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrAfterExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrAfterExpression))]
    public void OnOrAfterExpression_BehavesAsExpected(FluentCase<(TimeOnly value, TimeOnly other)> tc)
    { AssertResult(tc, new OnOrAfterExpressionValidator().Validate(new ComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }

    [Theory]
    [MemberData(nameof(FluentTimeOnlyExtensionsTestData.OnOrAfterNullableExpression.Cases), MemberType = typeof(FluentTimeOnlyExtensionsTestData.OnOrAfterNullableExpression))]
    public void OnOrAfterNullableExpression_BehavesAsExpected(FluentCase<(TimeOnly? value, TimeOnly other)> tc)
    { AssertResult(tc, new OnOrAfterNullableExpressionValidator().Validate(new NullableComparisonModel { Value = tc.Value.value, Other = tc.Value.other })); }
}
