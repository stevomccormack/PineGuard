using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("start<end exclusive", new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, true),
            new("start==end inclusive", new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, Inclusion.Exclusive, false),
            new("start==end exclusive", new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeRange? Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTimeRange?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("touching inclusive", (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), new DateTimeRange(new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc))), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching exclusive", (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), new DateTimeRange(new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc))), Inclusion.Exclusive, false),
            new("both null", (null, null), Inclusion.Exclusive, false),
            new("range2 null", (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), null), Inclusion.Exclusive, false),
            new("range1 null", (null, new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc))), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (DateTimeRange? Range1, DateTimeRange? Range2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateTimeRange? Range1, DateTimeRange? Range2)>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
