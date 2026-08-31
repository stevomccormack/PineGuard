using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardDateOnlyClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDateOnlyClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.FutureOrPresent.ValidCases), MemberType = typeof(TD.FutureOrPresent))]
    [MemberData(nameof(TD.FutureOrPresent.InvalidCases), MemberType = typeof(TD.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(GuardCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.FutureOrPresent(value));
        AssertCustomMessage(tc, () => Guard.Against.FutureOrPresent(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Future.ValidCases), MemberType = typeof(TD.Future))]
    [MemberData(nameof(TD.Future.InvalidCases), MemberType = typeof(TD.Future))]
    public void Future_BehavesAsExpected(GuardCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Future(value));
        AssertCustomMessage(tc, () => Guard.Against.Future(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.PastOrPresent.ValidCases), MemberType = typeof(TD.PastOrPresent))]
    [MemberData(nameof(TD.PastOrPresent.InvalidCases), MemberType = typeof(TD.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(GuardCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PastOrPresent(value));
        AssertCustomMessage(tc, () => Guard.Against.PastOrPresent(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Past.ValidCases), MemberType = typeof(TD.Past))]
    [MemberData(nameof(TD.Past.InvalidCases), MemberType = typeof(TD.Past))]
    public void Past_BehavesAsExpected(GuardCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Past(value));
        AssertCustomMessage(tc, () => Guard.Against.Past(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Between.ValidCases), MemberType = typeof(TD.Between))]
    [MemberData(nameof(TD.Between.InvalidCases), MemberType = typeof(TD.Between))]
    public void Between_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.Between(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Between(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotBetween.ValidCases), MemberType = typeof(TD.NotBetween))]
    [MemberData(nameof(TD.NotBetween.InvalidCases), MemberType = typeof(TD.NotBetween))]
    public void NotBetween_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.OnOrAfter.ValidCases), MemberType = typeof(TD.OnOrAfter))]
    [MemberData(nameof(TD.OnOrAfter.InvalidCases), MemberType = typeof(TD.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.OnOrAfter(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.OnOrAfter(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.After.ValidCases), MemberType = typeof(TD.After))]
    [MemberData(nameof(TD.After.InvalidCases), MemberType = typeof(TD.After))]
    public void After_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.After(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.After(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.OnOrBefore.ValidCases), MemberType = typeof(TD.OnOrBefore))]
    [MemberData(nameof(TD.OnOrBefore.InvalidCases), MemberType = typeof(TD.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.OnOrBefore(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.OnOrBefore(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Before.ValidCases), MemberType = typeof(TD.Before))]
    [MemberData(nameof(TD.Before.InvalidCases), MemberType = typeof(TD.Before))]
    public void Before_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.Before(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.Before(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotSame.ValidCases), MemberType = typeof(TD.NotSame))]
    [MemberData(nameof(TD.NotSame.InvalidCases), MemberType = typeof(TD.NotSame))]
    public void NotSame_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotSame(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.NotSame(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.Same.ValidCases), MemberType = typeof(TD.Same))]
    [MemberData(nameof(TD.Same.InvalidCases), MemberType = typeof(TD.Same))]
    public void Same_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.Same(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.Same(value, tc.Value.other, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotChronological.ValidCases), MemberType = typeof(TD.NotChronological))]
    [MemberData(nameof(TD.NotChronological.InvalidCases), MemberType = typeof(TD.NotChronological))]
    public void NotChronological_BehavesAsExpected(GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)> tc)
    {
        var start = tc.Value.start;
        var result = AssertResult(tc, () => Guard.Against.NotChronological(start, tc.Value.end, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotChronological(start, tc.Value.end, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(start, result);
    }

    [Theory]
    [MemberData(nameof(TD.Chronological.ValidCases), MemberType = typeof(TD.Chronological))]
    [MemberData(nameof(TD.Chronological.InvalidCases), MemberType = typeof(TD.Chronological))]
    public void Chronological_BehavesAsExpected(GuardCase<(DateOnly start, DateOnly end, Inclusion inclusion)> tc)
    {
        var start = tc.Value.start;
        var result = AssertResult(tc, () => Guard.Against.Chronological(start, tc.Value.end, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Chronological(start, tc.Value.end, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(start, result);
    }

    [Theory]
    [MemberData(nameof(TD.Overlapping.ValidCases), MemberType = typeof(TD.Overlapping))]
    [MemberData(nameof(TD.Overlapping.InvalidCases), MemberType = typeof(TD.Overlapping))]
    public void Overlapping_BehavesAsExpected(GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)> tc)
    {
        var start1 = tc.Value.start1;
        var result = AssertResult(tc, () => Guard.Against.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(start1, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotOverlapping.ValidCases), MemberType = typeof(TD.NotOverlapping))]
    [MemberData(nameof(TD.NotOverlapping.InvalidCases), MemberType = typeof(TD.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(GuardCase<(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2, Inclusion inclusion)> tc)
    {
        var start1 = tc.Value.start1;
        var result = AssertResult(tc, () => Guard.Against.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(start1, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotWithinDays.ValidCases), MemberType = typeof(TD.NotWithinDays))]
    [MemberData(nameof(TD.NotWithinDays.InvalidCases), MemberType = typeof(TD.NotWithinDays))]
    public void NotWithinDays_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly reference, int days)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotWithinDays(value, tc.Value.reference, tc.Value.days));
        AssertCustomMessage(tc, () => Guard.Against.NotWithinDays(value, tc.Value.reference, tc.Value.days, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.WithinDays.ValidCases), MemberType = typeof(TD.WithinDays))]
    [MemberData(nameof(TD.WithinDays.InvalidCases), MemberType = typeof(TD.WithinDays))]
    public void WithinDays_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly reference, int days)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.WithinDays(value, tc.Value.reference, tc.Value.days));
        AssertCustomMessage(tc, () => Guard.Against.WithinDays(value, tc.Value.reference, tc.Value.days, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotWithinCalendarMonths.ValidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    [MemberData(nameof(TD.NotWithinCalendarMonths.InvalidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotWithinCalendarMonths(value, tc.Value.reference, tc.Value.months));
        AssertCustomMessage(tc, () => Guard.Against.NotWithinCalendarMonths(value, tc.Value.reference, tc.Value.months, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.WithinCalendarMonths.ValidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    [MemberData(nameof(TD.WithinCalendarMonths.InvalidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    public void WithinCalendarMonths_BehavesAsExpected(GuardCase<(DateOnly value, DateOnly reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.WithinCalendarMonths(value, tc.Value.reference, tc.Value.months));
        AssertCustomMessage(tc, () => Guard.Against.WithinCalendarMonths(value, tc.Value.reference, tc.Value.months, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.BelowMinimumAge.ValidCases), MemberType = typeof(TD.BelowMinimumAge))]
    [MemberData(nameof(TD.BelowMinimumAge.InvalidCases), MemberType = typeof(TD.BelowMinimumAge))]
    public void BelowMinimumAge_BehavesAsExpected(GuardCase<(DateOnly value, int years)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, FixedTimeProvider.Default));
        AssertCustomMessage(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, FixedTimeProvider.Default, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.BelowMinimumAgeOnLeapDay.Cases), MemberType = typeof(TD.BelowMinimumAgeOnLeapDay))]
    public void BelowMinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch(GuardCase<(DateOnly value, int years, DateTimeOffset utcNow)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, new FixedTimeProvider(tc.Value.utcNow)));
        AssertCustomMessage(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, new FixedTimeProvider(tc.Value.utcNow), message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
