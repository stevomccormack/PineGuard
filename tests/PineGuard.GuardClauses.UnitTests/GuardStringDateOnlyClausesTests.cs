using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringDateOnlyClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // GuardStringDateOnlyClauses.FutureOrPresentDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.FutureOrPresent.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.FutureOrPresent))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.FutureOrPresent.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.FutureOrPresentDateOnly(value));
    }

    // GuardStringDateOnlyClauses.FutureDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Future.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Future))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Future.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Future))]
    public void Future_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.FutureDateOnly(value));
    }

    // GuardStringDateOnlyClauses.PastOrPresentDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.PastOrPresent.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.PastOrPresent))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.PastOrPresent.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.PastOrPresentDateOnly(value));
    }

    // GuardStringDateOnlyClauses.PastDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Past.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Past))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Past.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Past))]
    public void Past_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.PastDateOnly(value));
    }

    // GuardStringDateOnlyClauses.NotBetweenDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotBetween.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotBetween))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotBetween.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotBetween))]
    public void NotBetween_BehavesAsExpected(GuardCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotBetweenDateOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
    }

    // GuardStringDateOnlyClauses.BetweenDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Between.ValidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Between))]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.Between.InvalidCases), MemberType = typeof(GuardStringDateOnlyClausesTestData.Between))]
    public void Between_BehavesAsExpected(GuardCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.BetweenDateOnly(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
    }

    // GuardStringDateOnlyClauses.NotWithinDaysDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotWithinDaysDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotWithinDaysDateOnly))]
    public void NotWithinDaysDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotWithinDaysDateOnly(value, tc.Value.reference, tc.Value.days));
    }

    // GuardStringDateOnlyClauses.WithinDaysDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.WithinDaysDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.WithinDaysDateOnly))]
    public void WithinDaysDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly? reference, int days)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.WithinDaysDateOnly(value, tc.Value.reference, tc.Value.days));
    }

    // GuardStringDateOnlyClauses.NotWithinCalendarMonthsDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotWithinCalendarMonthsDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotWithinCalendarMonthsDateOnly))]
    public void NotWithinCalendarMonthsDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotWithinCalendarMonthsDateOnly(value, tc.Value.reference, tc.Value.months));
    }

    // GuardStringDateOnlyClauses.WithinCalendarMonthsDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.WithinCalendarMonthsDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.WithinCalendarMonthsDateOnly))]
    public void WithinCalendarMonthsDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly? reference, int months)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.WithinCalendarMonthsDateOnly(value, tc.Value.reference, tc.Value.months));
    }

    // GuardStringDateOnlyClauses.BeforeDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.BeforeDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.BeforeDateOnly))]
    public void BeforeDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.BeforeDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.OnOrBeforeDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.OnOrBeforeDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.OnOrBeforeDateOnly))]
    public void OnOrBeforeDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.OnOrBeforeDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.AfterDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.AfterDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.AfterDateOnly))]
    public void AfterDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.AfterDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.OnOrAfterDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.OnOrAfterDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.OnOrAfterDateOnly))]
    public void OnOrAfterDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.OnOrAfterDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.SameDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.SameDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.SameDateOnly))]
    public void SameDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.SameDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.NotSameDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotSameDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotSameDateOnly))]
    public void NotSameDateOnly_BehavesAsExpected(GuardCase<(string? value, DateOnly other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotSameDateOnly(value, tc.Value.other));
    }

    // GuardStringDateOnlyClauses.ChronologicalDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.ChronologicalDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.ChronologicalDateOnly))]
    public void ChronologicalDateOnly_BehavesAsExpected(GuardCase<(string? start, string? end)> tc)
    {
        var start = tc.Value.start;
        AssertResult(tc, () => Guard.Against.ChronologicalDateOnly(start!, tc.Value.end!));
    }

    // GuardStringDateOnlyClauses.OverlappingDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.OverlappingDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.OverlappingDateOnly))]
    public void OverlappingDateOnly_BehavesAsExpected(GuardCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    {
        var start1 = tc.Value.start1;
        AssertResult(tc, () => Guard.Against.OverlappingDateOnly(start1!, tc.Value.end1!, tc.Value.start2!, tc.Value.end2!));
    }

    // GuardStringDateOnlyClauses.NotOverlappingDateOnly
    [Theory]
    [MemberData(nameof(GuardStringDateOnlyClausesTestData.NotOverlappingDateOnly.Cases), MemberType = typeof(GuardStringDateOnlyClausesTestData.NotOverlappingDateOnly))]
    public void NotOverlappingDateOnly_BehavesAsExpected(GuardCase<(string? start1, string? end1, string? start2, string? end2)> tc)
    {
        var start1 = tc.Value.start1;
        AssertResult(tc, () => Guard.Against.NotOverlappingDateOnly(start1!, tc.Value.end1!, tc.Value.start2!, tc.Value.end2!));
    }
}
