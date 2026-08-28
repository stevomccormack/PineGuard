using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.TimeOnlyRangeRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustTimeOnlyRangeClausesTestData
{
    public static class Chronological
    {
        public static TheoryData<MustCase<(TimeOnlyRange range, Inclusion inclusion)>> ValidCases => F.IsChronological.NonNullValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(TimeOnlyRange range, Inclusion inclusion)>> InvalidCases => F.IsChronological.NonNullInvalidScenarios.ToMustCases(_ =>
            new MustExpected(false, "range must be chronological.", Code: MustCodes.Range.Order.NotChronological));
    }

    public static class Overlapping
    {
        public static TheoryData<MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>> ValidCases => F.IsOverlapping.NonNullValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>> InvalidCases => F.IsOverlapping.NonNullInvalidScenarios.ToMustCases(_ =>
            new MustExpected(false, "range1 must be overlapping.", Code: MustCodes.Range.Overlap.Missing));
    }

    public static class NotOverlapping
    {
        public static TheoryData<MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>> ValidCases => F.IsOverlapping.NonNullInvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>> InvalidCases => F.IsOverlapping.NonNullValidScenarios.ToMustCases(_ =>
            new MustExpected(false, "range1 must not be overlapping.", Code: MustCodes.Range.Overlap.Present));
    }

    public static class Contains
    {
        public static TheoryData<MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>> ValidCases => F.Contains.NonNullValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>> InvalidCases => F.Contains.NonNullInvalidScenarios.ToMustCases(_ =>
            new MustExpected(false, "range must contain the specified time.", Code: MustCodes.Range.Bounds.NotContains));
    }

    public static class NotContains
    {
        public static TheoryData<MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>> ValidCases => F.Contains.NonNullInvalidScenarios.ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>> InvalidCases => F.Contains.NonNullValidScenarios.ToMustCases(_ =>
            new MustExpected(false, "range must not contain the specified time.", Code: MustCodes.Range.Bounds.Contains));
    }
}
