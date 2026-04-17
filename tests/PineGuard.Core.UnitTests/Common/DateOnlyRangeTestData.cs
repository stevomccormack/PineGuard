using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class DateOnlyRangeTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("same day", new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 01), 1),
            new("one day span", new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 02), 2),
            new("ten day span", new DateOnly(2022, 03, 01), new DateOnly(2022, 03, 10), 10),
            new("leap day same", new DateOnly(2020, 02, 29), new DateOnly(2020, 02, 29), 1),
            new("leap day across months", new DateOnly(2020, 02, 28), new DateOnly(2020, 03, 01), 3),
            new("month boundary", new DateOnly(2019, 12, 31), new DateOnly(2020, 01, 01), 2),
            new("30 day month", new DateOnly(2020, 06, 01), new DateOnly(2020, 06, 30), 30),
            new("31 day month", new DateOnly(2020, 07, 01), new DateOnly(2020, 07, 31), 31),
            new("full leap year", new DateOnly(2024, 01, 01), new DateOnly(2024, 12, 31), 366),
            new("full non-leap year", new DateOnly(2025, 01, 01), new DateOnly(2025, 12, 31), 365)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("start after end (day)", new DateOnly(2020, 01, 02), new DateOnly(2020, 01, 01), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("start after end (month)", new DateOnly(2020, 12, 31), new DateOnly(2020, 01, 01), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("start after end (year)", new DateOnly(2001, 01, 01), new DateOnly(2000, 12, 31), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("start after end (leap)", new DateOnly(2024, 02, 29), new DateOnly(2024, 02, 28), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("start after end (random)", new DateOnly(2032, 11, 03), new DateOnly(2032, 11, 02), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("min+1 before min", DateOnly.MinValue.AddDays(1), DateOnly.MinValue, new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("max before max-1", DateOnly.MaxValue, DateOnly.MaxValue.AddDays(-1), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("max before min", DateOnly.MaxValue, DateOnly.MinValue, new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("leap march before feb 29", new DateOnly(2024, 03, 01), new DateOnly(2024, 02, 29), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("year end before year start", new DateOnly(2025, 12, 31), new DateOnly(2025, 01, 01), new ExpectedException(typeof(ArgumentException), "start", "less than or equal"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min-min", DateOnly.MinValue, DateOnly.MinValue, 1),
            new("min+1", DateOnly.MinValue, DateOnly.MinValue.AddDays(1), 2),
            new("max-max", DateOnly.MaxValue, DateOnly.MaxValue, 1),
            new("max-1", DateOnly.MaxValue.AddDays(-1), DateOnly.MaxValue, 2),
            new("leap boundary", new DateOnly(2024, 02, 28), new DateOnly(2024, 02, 29), 2)
        ];

        public sealed record ValidCase(string Name, DateOnly Start, DateOnly End, int ExpectedDayCount)
            : ReturnCase<(DateOnly Start, DateOnly End), int>(Name, (Start, End), ExpectedDayCount);

        public sealed record InvalidCase(string Name, DateOnly Start, DateOnly End, ExpectedException ExpectedException)
            : ThrowsCase<(DateOnly Start, DateOnly End)>(Name, (Start, End), ExpectedException);
    }

    public static class TryCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", (new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 02)), true, new DateOnlyRange(new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 02)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, new DateOnly(2020, 01, 01)), false, default),
            new("end null", (new DateOnly(2020, 01, 01), null), false, default),
            new("start after end", (new DateOnly(2020, 01, 02), new DateOnly(2020, 01, 01)), false, default)
        ];

        public sealed record ValidCase(string Name, (DateOnly? Start, DateOnly? End) Input, bool Expected, DateOnlyRange ExpectedOutValue)
            : TryCase<(DateOnly? Start, DateOnly? End), DateOnlyRange>(Name, Input, Expected, ExpectedOutValue);
    }

    public static class Equality
    {
        public static TheoryData<Case> Cases =>
        [
            new("Same Instance", new DateOnlyRange(DateOnly.MinValue, DateOnly.MaxValue), new DateOnlyRange(DateOnly.MinValue, DateOnly.MaxValue), true),
            new("Diff End", new DateOnlyRange(DateOnly.MinValue, DateOnly.MaxValue), new DateOnlyRange(DateOnly.MinValue, DateOnly.MaxValue.AddDays(-1)), false),
            new("Diff Start", new DateOnlyRange(DateOnly.MinValue, DateOnly.MaxValue), new DateOnlyRange(DateOnly.MinValue.AddDays(1), DateOnly.MaxValue), false)
        ];

        public sealed record Case(string Name, DateOnlyRange Left, DateOnlyRange Right, bool Expected)
            : ValueCase<(DateOnlyRange Left, DateOnlyRange Right)>(Name, (Left, Right));
    }

    public static class Intersect
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 20));
                var before = new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 15));
                var after = new DateOnlyRange(new DateOnly(2024, 01, 15), new DateOnly(2024, 01, 30));

                return
                [
                    new Case("Overlap Before", range, before, new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 15))),
                    new Case("Overlap After", range, after, new DateOnlyRange(new DateOnly(2024, 01, 15), new DateOnly(2024, 01, 20)))
                ];
            }
        }

        public sealed record Case(string Name, DateOnlyRange Base, DateOnlyRange Other, DateOnlyRange? Expected)
            : ReturnCase<(DateOnlyRange Base, DateOnlyRange Other), DateOnlyRange?>(Name, (Base, Other), Expected);
    }

    public static class Union
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 20));
                var before = new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 15));
                var after = new DateOnlyRange(new DateOnly(2024, 01, 15), new DateOnly(2024, 01, 30));

                return
                [
                    new Case("Overlap Before", range, before, new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 20))),
                    new Case("Overlap After", range, after, new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 30)))
                ];
            }
        }

        public sealed record Case(string Name, DateOnlyRange Base, DateOnlyRange Other, DateOnlyRange Expected)
            : ReturnCase<(DateOnlyRange Base, DateOnlyRange Other), DateOnlyRange>(Name, (Base, Other), Expected);
    }

    public static class Overlaps
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateOnlyRange(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 20));
                var before = new DateOnlyRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10)); // touches start
                var overlap = new DateOnlyRange(new DateOnly(2024, 1, 5), new DateOnly(2024, 1, 15)); // genuinely overlaps
                var after = new DateOnlyRange(new DateOnly(2024, 1, 20), new DateOnly(2024, 1, 30)); // touches end
                var disjointBefore = new DateOnlyRange(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 5)); // totally before
                var disjointAfter = new DateOnlyRange(new DateOnly(2024, 1, 25), new DateOnly(2024, 1, 30)); // totally after

                return
                [
                    new Case("Touches Before Exclusive", range, before, Inclusion.Exclusive, false),
                    new Case("Touches Before Inclusive", range, before, Inclusion.Inclusive, true),
                    new Case("Genuine Overlap Exclusive", range, overlap, Inclusion.Exclusive, true),
                    new Case("Touches After Exclusive", range, after, Inclusion.Exclusive, false),
                    new Case("Disjoint Before Inclusive", range, disjointBefore, Inclusion.Inclusive, false),
                    new Case("Disjoint After Inclusive", range, disjointAfter, Inclusion.Inclusive, false)
                ];
            }
        }

        public sealed record Case(string Name, DateOnlyRange Base, DateOnlyRange Other, Inclusion Inclusion, bool Expected)
            : ReturnCase<(DateOnlyRange Base, DateOnlyRange Other), bool>(Name, (Base, Other), Expected);
    }

    public static class Adjacency
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateOnlyRange(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 20));
                var touchesAtEnd = new DateOnlyRange(range.End.AddDays(1), range.End.AddDays(1));
                var touchesAtStart = new DateOnlyRange(range.Start.AddDays(-1), range.Start.AddDays(-1));

                return
                [
                    new Case("Touches at End", range, touchesAtEnd, true),
                    new Case("Touches at Start", range, touchesAtStart, true)
                ];
            }
        }

        public sealed record Case(string Name, DateOnlyRange Base, DateOnlyRange Other, bool Expected)
            : ReturnCase<(DateOnlyRange Base, DateOnlyRange Other), bool>(Name, (Base, Other), Expected);
    }
}
