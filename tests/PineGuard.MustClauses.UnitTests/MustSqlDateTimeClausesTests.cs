using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustSqlDateTimeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateRange.ValidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateRange))]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateRange.InvalidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateRange))]
    public void InSqlDateRange_BehavesAsExpected(MustCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = Must.Be.InSqlDateRange(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeOffset.ValidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeOffset))]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeOffset.InvalidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeOffset))]
    public void InSqlDateTimeRangeOffset_BehavesAsExpected(MustCase<DateTimeOffset> tc)
    {
        var value = tc.Value;
        var result = Must.Be.InSqlDateTimeRange(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeDateTime.ValidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeDateTime))]
    [MemberData(nameof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeDateTime.InvalidCases), MemberType = typeof(MustSqlDateTimeClausesTestData.InSqlDateTimeRangeDateTime))]
    public void InSqlDateTimeRangeDateTime_BehavesAsExpected(MustCase<DateTime> tc)
    {
        var value = tc.Value;
        var result = Must.Be.InSqlDateTimeRange(value);
        AssertResult(tc, result);
    }
}
