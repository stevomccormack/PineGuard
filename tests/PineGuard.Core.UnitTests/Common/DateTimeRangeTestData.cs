namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

public static class DateTimeRangeTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc same", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("utc +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("local +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Local), TimeSpan.FromSeconds(1)),
            new("utc 90 minutes", new DateTime(2020, 01, 01, 12, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 13, 30, 00, DateTimeKind.Utc), TimeSpan.FromMinutes(90)),
            new("leap day", new DateTime(2020, 02, 29, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(1)),
            new("unspecified same", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified +1 second", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("utc +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), TimeSpan.FromHours(1)),
            new("local +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Local), TimeSpan.FromHours(1)),
            new("local 30 minutes", new DateTime(2023, 11, 05, 01, 00, 00, DateTimeKind.Local), new DateTime(2023, 11, 05, 01, 30, 00, DateTimeKind.Local), TimeSpan.FromMinutes(30)),
            new("utc 2024 year span", new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 12, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(365)),
            new("utc 2025 year span", new DateTime(2025, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(364)),
            new("utc +1 second (2030)", new DateTime(2030, 04, 15, 10, 00, 00, DateTimeKind.Utc), new DateTime(2030, 04, 15, 10, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("unspecified 12 hours", new DateTime(2031, 08, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2031, 08, 01, 12, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromHours(12)),
            new("utc boundary second", new DateTime(2032, 10, 30, 23, 59, 59, DateTimeKind.Utc), new DateTime(2032, 10, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("utc 30 days", new DateTime(1999, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(1999, 01, 31, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(30)),
            new("utc feb 2000", new DateTime(2000, 02, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromDays(28)),
            new("local same", new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("unspecified +2 seconds", new DateTime(2011, 06, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2011, 06, 01, 00, 00, 02, DateTimeKind.Unspecified), TimeSpan.FromSeconds(2)),
            new("utc +2 seconds", new DateTime(2012, 07, 01, 12, 00, 00, DateTimeKind.Utc), new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Utc), TimeSpan.FromSeconds(2)),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("utc -1 second", new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day", new DateTime(2020, 01, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local previous day", new DateTime(2020, 03, 02, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("unspecified previous day", new DateTime(2021, 01, 02, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc hour reversed", new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local hour reversed", new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Local), new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local minute reversed", new DateTime(2023, 11, 05, 01, 30, 00, DateTimeKind.Local), new DateTime(2023, 11, 05, 01, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc -1 second (2024)", new DateTime(2024, 12, 31, 00, 00, 01, DateTimeKind.Utc), new DateTime(2024, 12, 31, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc -1 second (2030)", new DateTime(2030, 04, 15, 10, 00, 01, DateTimeKind.Utc), new DateTime(2030, 04, 15, 10, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (1999)", new DateTime(1999, 01, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(1999, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (2000)", new DateTime(2000, 02, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2000, 02, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local -1 second", new DateTime(2010, 05, 01, 00, 00, 01, DateTimeKind.Local), new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("unspecified -1 second", new DateTime(2011, 06, 01, 00, 00, 02, DateTimeKind.Unspecified), new DateTime(2011, 06, 01, 00, 00, 01, DateTimeKind.Unspecified), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc -1 second (2012)", new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Utc), new DateTime(2012, 07, 01, 12, 00, 01, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (2013)", new DateTime(2013, 01, 10, 00, 00, 00, DateTimeKind.Utc), new DateTime(2013, 01, 09, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (2014)", new DateTime(2014, 06, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2014, 06, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("local previous day (2015)", new DateTime(2015, 08, 16, 00, 00, 00, DateTimeKind.Local), new DateTime(2015, 08, 15, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("unspecified previous day (2016)", new DateTime(2016, 11, 03, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2016, 11, 02, 00, 00, 00, DateTimeKind.Unspecified), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (2017)", new DateTime(2017, 12, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2017, 12, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc previous day (2018)", new DateTime(2018, 10, 02, 00, 00, 00, DateTimeKind.Utc), new DateTime(2018, 10, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc-local", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local leap", new DateTime(2020, 02, 29, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 02, 29, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc leap", new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 03, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc end local", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local end utc", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local (dst-ish)", new DateTime(2023, 11, 05, 01, 00, 00, DateTimeKind.Utc), new DateTime(2023, 11, 05, 01, 30, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc (dst-ish)", new DateTime(2023, 11, 05, 01, 00, 00, DateTimeKind.Local), new DateTime(2023, 11, 05, 01, 30, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local (2024)", new DateTime(2024, 12, 31, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 12, 31, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc (2025)", new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local (1999)", new DateTime(1999, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(1999, 01, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc (2000)", new DateTime(2000, 02, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local (2010)", new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2010, 05, 01, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc (2011)", new DateTime(2011, 06, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2011, 06, 01, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local +2 seconds", new DateTime(2012, 07, 01, 12, 00, 00, DateTimeKind.Utc), new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc +1 day", new DateTime(2013, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2013, 01, 02, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("utc-local +1 day", new DateTime(2014, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2014, 06, 02, 00, 00, 00, DateTimeKind.Local), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
            new("local-utc +1 day", new DateTime(2015, 08, 15, 00, 00, 00, DateTimeKind.Local), new DateTime(2015, 08, 16, 00, 00, 00, DateTimeKind.Utc), new ExpectedException(typeof(ArgumentException), "start", "compatible kind")),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min-min unspecified", new DateTime(0001, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(0001, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("max-max unspecified", new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified), new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("utc with unspecified end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified with utc end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("local with unspecified end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified with local end", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("unspecified to utc +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("utc to unspecified +1 second", new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +1 second (2021)", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("local to unspecified +1 second", new DateTime(2021, 01, 01, 00, 00, 00, DateTimeKind.Local), new DateTime(2021, 01, 01, 00, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Utc), TimeSpan.FromHours(1)),
            new("utc to unspecified +1 hour", new DateTime(2022, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2022, 06, 01, 01, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromHours(1)),
            new("unspecified same", new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.Zero),
            new("unspecified +1 day", new DateTime(2023, 01, 01, 00, 00, 00, DateTimeKind.Unspecified), new DateTime(2023, 01, 02, 00, 00, 00, DateTimeKind.Unspecified), TimeSpan.FromDays(1)),
            new("utc year boundary second", new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc), new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc), TimeSpan.FromSeconds(1)),
            new("leap day same utc", new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Utc), new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Utc), TimeSpan.Zero),
            new("leap day +1 second unspecified", new DateTime(2024, 02, 29, 12, 00, 00, DateTimeKind.Unspecified), new DateTime(2024, 02, 29, 12, 00, 01, DateTimeKind.Unspecified), TimeSpan.FromSeconds(1)),
            new("local same", new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), TimeSpan.Zero),
            new("local +1 second", new DateTime(2025, 12, 31, 00, 00, 00, DateTimeKind.Local), new DateTime(2025, 12, 31, 00, 00, 01, DateTimeKind.Local), TimeSpan.FromSeconds(1)),
            new("unspecified to utc +2 seconds", new DateTime(2012, 07, 01, 12, 00, 00, DateTimeKind.Unspecified), new DateTime(2012, 07, 01, 12, 00, 02, DateTimeKind.Utc), TimeSpan.FromSeconds(2)),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, DateTime Start, DateTime End, TimeSpan ExpectedDuration)
            : ReturnCase<(DateTime Start, DateTime End), TimeSpan>(Name, (Start, End), ExpectedDuration);

        public sealed record InvalidCase(string Name, DateTime Start, DateTime End, ExpectedException ExpectedException)
            : ThrowsCase<(DateTime Start, DateTime End)>(Name, (Start, End), ExpectedException);

        #endregion
    }
}
