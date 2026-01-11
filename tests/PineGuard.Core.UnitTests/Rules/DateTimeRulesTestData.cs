using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeRulesTestData
{
    public static class IsInPast
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new("past", now.AddDays(-2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases => [];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new("future", now.AddDays(2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases => [];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("middle_inclusive", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
            new("min_inclusive", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("min_exclusive", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, DateTime Min, DateTime Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("chronological_exclusive", (Start: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, true),
            new("same_inclusive", (Start: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null_null", (Start: null, End: null), Inclusion.Exclusive, false),
            new("start_nullEnd", (Start: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End: null), Inclusion.Exclusive, false),
            new("nullStart_end", (Start: null, End: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
            new("same_exclusive", (Start: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, (DateTime? Start, DateTime? End) Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<(DateTime? Start, DateTime? End)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("touching_inclusive", (Start1: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Start2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), End2: new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching_exclusive", (Start1: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Start2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), End2: new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),

            // First overlap comparison false (start1 >= end2)
            new("disjoint_exclusive", (Start1: new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 04, 0, 0, 0, DateTimeKind.Utc), Start2: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
            new("disjoint_inclusive", (Start1: new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 04, 0, 0, 0, DateTimeKind.Utc), Start2: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Inclusive, false),

            new("all_null", (Start1: null, End1: null, Start2: null, End2: null), Inclusion.Exclusive, false),
            new("null_end1", (Start1: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End1: null, Start2: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
            new("null_start2", (Start1: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Start2: null, End2: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
            new("null_end2", (Start1: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End1: new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Start2: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), End2: null), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (DateTime? Start1, DateTime? End1, DateTime? Start2, DateTime? End2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateTime? Start1, DateTime? End1, DateTime? Start2, DateTime? End2)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBefore
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("before", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
            new("same_inclusive", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("same_exclusive", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, DateTime Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsAfter
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("after", new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
            new("same_inclusive", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("same_exclusive", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, DateTime Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsSame
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("same_utc", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), true),
            new("same_unspecified_vs_utc", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("different", new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, DateTime Other, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsWithinDaysFromNow
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new("within_future", now.AddHours(12), 1, true),
                    new("within_past", now.AddHours(-12), 1, true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                var now = DateTime.UtcNow;
                return
                [
                    new("outside_window", now.AddDays(5), 1, false),
                    new("null", null, 1, false),
                    new("negative_days", now, -1, false),
                ];
            }
        }

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, int Days, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsWeekday
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("monday", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), true), // Monday
            new("null", null, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("saturday", new DateTime(2024, 01, 06, 0, 0, 0, DateTimeKind.Utc), false), // Saturday
            new("sunday", new DateTime(2024, 01, 07, 0, 0, 0, DateTimeKind.Utc), false), // Sunday
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsWeekend
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("saturday", new DateTime(2024, 01, 06, 0, 0, 0, DateTimeKind.Utc), true), // Saturday
            new("null", null, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("monday", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), false), // Monday
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsFirstDayOfMonth
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("first_day", new DateTime(2024, 02, 01, 12, 0, 0, DateTimeKind.Utc), true),
            new("null", null, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("not_first", new DateTime(2024, 02, 02, 12, 0, 0, DateTimeKind.Utc), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsLastDayOfMonth
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("last_day", new DateTime(2024, 02, 29, 12, 0, 0, DateTimeKind.Utc), true),
            new("null", null, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("not_last", new DateTime(2024, 02, 28, 12, 0, 0, DateTimeKind.Utc), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsSameDay
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("same_day", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 23, 59, 59, DateTimeKind.Utc), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null_null", null, null, false),
            new("value_null", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, false),
            new("null_other", null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), false),
            new("different_day", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime? Value, DateTime? Other, bool ExpectedReturn)
            : IsCase<DateTime?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class KindChecks
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("utc", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), DateTimeKind.Utc, true),
            new("local", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Local), DateTimeKind.Local, true),
            new("unspecified", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), DateTimeKind.Unspecified, true),
        ];

        public static TheoryData<Case> EdgeCases => [];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, DateTimeKind Kind, bool ExpectedReturn)
            : IsCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class HasExplicitKind
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("utc", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), true),
            new("local", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Local), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("unspecified", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTime Value, bool ExpectedReturn)
            : HasCase<DateTime>(Name, Value, ExpectedReturn);

        #endregion
    }
}
