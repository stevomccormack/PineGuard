using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        private static Case V(string name, DateOnlyRange range, Inclusion inclusion, bool expected)
            => new(name, range, inclusion, expected);

        private static Case E(string name, DateOnlyRange? range, Inclusion inclusion, bool expected)
            => new(name, range, inclusion, expected);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("two days, exclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), Inclusion.Exclusive, true) },
            { V("same day, inclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Inclusive, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { E("null", null, Inclusion.Exclusive, false) },
            { E("same day, exclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Exclusive, false) },
        };

        #region Cases

        public sealed record Case(string Name, DateOnlyRange? Range, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOverlapping
    {
        private static Case V(string name, DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion, bool expected)
            => new(name, range1, range2, inclusion, expected);

        private static Case E(string name, DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion, bool expected)
            => new(name, range1, range2, inclusion, expected);

        public static TheoryData<Case> ValidCases => new()
        {
            {
                V(
                    "touching boundary, inclusive",
                    new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)),
                    new DateOnlyRange(new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03)),
                    Inclusion.Inclusive,
                    true)
            },
            {
                V(
                    "touching boundary, exclusive",
                    new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)),
                    new DateOnlyRange(new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03)),
                    Inclusion.Exclusive,
                    false)
            },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { E("both null", null, null, Inclusion.Exclusive, false) },
            {
                E(
                    "range2 null",
                    new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)),
                    null,
                    Inclusion.Exclusive,
                    false)
            },
            {
                E(
                    "range1 null",
                    null,
                    new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)),
                    Inclusion.Exclusive,
                    false)
            },
        };

        #region Cases

        public sealed record Case(string Name, DateOnlyRange? Range1, DateOnlyRange? Range2, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
