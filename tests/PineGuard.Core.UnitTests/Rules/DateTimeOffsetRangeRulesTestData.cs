using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("start<end exclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, true),
            new("start==end inclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Inclusive, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("null", null, Inclusion.Exclusive, false),
            new("start==end exclusive", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffsetRange? Range, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new(
                "touching inclusive",
                new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)),
                new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero)),
                Inclusion.Inclusive,
                true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new(
                "touching exclusive",
                new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)),
                new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero)),
                Inclusion.Exclusive,
                false),
            new("both null", null, null, Inclusion.Exclusive, false),
            new("range2 null", new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), null, Inclusion.Exclusive, false),
            new("range1 null", null, new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), Inclusion.Exclusive, false),
        };

        #region Cases

        public sealed record Case(string Name, DateTimeOffsetRange? Range1, DateTimeOffsetRange? Range2, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
