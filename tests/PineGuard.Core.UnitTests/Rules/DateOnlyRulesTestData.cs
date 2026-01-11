using PineGuard.Common;
using PineGuard.Testing.UnitTests;

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
                return
                [
                    new("Two days ago", today.AddDays(-2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new("Today", today, false),
                    new("Null", null, false),
                ];
            }
        }

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsInFuture
    {
        public static TheoryData<Case> ValidCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new("In two days", today.AddDays(2), true),
                ];
            }
        }

        public static TheoryData<Case> EdgeCases
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                return
                [
                    new("Today", today, false),
                    new("Null", null, false),
                ];
            }
        }

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Middle inclusive", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive, true),
            new("Min inclusive", new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive, true),
            new("Min exclusive", new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Exclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null value", null, new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, DateOnly Min, DateOnly Max, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBefore
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Before inclusive", new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), Inclusion.Inclusive, true),
            new("Same day inclusive", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Inclusive, true),
            new("Same day exclusive", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Exclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null value", null, new DateOnly(2024, 01, 02), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsAfter
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("After inclusive", new DateOnly(2024, 01, 03), new DateOnly(2024, 01, 02), Inclusion.Inclusive, true),
            new("Same day inclusive", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Inclusive, true),
            new("Same day exclusive", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Exclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null value", null, new DateOnly(2024, 01, 02), Inclusion.Inclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsSame
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Same day", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), true),
            new("Different day", new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03), false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null value", null, new DateOnly(2024, 01, 02), false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateOnly? Value, DateOnly Other, bool ExpectedReturn)
            : IsCase<DateOnly?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Increasing exclusive", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), Inclusion.Exclusive, true),
            new("Same day inclusive", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Both null", (null, null), Inclusion.Exclusive, false),
            new("End null", (new DateOnly(2024, 01, 01), null), Inclusion.Exclusive, false),
            new("Start null", (null, new DateOnly(2024, 01, 01)), Inclusion.Exclusive, false),
            new("Same day exclusive", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, (DateOnly? Start, DateOnly? End) Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<(DateOnly? Start, DateOnly? End)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Touching inclusive", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03)), Inclusion.Inclusive, true),
            new("Touching exclusive", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03)), Inclusion.Exclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("All null", (null, null, null, null), Inclusion.Exclusive, false),
            new("End1 null", (new DateOnly(2024, 01, 01), null, new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), Inclusion.Exclusive, false),
            new("Start2 null", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), null, new DateOnly(2024, 01, 02)), Inclusion.Exclusive, false),
            new("End2 null", (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 01), null), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (DateOnly? Start1, DateOnly? End1, DateOnly? Start2, DateOnly? End2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateOnly? Start1, DateOnly? End1, DateOnly? Start2, DateOnly? End2)>(Name, Value, ExpectedReturn);

        #endregion
    }
}
