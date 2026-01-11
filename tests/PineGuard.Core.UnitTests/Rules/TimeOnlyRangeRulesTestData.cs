using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("chronological", new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(13, 0)), Inclusion.Exclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal exclusive", new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(12, 0)), Inclusion.Exclusive, false),
            new("equal inclusive", new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(12, 0)), Inclusion.Inclusive, true),
            new("null", null, Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, TimeOnlyRange? Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<TimeOnlyRange?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("overlap exclusive", (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(9, 30), new TimeOnly(9, 45))), Inclusion.Exclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching exclusive", (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0))), Inclusion.Exclusive, false),
            new("touching inclusive", (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0))), Inclusion.Inclusive, true),
            new("null left", (null, new TimeOnlyRange(new TimeOnly(9, 30), new TimeOnly(9, 45))), Inclusion.Exclusive, false),
            new("null right", (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), null), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (TimeOnlyRange? Range1, TimeOnlyRange? Range2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(TimeOnlyRange? Range1, TimeOnlyRange? Range2)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
