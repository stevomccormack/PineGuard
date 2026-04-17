using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class DateTimeRangeTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc same", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("utc +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("local +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Local), TimeSpan.FromSeconds(1)),
            new("utc 90 minutes", new DateTime(2020, 01, 01, 12, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 13, 30, 00, DateTimeKind.Utc), TimeSpan.FromMinutes(90)),
            new("leap day", new DateTime(2020, 02, 29, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(1)),
            new("unspecified same", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified +1 second", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("utc +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), TimeSpan.FromHours(1)),
            new("local +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Local), TimeSpan.FromHours(1)),
            new("local 30 minutes", new DateTime(2023, 11, 05, 01, 00, 00, DateTimeKind.Local), new DateTime(2023, 11, 05, 01, 30, 00, DateTimeKind.Local), TimeSpan.FromMinutes(30)),
            new("utc 2024 year span", new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 12, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(365)),
            new("utc 2025 year span", new DateTime(2025, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(364)),
            new("utc +1 second (2030)", new DateTime(2030, 04, 15, 10, 00, 00, DateTimeKind.Utc), new DateTime(2030, 04, 15, 10, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("unspecified 12 hours", new DateTime(2031, 08, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2031, 08, 01, 12, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromHours(12)),
            new("utc boundary second", new DateTime(2032, 10, 30, 23, 59, 59, DateTimeKind.Utc), new DateTime(2032, 10, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("utc 30 days", new DateTime(1999, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(1999, 01, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(30)),
            new("utc feb 2000", new DateTime(2000, 02, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(28)),
            new("local same", new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("unspecified +2 seconds", new DateTime(2011, 06, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2011, 06, 01, 00, 00, 02, DateTimeKind.Unspecified), TimeSpan.FromSeconds(2)),
            new("utc +2 seconds", new DateTime(2012, 07, 01, 12, 00, 00, DateTimeKind.Utc), new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Utc), TimeSpan.FromSeconds(2))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min-min unspecified", new DateTime(0001, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(0001, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("max-max unspecified", new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("utc with unspecified end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified with utc end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("local with unspecified end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified with local end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("unspecified to utc +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("utc to unspecified +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +1 second (2021)", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("local to unspecified +1 second", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), TimeSpan.FromHours(1)),
            new("utc to unspecified +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromHours(1)),
            new("unspecified same", new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified +1 day", new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2023, 01, 02, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromDays(1)),
            new("utc year boundary second", new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc), new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("leap day same utc", new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Utc), new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("leap day +1 second unspecified", new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Unspecified), new DateTime(2024, 02, 29, 12, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("local same", new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("local +1 second", new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), new DateTime(2025, 12, 31, 00, 00, 01, DateTimeKind.Local), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +2 seconds", new DateTime(2012, 07, 01, 12, 00, 00, DateTimeKind.Unspecified), new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Utc), TimeSpan.FromSeconds(2))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("utc -1 second", new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day", new DateTime(2020, 01, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc -1 second", new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day", new DateTime(2020, 01, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local previous day", new DateTime(2020, 03, 02, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("unspecified previous day", new DateTime(2021, 01, 02, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc-local", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind"))
        ];

        public sealed record ValidCase(string Name, DateTime Start, DateTime End, TimeSpan ExpectedDuration)
            : ReturnCase<(DateTime Start, DateTime End), TimeSpan>(Name, (Start, End), ExpectedDuration);

        public sealed record InvalidCase(string Name, DateTime Start, DateTime End, ExpectedException ExpectedException)
            : ThrowsCase<(DateTime Start, DateTime End)>(Name, (Start, End), ExpectedException);
    }

    public static class TryCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc ok", (new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc)), true, new DateTimeRange(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc))),
            new("unspecified+utc allowed", (new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc)), true, new DateTimeRange(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc)), false, default),
            new("end null", (new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), null), false, default),
            new("start after end", (new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc)), false, default),
            new("utc/local mismatch", (new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Local)), false, default)
        ];

        public sealed record ValidCase(string Name, (DateTime? Start, DateTime? End) Input, bool Expected, DateTimeRange ExpectedOutValue)
            : TryCase<(DateTime? Start, DateTime? End), DateTimeRange>(Name, Input, Expected, ExpectedOutValue);
    }

    public static class Equality
    {
        public static TheoryData<Case> Cases =>
        [
            new("Same Instance", new DateTimeRange(DateTime.MinValue, DateTime.MaxValue), new DateTimeRange(DateTime.MinValue, DateTime.MaxValue), true),
            new("Diff End", new DateTimeRange(DateTime.MinValue, DateTime.MaxValue), new DateTimeRange(DateTime.MinValue, DateTime.MaxValue.AddDays(-1)), false),
            new("Diff Start", new DateTimeRange(DateTime.MinValue, DateTime.MaxValue), new DateTimeRange(DateTime.MinValue.AddDays(1), DateTime.MaxValue), false)
        ];

        public sealed record Case(string Name, DateTimeRange Left, DateTimeRange Right, bool Expected)
            : ValueCase<(DateTimeRange Left, DateTimeRange Right)>(Name, (Left, Right));
    }

    public static class Intersect
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var start = new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc);
                var range = new DateTimeRange(start, start.AddDays(10));
                var before = new DateTimeRange(start.AddDays(-9), start.AddDays(5));
                var after = new DateTimeRange(start.AddDays(5), start.AddDays(20));
                var nonOverlap = new DateTimeRange(start.AddDays(20), start.AddDays(30));

                return
                [
                    new Case("Overlap Before", range, before, new DateTimeRange(start, start.AddDays(5))),
                    new Case("Overlap After", range, after, new DateTimeRange(start.AddDays(5), start.AddDays(10))),
                    new Case("No Overlap", range, nonOverlap, null)
                ];
            }
        }

        public sealed record Case(string Name, DateTimeRange Base, DateTimeRange Other, DateTimeRange? Expected)
            : ReturnCase<(DateTimeRange Base, DateTimeRange Other), DateTimeRange?>(Name, (Base, Other), Expected);
    }

    public static class Union
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var start = new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc);
                var range = new DateTimeRange(start, start.AddDays(10));
                var before = new DateTimeRange(start.AddDays(-9), start.AddDays(5));
                var after = new DateTimeRange(start.AddDays(5), start.AddDays(20));

                return
                [
                    new Case("Overlap Before", range, before, new DateTimeRange(start.AddDays(-9), start.AddDays(10))),
                    new Case("Overlap After", range, after, new DateTimeRange(start, start.AddDays(20)))
                ];
            }
        }

        public sealed record Case(string Name, DateTimeRange Base, DateTimeRange Other, DateTimeRange Expected)
            : ReturnCase<(DateTimeRange Base, DateTimeRange Other), DateTimeRange>(Name, (Base, Other), Expected);
    }

    public static class Overlaps
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateTimeRange(new DateTime(2024, 1, 10), new DateTime(2024, 1, 20));
                var before = new DateTimeRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 10)); // touches start
                var overlap = new DateTimeRange(new DateTime(2024, 1, 5), new DateTime(2024, 1, 15)); // genuinely overlaps
                var after = new DateTimeRange(new DateTime(2024, 1, 20), new DateTime(2024, 1, 30)); // touches end
                var disjointBefore = new DateTimeRange(new DateTime(2024, 1, 1), new DateTime(2024, 1, 5)); // totally before
                var disjointAfter = new DateTimeRange(new DateTime(2024, 1, 25), new DateTime(2024, 1, 30)); // totally after

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

        public sealed record Case(string Name, DateTimeRange Base, DateTimeRange Other, Inclusion Inclusion, bool Expected)
            : ReturnCase<(DateTimeRange Base, DateTimeRange Other), bool>(Name, (Base, Other), Expected);
    }

    public static class Adjacency
    {
        public static TheoryData<Case> Cases
        {
            get
            {
                var range = new DateTimeRange(new DateTime(2024, 1, 10), new DateTime(2024, 1, 20));
                var touchesAtEnd = new DateTimeRange(range.End, range.End);
                var touchesAtStart = new DateTimeRange(range.Start, range.Start);

                return
                [
                    new Case("Touches at End", range, touchesAtEnd, true),
                    new Case("Touches at Start", range, touchesAtStart, true),
                    new Case("Self (Empty)", range, range, false) // range duration > 0, so not adjacent to self
                ];
            }
        }

        public sealed record Case(string Name, DateTimeRange Base, DateTimeRange Other, bool Expected)
            : ReturnCase<(DateTimeRange Base, DateTimeRange Other), bool>(Name, (Base, Other), Expected);
    }
}
