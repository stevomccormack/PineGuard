using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRangeRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDateTimeOffsetRangeClausesTestData
{
    public static class NotChronological
    {
        public static TheoryData<GuardCase<(DateTimeOffsetRange range, Inclusion inclusion)>> ValidCases =>
            F.IsChronological.NonNullValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffsetRange range, Inclusion inclusion)>> InvalidCases =>
            F.IsChronological.NonNullInvalidScenarios.ToGuardCases("range");
    }

    public static class Overlapping
    {
        public static TheoryData<GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>> ValidCases =>
            F.IsOverlapping.NonNullInvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>> InvalidCases =>
            F.IsOverlapping.NonNullValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "range1"));
    }

    public static class NotOverlapping
    {
        public static TheoryData<GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>> ValidCases =>
            F.IsOverlapping.NonNullValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>> InvalidCases =>
            F.IsOverlapping.NonNullInvalidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "range1"));
    }

    public static class NotContains
    {
        public static TheoryData<GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>> ValidCases =>
            F.Contains.NonNullValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>> InvalidCases =>
            F.Contains.NonNullInvalidScenarios.ToGuardCases("range");
    }

    public static class Contains
    {
        public static TheoryData<GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>> ValidCases =>
            F.Contains.NonNullInvalidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>> InvalidCases =>
            F.Contains.NonNullValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "range"));
    }
}
