using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringDateTimeOffsetClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.PastDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.PastDateTimeOffset))]
    public void PastDateTimeOffset_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.PastDateTimeOffset(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.PastOrPresentDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.PastOrPresentDateTimeOffset))]
    public void PastOrPresentDateTimeOffset_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.PastOrPresentDateTimeOffset(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.FutureDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.FutureDateTimeOffset))]
    public void FutureDateTimeOffset_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.FutureDateTimeOffset(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.FutureOrPresentDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.FutureOrPresentDateTimeOffset))]
    public void FutureOrPresentDateTimeOffset_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.FutureOrPresentDateTimeOffset(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.BetweenDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.BetweenDateTimeOffset))]
    public void BetweenDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.BetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.NotBetweenDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.NotBetweenDateTimeOffset))]
    public void NotBetweenDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotBetweenDateTimeOffset(value, tc.Value.min, tc.Value.max, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.WithinDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.WithinDateTimeOffset))]
    public void WithinDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.WithinDateTimeOffset(value, tc.Value.reference, tc.Value.window);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.NotWithinDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.NotWithinDateTimeOffset))]
    public void NotWithinDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotWithinDateTimeOffset(value, tc.Value.reference, tc.Value.window);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.WithinCalendarMonthsDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.WithinCalendarMonthsDateTimeOffset))]
    public void WithinCalendarMonthsDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.WithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringDateTimeOffsetClausesTestData.NotWithinCalendarMonthsDateTimeOffset.Cases), MemberType = typeof(MustStringDateTimeOffsetClausesTestData.NotWithinCalendarMonthsDateTimeOffset))]
    public void NotWithinCalendarMonthsDateTimeOffset_BehavesAsExpected(MustCase<(string? value, DateTimeOffset? reference, int months)> tc)
    {
        var value = tc.Value.value;
        var result = Must.Be.NotWithinCalendarMonthsDateTimeOffset(value, tc.Value.reference, tc.Value.months);
        AssertResult(tc, result);
    }
}
