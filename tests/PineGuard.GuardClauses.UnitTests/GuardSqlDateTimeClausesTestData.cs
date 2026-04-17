using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.SqlDateTimeRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

#pragma warning disable CS0618

public static class GuardSqlDateTimeClausesTestData
{
    public static class NotInSqlDateRange
    {
        public static TheoryData<GuardCase<DateOnly>> ValidCases => F.IsInSqlDateRange.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<DateOnly>> InvalidCases => F.IsInSqlDateRange.AllInvalid.ToGuardCases("value");
    }

    public static class NotInSqlDateTimeOffsetRange
    {
        public static TheoryData<GuardCase<DateTimeOffset>> ValidCases => F.IsInSqlDateTimeRangeDateTimeOffset.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<DateTimeOffset>> InvalidCases => F.IsInSqlDateTimeRangeDateTimeOffset.AllInvalid.ToGuardCases("value");
    }

    public static class NotInSqlDateTimeRange
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases => F.IsInSqlDateTimeRangeDateTime.AllValid.ToGuardCases();
        public static TheoryData<GuardCase<DateTime>> InvalidCases => F.IsInSqlDateTimeRangeDateTime.AllInvalid.ToGuardCases("value");
    }
}
