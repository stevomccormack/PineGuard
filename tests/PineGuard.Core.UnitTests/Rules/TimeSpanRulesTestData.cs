using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeSpanRulesTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "middle inclusive", Value: TimeSpan.FromMinutes(30), Min: TimeSpan.FromMinutes(10), Max: TimeSpan.FromMinutes(60), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "at min inclusive", Value: TimeSpan.FromMinutes(10), Min: TimeSpan.FromMinutes(10), Max: TimeSpan.FromMinutes(60), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "at min exclusive", Value: TimeSpan.FromMinutes(10), Min: TimeSpan.FromMinutes(10), Max: TimeSpan.FromMinutes(60), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "null", Value: null, Min: TimeSpan.FromMinutes(10), Max: TimeSpan.FromMinutes(60), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Min, TimeSpan Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "above exclusive", Value: TimeSpan.FromMinutes(11), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "equal exclusive", Value: TimeSpan.FromMinutes(10), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "equal inclusive", Value: TimeSpan.FromMinutes(10), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null", Value: null, Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Threshold, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "below exclusive", Value: TimeSpan.FromMinutes(9), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "equal exclusive", Value: TimeSpan.FromMinutes(10), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "equal inclusive", Value: TimeSpan.FromMinutes(10), Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "null", Value: null, Threshold: TimeSpan.FromMinutes(10), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, TimeSpan? Value, TimeSpan Threshold, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
