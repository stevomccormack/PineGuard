namespace PineGuard.Core.UnitTests.Common;

using PineGuard.Testing;

public static class TimeOnlyRangeTestData
{
    public static class Constructor
    {
        private static ValidCase V(string name, TimeOnly start, TimeOnly end, TimeSpan expectedDuration)
            => new(Name: name, Start: start, End: end, ExpectedDuration: expectedDuration);

        private static InvalidCase I(string name, TimeOnly start, TimeOnly end)
            => new(Name: name, Start: start, End: end, ExpectedException: new ExpectedException(typeof(ArgumentException), ParamName: "start", MessageContains: "less than or equal"));

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("same second", new TimeOnly(00, 00, 00), new TimeOnly(00, 00, 00), TimeSpan.Zero) },
            { V("one second", new TimeOnly(00, 00, 00), new TimeOnly(00, 00, 01), TimeSpan.FromSeconds(1)) },
            { V("midday span", new TimeOnly(12, 00, 00), new TimeOnly(13, 30, 00), TimeSpan.FromMinutes(90)) },
            { V("workday", new TimeOnly(08, 00, 00), new TimeOnly(17, 00, 00), TimeSpan.FromHours(9)) },
            { V("last second", new TimeOnly(23, 59, 59), new TimeOnly(23, 59, 59), TimeSpan.Zero) },
            { V("whole day", new TimeOnly(00, 00, 00), new TimeOnly(23, 59, 59), TimeSpan.FromSeconds(86399)) },
            { V("01:00:00 to 01:00:01", new TimeOnly(01, 00, 00), new TimeOnly(01, 00, 01), TimeSpan.FromSeconds(1)) },
            { V("02:00:00 to 02:00:02", new TimeOnly(02, 00, 00), new TimeOnly(02, 00, 02), TimeSpan.FromSeconds(2)) },
            { V("03:00:00 to 03:00:03", new TimeOnly(03, 00, 00), new TimeOnly(03, 00, 03), TimeSpan.FromSeconds(3)) },
            { V("04:00:00 to 04:00:04", new TimeOnly(04, 00, 00), new TimeOnly(04, 00, 04), TimeSpan.FromSeconds(4)) },
            { V("05:00:00 to 05:00:05", new TimeOnly(05, 00, 00), new TimeOnly(05, 00, 05), TimeSpan.FromSeconds(5)) },
            { V("06:00:00 to 06:00:06", new TimeOnly(06, 00, 00), new TimeOnly(06, 00, 06), TimeSpan.FromSeconds(6)) },
            { V("07:00:00 to 07:00:07", new TimeOnly(07, 00, 00), new TimeOnly(07, 00, 07), TimeSpan.FromSeconds(7)) },
            { V("08:00:00 to 08:00:08", new TimeOnly(08, 00, 00), new TimeOnly(08, 00, 08), TimeSpan.FromSeconds(8)) },
            { V("09:00:00 to 09:00:09", new TimeOnly(09, 00, 00), new TimeOnly(09, 00, 09), TimeSpan.FromSeconds(9)) },
            { V("10:00:00 to 10:00:10", new TimeOnly(10, 00, 00), new TimeOnly(10, 00, 10), TimeSpan.FromSeconds(10)) },
            { V("11:00:00 to 11:00:11", new TimeOnly(11, 00, 00), new TimeOnly(11, 00, 11), TimeSpan.FromSeconds(11)) },
            { V("12:00:00 to 12:00:12", new TimeOnly(12, 00, 00), new TimeOnly(12, 00, 12), TimeSpan.FromSeconds(12)) },
            { V("13:00:00 to 13:00:13", new TimeOnly(13, 00, 00), new TimeOnly(13, 00, 13), TimeSpan.FromSeconds(13)) },
            { V("14:00:00 to 14:00:14", new TimeOnly(14, 00, 00), new TimeOnly(14, 00, 14), TimeSpan.FromSeconds(14)) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("start after end (second)", new TimeOnly(00, 00, 01), new TimeOnly(00, 00, 00)) },
            { I("start after end (midday)", new TimeOnly(13, 30, 00), new TimeOnly(12, 00, 00)) },
            { I("start after end (workday)", new TimeOnly(17, 00, 00), new TimeOnly(08, 00, 00)) },
            { I("start after end (day wrap)", new TimeOnly(23, 59, 59), new TimeOnly(00, 00, 00)) },
            { I("01:00:01 before 01:00:00", new TimeOnly(01, 00, 01), new TimeOnly(01, 00, 00)) },
            { I("02:00:02 before 02:00:00", new TimeOnly(02, 00, 02), new TimeOnly(02, 00, 00)) },
            { I("03:00:03 before 03:00:00", new TimeOnly(03, 00, 03), new TimeOnly(03, 00, 00)) },
            { I("04:00:04 before 04:00:00", new TimeOnly(04, 00, 04), new TimeOnly(04, 00, 00)) },
            { I("05:00:05 before 05:00:00", new TimeOnly(05, 00, 05), new TimeOnly(05, 00, 00)) },
            { I("06:00:06 before 06:00:00", new TimeOnly(06, 00, 06), new TimeOnly(06, 00, 00)) },
            { I("07:00:07 before 07:00:00", new TimeOnly(07, 00, 07), new TimeOnly(07, 00, 00)) },
            { I("08:00:08 before 08:00:00", new TimeOnly(08, 00, 08), new TimeOnly(08, 00, 00)) },
            { I("09:00:09 before 09:00:00", new TimeOnly(09, 00, 09), new TimeOnly(09, 00, 00)) },
            { I("10:00:10 before 10:00:00", new TimeOnly(10, 00, 10), new TimeOnly(10, 00, 00)) },
            { I("11:00:11 before 11:00:00", new TimeOnly(11, 00, 11), new TimeOnly(11, 00, 00)) },
            { I("12:00:12 before 12:00:00", new TimeOnly(12, 00, 12), new TimeOnly(12, 00, 00)) },
            { I("13:00:13 before 13:00:00", new TimeOnly(13, 00, 13), new TimeOnly(13, 00, 00)) },
            { I("14:00:14 before 14:00:00", new TimeOnly(14, 00, 14), new TimeOnly(14, 00, 00)) },
            { I("15:00:15 before 15:00:00", new TimeOnly(15, 00, 15), new TimeOnly(15, 00, 00)) },
            { I("16:00:16 before 16:00:00", new TimeOnly(16, 00, 16), new TimeOnly(16, 00, 00)) },
            { I("min+1 tick before min", TimeOnly.MinValue.Add(TimeSpan.FromTicks(1)), TimeOnly.MinValue) },
            { I("max before max-1 tick", TimeOnly.MaxValue, TimeOnly.MaxValue.Add(TimeSpan.FromTicks(-1))) },
            { I("23:59:59 before 23:59:58", new TimeOnly(23, 59, 59), new TimeOnly(23, 59, 58)) },
            { I("00:00:10 before 00:00:00", new TimeOnly(00, 00, 10), new TimeOnly(00, 00, 00)) },
            { I("00:01:00 before 00:00:59", new TimeOnly(00, 01, 00), new TimeOnly(00, 00, 59)) },
            { I("01:00:00 before 00:59:59", new TimeOnly(01, 00, 00), new TimeOnly(00, 59, 59)) },
            { I("02:00:00 before 01:59:59", new TimeOnly(02, 00, 00), new TimeOnly(01, 59, 59)) },
            { I("03:00:00 before 02:59:59", new TimeOnly(03, 00, 00), new TimeOnly(02, 59, 59)) },
            { I("04:00:00 before 03:59:59", new TimeOnly(04, 00, 00), new TimeOnly(03, 59, 59)) },
            { I("05:00:00 before 04:59:59", new TimeOnly(05, 00, 00), new TimeOnly(04, 59, 59)) },
            { I("06:00:00 before 05:59:59", new TimeOnly(06, 00, 00), new TimeOnly(05, 59, 59)) },
            { I("07:00:00 before 06:59:59", new TimeOnly(07, 00, 00), new TimeOnly(06, 59, 59)) },
            { I("08:00:00 before 07:59:59", new TimeOnly(08, 00, 00), new TimeOnly(07, 59, 59)) },
            { I("09:00:00 before 08:59:59", new TimeOnly(09, 00, 00), new TimeOnly(08, 59, 59)) },
            { I("10:00:00 before 09:59:59", new TimeOnly(10, 00, 00), new TimeOnly(09, 59, 59)) },
            { I("11:00:00 before 10:59:59", new TimeOnly(11, 00, 00), new TimeOnly(10, 59, 59)) },
            { I("12:00:00 before 11:59:59", new TimeOnly(12, 00, 00), new TimeOnly(11, 59, 59)) },
            { I("13:00:00 before 12:59:59", new TimeOnly(13, 00, 00), new TimeOnly(12, 59, 59)) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("min-min", TimeOnly.MinValue, TimeOnly.MinValue, TimeSpan.Zero) },
            { V("max-max", TimeOnly.MaxValue, TimeOnly.MaxValue, TimeSpan.Zero) },
            { V("min to +1 second", TimeOnly.MinValue, new TimeOnly(00, 00, 01), TimeSpan.FromSeconds(1)) },
            { V("near max to max", new TimeOnly(23, 59, 58), TimeOnly.MaxValue, TimeOnly.MaxValue.ToTimeSpan() - new TimeOnly(23, 59, 58).ToTimeSpan()) },
        };

        #region Cases

        public record Case(string Name, TimeOnly Start, TimeOnly End);

        public sealed record ValidCase(string Name, TimeOnly Start, TimeOnly End, TimeSpan ExpectedDuration)
            : Case(Name, Start, End);

        public record InvalidCase(string Name, TimeOnly Start, TimeOnly End, ExpectedException ExpectedException)
            : Case(Name, Start, End);

        #endregion
    }
}
