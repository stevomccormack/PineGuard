using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.SqlDateTimeRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

#pragma warning disable CS0618

public static class MustSqlDateTimeClausesTestData
{
    public static class InSqlDateRange
    {
        public static TheoryData<MustCase<DateOnly>> ValidCases => F.IsInSqlDateRange.AllValid.ToMustCases();

        public static TheoryData<MustCase<DateOnly>> InvalidCases => F.IsInSqlDateRange.AllInvalid.ToMustCases(_ =>
            new MustExpected(false, "value must be within the SQL date range."));
    }

    public static class InSqlDateTimeRangeOffset
    {
        public static TheoryData<MustCase<DateTimeOffset>> ValidCases => F.IsInSqlDateTimeRangeDateTimeOffset.AllValid.ToMustCases();

        public static TheoryData<MustCase<DateTimeOffset>> InvalidCases => F.IsInSqlDateTimeRangeDateTimeOffset.AllInvalid.ToMustCases(_ =>
            new MustExpected(false, "value must be within the SQL date/time range."));
    }

    public static class InSqlDateTimeRangeDateTime
    {
        public static TheoryData<MustCase<DateTime>> ValidCases => F.IsInSqlDateTimeRangeDateTime.AllValid.ToMustCases();

        public static TheoryData<MustCase<DateTime>> InvalidCases => F.IsInSqlDateTimeRangeDateTime.AllInvalid.ToMustCases(_ =>
            new MustExpected(false, "value must be within the SQL date/time range."));
    }
}
