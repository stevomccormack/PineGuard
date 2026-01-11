using PineGuard.Common;
using PineGuard.Testing.UnitTests;

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
                return
                [
                    new("Two days ago", now.AddDays(-2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                return
                [
                    new("Null", null, false),
                ];
            }
        }

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var now = DateTimeOffset.UtcNow;
                return
                [
                    new("In two days", now.AddDays(2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                return
                [
                    new("Null", null, false),
                ];
            }
        }

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Middle inclusive", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
            new("Min inclusive", new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Min exclusive", new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive, false),
            new("Null", null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Min, DateTimeOffset Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBefore
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Before inclusive", new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
            new("Same instant inclusive", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Same instant exclusive", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive, false),
            new("Null", null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsAfter
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("After inclusive", new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
            new("Same instant inclusive", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Same instant exclusive", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive, false),
            new("Null", null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsSame
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Same instant", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Different instant", new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), false),
            new("Null", null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeOffset? Value, DateTimeOffset Other, bool ExpectedReturn)
            : IsCase<DateTimeOffset?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Increasing exclusive", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, true),
            new("Same instant inclusive", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Both null", (null, null), Inclusion.Exclusive, false),
            new("End null", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null), Inclusion.Exclusive, false),
            new("Start null", (null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
            new("Same instant exclusive", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, (DateTimeOffset? Start, DateTimeOffset? End) Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<(DateTimeOffset? Start, DateTimeOffset? End)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Touching inclusive", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Touching exclusive", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),

            new("All null", (null, null, null, null), Inclusion.Exclusive, false),
            new("End1 null", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
            new("Start2 null", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
            new("End2 null", (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (DateTimeOffset? Start1, DateTimeOffset? End1, DateTimeOffset? Start2, DateTimeOffset? End2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateTimeOffset? Start1, DateTimeOffset? End1, DateTimeOffset? Start2, DateTimeOffset? End2)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
