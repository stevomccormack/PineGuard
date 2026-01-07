using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRulesTestData
{
    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "middle inclusive", Value: new TimeOnly(12, 0), Min: new TimeOnly(11, 0), Max: new TimeOnly(13, 0), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "at min inclusive", Value: new TimeOnly(11, 0), Min: new TimeOnly(11, 0), Max: new TimeOnly(13, 0), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "at min exclusive", Value: new TimeOnly(11, 0), Min: new TimeOnly(11, 0), Max: new TimeOnly(13, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "null", Value: null, Min: new TimeOnly(11, 0), Max: new TimeOnly(13, 0), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeOnly? Value, TimeOnly Min, TimeOnly Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "before exclusive", Start: new TimeOnly(11, 0), End: new TimeOnly(12, 0), Inclusion: Inclusion.Exclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "equal exclusive", Start: new TimeOnly(11, 0), End: new TimeOnly(11, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "equal inclusive", Start: new TimeOnly(11, 0), End: new TimeOnly(11, 0), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null start", Start: null, End: new TimeOnly(12, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "null end", Start: new TimeOnly(11, 0), End: null, Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeOnly? Start, TimeOnly? End, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "overlap exclusive", Start1: new TimeOnly(9, 0), End1: new TimeOnly(10, 0), Start2: new TimeOnly(9, 30), End2: new TimeOnly(9, 45), Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "separate exclusive", Start1: new TimeOnly(11, 0), End1: new TimeOnly(12, 0), Start2: new TimeOnly(9, 0), End2: new TimeOnly(10, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "separate inclusive", Start1: new TimeOnly(11, 0), End1: new TimeOnly(12, 0), Start2: new TimeOnly(9, 0), End2: new TimeOnly(10, 0), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "touching exclusive", Start1: new TimeOnly(9, 0), End1: new TimeOnly(10, 0), Start2: new TimeOnly(10, 0), End2: new TimeOnly(11, 0), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "touching inclusive", Start1: new TimeOnly(9, 0), End1: new TimeOnly(10, 0), Start2: new TimeOnly(10, 0), End2: new TimeOnly(11, 0), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null start", Start1: null, End1: new TimeOnly(10, 0), Start2: new TimeOnly(9, 30), End2: new TimeOnly(9, 45), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeOnly? Start1, TimeOnly? End1, TimeOnly? Start2, TimeOnly? End2, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
