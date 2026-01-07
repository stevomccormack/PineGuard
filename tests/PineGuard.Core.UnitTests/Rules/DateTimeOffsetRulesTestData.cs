using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var now = DateTimeOffset.UtcNow;
                return new TheoryData<Case>
                {
                    { new Case(Name: "Two days ago", Value: now.AddDays(-2), Expected: true) },
                };
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                return new TheoryData<Case>
                {
                    { new Case(Name: "Null", Value: null, Expected: false) },
                };
            }
        }

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, bool Expected)
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
                var now = DateTimeOffset.UtcNow;
                return new TheoryData<Case>
                {
                    { new Case(Name: "In two days", Value: now.AddDays(2), Expected: true) },
                };
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                return new TheoryData<Case>
                {
                    { new Case(Name: "Null", Value: null, Expected: false) },
                };
            }
        }

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "Middle inclusive",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Min: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Max: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
            { new Case(
                Name: "Min inclusive",
                Value: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Min: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Max: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(
                Name: "Min exclusive",
                Value: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Min: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Max: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
            { new Case(
                Name: "Null",
                Value: null,
                Min: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Max: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Min, DateTimeOffset Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsBefore
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "Before inclusive",
                Value: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
            { new Case(
                Name: "Same instant inclusive",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(
                Name: "Same instant exclusive",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
            { new Case(
                Name: "Null",
                Value: null,
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsAfter
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "After inclusive",
                Value: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
            { new Case(
                Name: "Same instant inclusive",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(
                Name: "Same instant exclusive",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
            { new Case(
                Name: "Null",
                Value: null,
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsSame
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "Same instant",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(
                Name: "Different instant",
                Value: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Other: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Expected: false) },
            { new Case(
                Name: "Null",
                Value: null,
                Other: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "Increasing exclusive",
                Start: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: true) },
            { new Case(
                Name: "Same instant inclusive",
                Start: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Both null", Start: null, End: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "End null", Start: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), End: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "Start null", Start: null, End: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(
                Name: "Same instant exclusive",
                Start: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffset? Start, DateTimeOffset? End, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(
                Name: "Touching inclusive",
                Start1: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End1: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Start2: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                End2: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Inclusive,
                Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(
                Name: "Touching exclusive",
                Start1: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End1: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Start2: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                End2: new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },

            { new Case(Name: "All null", Start1: null, End1: null, Start2: null, End2: null, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(
                Name: "End1 null",
                Start1: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End1: null,
                Start2: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End2: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
            { new Case(
                Name: "Start2 null",
                Start1: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End1: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Start2: null,
                End2: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
            { new Case(
                Name: "End2 null",
                Start1: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End1: new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero),
                Start2: new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
                End2: null,
                Inclusion: Inclusion.Exclusive,
                Expected: false) },
        };

        #region Cases

        public sealed record Case(
            string Name,
            DateTimeOffset? Start1,
            DateTimeOffset? End1,
            DateTimeOffset? Start2,
            DateTimeOffset? End2,
            Inclusion Inclusion,
            bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
