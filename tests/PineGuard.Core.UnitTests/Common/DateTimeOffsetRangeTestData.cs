namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing;

public static class DateTimeOffsetRangeTestData
{
    public static class Constructor
    {
        private static ValidCase V(string name, DateTimeOffset start, DateTimeOffset end, TimeSpan expectedDuration)
            => new(Name: name, Start: start, End: end, ExpectedDuration: expectedDuration);

        private static InvalidCase I(string name, DateTimeOffset start, DateTimeOffset end)
            => new(Name: name, Start: start, End: end, ExpectedException: new ExpectedException(typeof(ArgumentException), ParamName: "start", MessageContains: "less than or equal"));

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("utc+00 same", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("utc+00 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("+01 90 minutes", new DateTimeOffset(2020, 01, 01, 12, 00, 00, TimeSpan.FromHours(1)), new DateTimeOffset(2020, 01, 01, 13, 30, 00, TimeSpan.FromHours(1)), TimeSpan.FromMinutes(90)) },
            { V("leap day", new DateTimeOffset(2020, 02, 29, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 03, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(1)) },
            { V("-05 same", new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), TimeSpan.Zero) },
            { V("-05 +1 second", new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 01, TimeSpan.FromHours(-5)), TimeSpan.FromSeconds(1)) },
            { V("utc+00 +1 hour", new DateTimeOffset(2022, 06, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2022, 06, 01, 01, 00, 00, TimeSpan.Zero), TimeSpan.FromHours(1)) },
            { V("-05 30 minutes", new DateTimeOffset(2023, 11, 05, 01, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2023, 11, 05, 01, 30, 00, TimeSpan.FromHours(-5)), TimeSpan.FromMinutes(30)) },
            { V("utc+00 2024 year span", new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 12, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(365)) },
            { V("utc+00 2025 year span", new DateTimeOffset(2025, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(364)) },
            { V("utc+00 +1 second (2030)", new DateTimeOffset(2030, 04, 15, 10, 00, 00, TimeSpan.Zero), new DateTimeOffset(2030, 04, 15, 10, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("+10 12 hours", new DateTimeOffset(2031, 08, 01, 00, 00, 00, TimeSpan.FromHours(10)), new DateTimeOffset(2031, 08, 01, 12, 00, 00, TimeSpan.FromHours(10)), TimeSpan.FromHours(12)) },
            { V("utc+00 boundary second", new DateTimeOffset(2032, 10, 30, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("utc+00 30 days", new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(30)) },
            { V("utc+00 feb 2000", new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(28)) },
            { V("+02 same", new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), TimeSpan.Zero) },
            { V("+03 +2 seconds", new DateTimeOffset(2011, 06, 01, 00, 00, 00, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), TimeSpan.FromSeconds(2)) },
            { V("-03 +2 seconds", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), TimeSpan.FromSeconds(2)) },
            { V("utc+00 +10 seconds", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 10, TimeSpan.Zero), TimeSpan.FromSeconds(10)) },
            { V("utc+00 +1 day", new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(1)) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("utc+00 -1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day", new DateTimeOffset(2020, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (march)", new DateTimeOffset(2020, 03, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 03, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("-05 previous day", new DateTimeOffset(2021, 01, 02, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5))) },
            { I("utc+00 hour reversed", new DateTimeOffset(2022, 06, 01, 01, 00, 00, TimeSpan.Zero), new DateTimeOffset(2022, 06, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("-05 minute reversed", new DateTimeOffset(2023, 11, 05, 01, 30, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2023, 11, 05, 01, 00, 00, TimeSpan.FromHours(-5))) },
            { I("utc+00 -1 second (2024)", new DateTimeOffset(2024, 12, 31, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(2024, 12, 31, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 -1 second (2030)", new DateTimeOffset(2030, 04, 15, 10, 00, 01, TimeSpan.Zero), new DateTimeOffset(2030, 04, 15, 10, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (1999)", new DateTimeOffset(1999, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2000)", new DateTimeOffset(2000, 02, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("+02 -1 second", new DateTimeOffset(2010, 05, 01, 00, 00, 01, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2))) },
            { I("+03 -1 second", new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 01, TimeSpan.FromHours(3))) },
            { I("-03 -1 second", new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 01, TimeSpan.FromHours(-3))) },
            { I("utc+00 previous day (2013)", new DateTimeOffset(2013, 01, 10, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2013, 01, 09, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2014)", new DateTimeOffset(2014, 06, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2014, 06, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2015)", new DateTimeOffset(2015, 08, 16, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2015, 08, 15, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2016)", new DateTimeOffset(2016, 11, 03, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2016, 11, 02, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2017)", new DateTimeOffset(2017, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2017, 12, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2018)", new DateTimeOffset(2018, 10, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2018, 10, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2027)", new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("min+1tick before min", DateTimeOffset.MinValue.AddTicks(1), DateTimeOffset.MinValue) },
            { I("max after max-1tick", DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.AddTicks(-1)) },
            { I("year 1 reversed", new DateTimeOffset(0001, 01, 01, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(0001, 01, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("max date reversed", new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(9999, 12, 31, 23, 59, 58, TimeSpan.Zero)) },
            { I("leap day reversed", new DateTimeOffset(2024, 02, 29, 12, 00, 01, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero)) },
            { I("+14 reversed", new DateTimeOffset(2025, 12, 31, 00, 00, 01, TimeSpan.FromHours(14)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14))) },
            { I("-12 reversed", new DateTimeOffset(2025, 12, 31, 00, 00, 01, TimeSpan.FromHours(-12)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12))) },
            { I("utc+00 reversed (2026)", new DateTimeOffset(2026, 11, 05, 00, 00, 10, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero)) },
            { I("utc+00 previous day (2027)", new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("+05 reversed hour", new DateTimeOffset(2020, 01, 01, 01, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5))) },
            { I("-07 reversed second", new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7))) },
            { I("2032 reversed", new DateTimeOffset(2032, 10, 31, 00, 00, 10, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero)) },
            { I("1999 previous day", new DateTimeOffset(1999, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("2000 previous day", new DateTimeOffset(2000, 02, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("+02 reversed second", new DateTimeOffset(2010, 05, 01, 00, 00, 01, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2))) },
            { I("+03 reversed second", new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 01, TimeSpan.FromHours(3))) },
            { I("-03 reversed second", new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 01, TimeSpan.FromHours(-3))) },
            { I("2013 previous day", new DateTimeOffset(2013, 01, 10, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2013, 01, 09, 00, 00, 00, TimeSpan.Zero)) },
            { I("2014 previous day", new DateTimeOffset(2014, 06, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2014, 06, 01, 00, 00, 00, TimeSpan.Zero)) },
            { I("2015 previous day", new DateTimeOffset(2015, 08, 16, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2015, 08, 15, 00, 00, 00, TimeSpan.Zero)) },
            { I("2016 previous day", new DateTimeOffset(2016, 11, 03, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2016, 11, 02, 00, 00, 00, TimeSpan.Zero)) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("min-min", DateTimeOffset.MinValue, DateTimeOffset.MinValue, TimeSpan.Zero) },
            { V("max-max", DateTimeOffset.MaxValue, DateTimeOffset.MaxValue, TimeSpan.Zero) },
            { V("year 1 +1 second", new DateTimeOffset(0001, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(0001, 01, 01, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("max date same", new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), TimeSpan.Zero) },
            { V("leap day same", new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("leap day +1 second", new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("+14 same", new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14)), TimeSpan.Zero) },
            { V("-12 same", new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12)), TimeSpan.Zero) },
            { V("utc+00 same", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("utc+00 +1 second", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)) },
            { V("+05 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(5)), TimeSpan.FromSeconds(1)) },
            { V("+05 +1 hour", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 01, 00, 00, TimeSpan.FromHours(5)), TimeSpan.FromHours(1)) },
            { V("-07 same", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), TimeSpan.Zero) },
            { V("-07 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(-7)), TimeSpan.FromSeconds(1)) },
            { V("2032 same", new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("2032 +10 seconds", new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 10, TimeSpan.Zero), TimeSpan.FromSeconds(10)) },
            { V("1999 same", new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("2000 leap same", new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero) },
            { V("-03 same", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), TimeSpan.Zero) },
            { V("-03 +2 seconds", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), TimeSpan.FromSeconds(2)) },
        };

        #region Cases

        public record Case(string Name, DateTimeOffset Start, DateTimeOffset End);

        public sealed record ValidCase(string Name, DateTimeOffset Start, DateTimeOffset End, TimeSpan ExpectedDuration)
            : Case(Name, Start, End);

        public record InvalidCase(string Name, DateTimeOffset Start, DateTimeOffset End, ExpectedException ExpectedException)
            : Case(Name, Start, End);

        #endregion
    }
}
