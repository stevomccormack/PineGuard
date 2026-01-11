using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("two days, exclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), Inclusion.Exclusive, true),
            new("same day, inclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Inclusive, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, Inclusion.Exclusive, false),
            new("same day, exclusive", new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01)), Inclusion.Exclusive, false),
        ];

        public sealed record Case(string Name, DateOnlyRange? Value, Inclusion Inclusion, bool ExpectedReturn)
            : IsCase<DateOnlyRange?>(Name, Value, ExpectedReturn);
    }

    public static class IsOverlapping
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("touching boundary, inclusive", (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), new DateOnlyRange(new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03))), Inclusion.Inclusive, true),
            new("touching boundary, exclusive", (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), new DateOnlyRange(new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03))), Inclusion.Exclusive, false),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("both null", (null, null), Inclusion.Exclusive, false),
            new("range2 null", (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), null), Inclusion.Exclusive, false),
            new("range1 null", (null, new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02))), Inclusion.Exclusive, false),
        ];

        public sealed record Case(
            string Name,
            (DateOnlyRange? Range1, DateOnlyRange? Range2) Value,
            Inclusion Inclusion,
            bool ExpectedReturn)
            : IsCase<(DateOnlyRange? Range1, DateOnlyRange? Range2)>(Name, Value, ExpectedReturn);
    }
}
