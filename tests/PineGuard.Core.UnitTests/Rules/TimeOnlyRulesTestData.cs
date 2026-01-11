using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRulesTestData
{
    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("middle inclusive", new TimeOnly(12, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive, true),
            new("at min inclusive", new TimeOnly(11, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("at min exclusive", new TimeOnly(11, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Exclusive, false),
            new("null", null, new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, TimeOnly? Value, TimeOnly Min, TimeOnly Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<TimeOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("before exclusive", (new TimeOnly(11, 0), new TimeOnly(12, 0)), Inclusion.Exclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("equal exclusive", (new TimeOnly(11, 0), new TimeOnly(11, 0)), Inclusion.Exclusive, false),
            new("equal inclusive", (new TimeOnly(11, 0), new TimeOnly(11, 0)), Inclusion.Inclusive, true),
            new("null start", (null, new TimeOnly(12, 0)), Inclusion.Exclusive, false),
            new("null end", (new TimeOnly(11, 0), null), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, (TimeOnly? Start, TimeOnly? End) Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<(TimeOnly? Start, TimeOnly? End)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("overlap exclusive", (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(9, 30), new TimeOnly(9, 45)), Inclusion.Exclusive, true),
            new("separate exclusive", (new TimeOnly(11, 0), new TimeOnly(12, 0), new TimeOnly(9, 0), new TimeOnly(10, 0)), Inclusion.Exclusive, false),
            new("separate inclusive", (new TimeOnly(11, 0), new TimeOnly(12, 0), new TimeOnly(9, 0), new TimeOnly(10, 0)), Inclusion.Inclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching exclusive", (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(10, 0), new TimeOnly(11, 0)), Inclusion.Exclusive, false),
            new("touching inclusive", (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(10, 0), new TimeOnly(11, 0)), Inclusion.Inclusive, true),
            new("null start", (null, new TimeOnly(10, 0), new TimeOnly(9, 30), new TimeOnly(9, 45)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (TimeOnly? Start1, TimeOnly? End1, TimeOnly? Start2, TimeOnly? End2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(TimeOnly? Start1, TimeOnly? End1, TimeOnly? Start2, TimeOnly? End2)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
