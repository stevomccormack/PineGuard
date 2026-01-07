namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing;

public static class DateOnlyRangeTestData
{
    public static class Constructor
    {
        private static ValidCase V(string name, DateOnly start, DateOnly end, int expectedDayCount)
            => new(Name: name, Start: start, End: end, ExpectedDayCount: expectedDayCount);

        private static InvalidCase I(string name, DateOnly start, DateOnly end)
            => new(Name: name, Start: start, End: end, ExpectedException: new ExpectedException(typeof(ArgumentException), ParamName: "start", MessageContains: "less than or equal"));

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("same day", new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 01), 1) },
            { V("one day span", new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 02), 2) },
            { V("ten day span", new DateOnly(2022, 03, 01), new DateOnly(2022, 03, 10), 10) },
            { V("leap day same", new DateOnly(2020, 02, 29), new DateOnly(2020, 02, 29), 1) },
            { V("leap day across months", new DateOnly(2020, 02, 28), new DateOnly(2020, 03, 01), 3) },
            { V("month boundary", new DateOnly(2019, 12, 31), new DateOnly(2020, 01, 01), 2) },
            { V("30 day month", new DateOnly(2020, 06, 01), new DateOnly(2020, 06, 30), 30) },
            { V("31 day month", new DateOnly(2020, 07, 01), new DateOnly(2020, 07, 31), 31) },
            { V("full leap year", new DateOnly(2024, 01, 01), new DateOnly(2024, 12, 31), 366) },
            { V("full non-leap year", new DateOnly(2025, 01, 01), new DateOnly(2025, 12, 31), 365) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("start after end (day)", new DateOnly(2020, 01, 02), new DateOnly(2020, 01, 01)) },
            { I("start after end (month)", new DateOnly(2020, 12, 31), new DateOnly(2020, 01, 01)) },
            { I("start after end (year)", new DateOnly(2001, 01, 01), new DateOnly(2000, 12, 31)) },
            { I("start after end (leap)", new DateOnly(2024, 02, 29), new DateOnly(2024, 02, 28)) },
            { I("start after end (random)", new DateOnly(2032, 11, 03), new DateOnly(2032, 11, 02)) },
            { I("min+1 before min", DateOnly.MinValue.AddDays(1), DateOnly.MinValue) },
            { I("max before max-1", DateOnly.MaxValue, DateOnly.MaxValue.AddDays(-1)) },
            { I("max before min", DateOnly.MaxValue, DateOnly.MinValue) },
            { I("leap march before feb 29", new DateOnly(2024, 03, 01), new DateOnly(2024, 02, 29)) },
            { I("year end before year start", new DateOnly(2025, 12, 31), new DateOnly(2025, 01, 01)) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("min-min", DateOnly.MinValue, DateOnly.MinValue, 1) },
            { V("min+1", DateOnly.MinValue, DateOnly.MinValue.AddDays(1), 2) },
            { V("max-max", DateOnly.MaxValue, DateOnly.MaxValue, 1) },
            { V("max-1", DateOnly.MaxValue.AddDays(-1), DateOnly.MaxValue, 2) },
            { V("leap boundary", new DateOnly(2024, 02, 28), new DateOnly(2024, 02, 29), 2) },
        };

        #region Cases

        public record Case(string Name, DateOnly Start, DateOnly End);

        public sealed record ValidCase(string Name, DateOnly Start, DateOnly End, int ExpectedDayCount)
            : Case(Name, Start, End);

        public record InvalidCase(string Name, DateOnly Start, DateOnly End, ExpectedException ExpectedException)
            : Case(Name, Start, End);

        #endregion
    }
}
