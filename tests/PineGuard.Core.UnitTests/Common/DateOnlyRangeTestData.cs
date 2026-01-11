namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

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
            new("full non-leap year", new DateOnly(2025, 01, 01), new DateOnly(2025, 12, 31), 365),
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
            new("year end before year start", new DateOnly(2025, 12, 31), new DateOnly(2025, 01, 01), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min-min", DateOnly.MinValue, DateOnly.MinValue, 1),
            new("min+1", DateOnly.MinValue, DateOnly.MinValue.AddDays(1), 2),
            new("max-max", DateOnly.MaxValue, DateOnly.MaxValue, 1),
            new("max-1", DateOnly.MaxValue.AddDays(-1), DateOnly.MaxValue, 2),
            new("leap boundary", new DateOnly(2024, 02, 28), new DateOnly(2024, 02, 29), 2),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, DateOnly Start, DateOnly End, int ExpectedDayCount)
            : ReturnCase<(DateOnly Start, DateOnly End), int>(Name, (Start, End), ExpectedDayCount);

        public sealed record InvalidCase(string Name, DateOnly Start, DateOnly End, ExpectedException ExpectedException)
            : ThrowsCase<(DateOnly Start, DateOnly End)>(Name, (Start, End), ExpectedException);

        #endregion
    }
}
