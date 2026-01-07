using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "chronological", Range: new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(13, 0)), Inclusion: Inclusion.Exclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "equal exclusive", Range: new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(12, 0)), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "equal inclusive", Range: new TimeOnlyRange(new TimeOnly(12, 0), new TimeOnly(12, 0)), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null", Range: null, Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeOnlyRange? Range, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "overlap exclusive", Range1: new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), Range2: new TimeOnlyRange(new TimeOnly(9, 30), new TimeOnly(9, 45)), Inclusion: Inclusion.Exclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "touching exclusive", Range1: new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), Range2: new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "touching inclusive", Range1: new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), Range2: new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0)), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null left", Range1: null, Range2: new TimeOnlyRange(new TimeOnly(9, 30), new TimeOnly(9, 45)), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "null right", Range1: new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), Range2: null, Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeOnlyRange? Range1, TimeOnlyRange? Range2, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
