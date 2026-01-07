using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return new TheoryData<Case>
                {
                    { new Case(Name: "Two days ago", Value: today.AddDays(-2), Expected: true) },
                };
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return new TheoryData<Case>
                {
                    { new Case(Name: "Today", Value: today, Expected: false) },
                    { new Case(Name: "Null", Value: null, Expected: false) },
                };
            }
        }

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return new TheoryData<Case>
                {
                    { new Case(Name: "In two days", Value: today.AddDays(2), Expected: true) },
                };
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return new TheoryData<Case>
                {
                    { new Case(Name: "Today", Value: today, Expected: false) },
                    { new Case(Name: "Null", Value: null, Expected: false) },
                };
            }
        }

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Middle inclusive", Value: new DateOnly(2024, 01, 02), Min: new DateOnly(2024, 01, 01), Max: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Min inclusive", Value: new DateOnly(2024, 01, 01), Min: new DateOnly(2024, 01, 01), Max: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Min exclusive", Value: new DateOnly(2024, 01, 01), Min: new DateOnly(2024, 01, 01), Max: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Null value", Value: null, Min: new DateOnly(2024, 01, 01), Max: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, DateOnly Min, DateOnly Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsBefore
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Before inclusive", Value: new DateOnly(2024, 01, 01), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Same day inclusive", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Same day exclusive", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Null value", Value: null, Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsAfter
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "After inclusive", Value: new DateOnly(2024, 01, 03), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Same day inclusive", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Same day exclusive", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Null value", Value: null, Other: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsSame
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Same day", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 02), Expected: true) },
            { new Case(Name: "Different day", Value: new DateOnly(2024, 01, 02), Other: new DateOnly(2024, 01, 03), Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Null value", Value: null, Other: new DateOnly(2024, 01, 02), Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Increasing exclusive", Start: new DateOnly(2024, 01, 01), End: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Exclusive, Expected: true) },
            { new Case(Name: "Same day inclusive", Start: new DateOnly(2024, 01, 01), End: new DateOnly(2024, 01, 01), Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Both null", Start: null, End: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "End null", Start: new DateOnly(2024, 01, 01), End: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "Start null", Start: null, End: new DateOnly(2024, 01, 01), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "Same day exclusive", Start: new DateOnly(2024, 01, 01), End: new DateOnly(2024, 01, 01), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Start, DateOnly? End, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Touching inclusive", Start1: new DateOnly(2024, 01, 01), End1: new DateOnly(2024, 01, 02), Start2: new DateOnly(2024, 01, 02), End2: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "Touching exclusive", Start1: new DateOnly(2024, 01, 01), End1: new DateOnly(2024, 01, 02), Start2: new DateOnly(2024, 01, 02), End2: new DateOnly(2024, 01, 03), Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "All null", Start1: null, End1: null, Start2: null, End2: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "End1 null", Start1: new DateOnly(2024, 01, 01), End1: null, Start2: new DateOnly(2024, 01, 01), End2: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "Start2 null", Start1: new DateOnly(2024, 01, 01), End1: new DateOnly(2024, 01, 02), Start2: null, End2: new DateOnly(2024, 01, 02), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "End2 null", Start1: new DateOnly(2024, 01, 01), End1: new DateOnly(2024, 01, 02), Start2: new DateOnly(2024, 01, 01), End2: null, Inclusion: Inclusion.Exclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnly? Start1, DateOnly? End1, DateOnly? Start2, DateOnly? End2, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
