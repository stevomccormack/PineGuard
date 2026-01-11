using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("start<end exclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, true),
            new("start==end inclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, Inclusion.Exclusive, false),
            new("start==end exclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(string Name, DateTimeOffsetRange? Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateTimeOffsetRange?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("touching inclusive", (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero))), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("touching exclusive", (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero))), Inclusion.Exclusive, false),
            new("both null", (null, null), Inclusion.Exclusive, false),
            new("range2 null", (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), null), Inclusion.Exclusive, false),
            new("range1 null", (null, new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero))), Inclusion.Exclusive, false),
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            (DateTimeOffsetRange? Range1, DateTimeOffsetRange? Range2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateTimeOffsetRange? Range1, DateTimeOffsetRange? Range2)>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
