using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardSqlDateTimeClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateRange.ValidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateRange))]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateRange.InvalidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateRange))]
    public void NotInSqlDateRange_BehavesAsExpected(GuardCase<DateOnly> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotInSqlDateRange(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeOffsetRange.ValidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeOffsetRange))]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeOffsetRange.InvalidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeOffsetRange))]
    public void NotInSqlDateTimeOffsetRange_BehavesAsExpected(GuardCase<DateTimeOffset> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotInSqlDateTimeOffsetRange(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeRange.ValidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeRange))]
    [MemberData(nameof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeRange.InvalidCases), MemberType = typeof(GuardSqlDateTimeClausesTestData.NotInSqlDateTimeRange))]
    public void NotInSqlDateTimeRange_BehavesAsExpected(GuardCase<DateTime> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotInSqlDateTimeRange(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
