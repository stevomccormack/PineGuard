using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Common;

public static class TestTimeZonesTestData
{
    public static class Zones_HaveExpectedShape
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(TestTimeZones.PlusTen), TestTimeZones.PlusTen, (TimeSpan.FromHours(10), false)),
            new(nameof(TestTimeZones.MinusEight), TestTimeZones.MinusEight, (TimeSpan.FromHours(-8), false)),
            new(nameof(TestTimeZones.DaylightSaving), TestTimeZones.DaylightSaving, (TimeSpan.FromHours(1), true))
        ];

        public sealed record Case(string Name, TimeZoneInfo Value, (TimeSpan baseUtcOffset, bool supportsDaylightSavingTime) Expected)
            : ReturnCase<TimeZoneInfo, (TimeSpan baseUtcOffset, bool supportsDaylightSavingTime)>(Name, Value, Expected);
    }

    public static class DaylightSaving_ClassifiesLocalTimes
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(TestTimeZones.InvalidLocalTime), (TestTimeZones.DaylightSaving, TestTimeZones.InvalidLocalTime), (true, false)),
            new(nameof(TestTimeZones.AmbiguousLocalTime), (TestTimeZones.DaylightSaving, TestTimeZones.AmbiguousLocalTime), (false, true)),
            new(nameof(TestTimeZones.OrdinaryLocalTime), (TestTimeZones.DaylightSaving, TestTimeZones.OrdinaryLocalTime), (false, false)),
            new(nameof(TestTimeZones.HalfHourInvalidLocalTime), (TestTimeZones.HalfHourDaylightSaving, TestTimeZones.HalfHourInvalidLocalTime), (true, false)),
            new(nameof(TestTimeZones.HalfHourAmbiguousLocalTime), (TestTimeZones.HalfHourDaylightSaving, TestTimeZones.HalfHourAmbiguousLocalTime), (false, true)),
            new(nameof(TestTimeZones.HalfHourOrdinaryLocalTime), (TestTimeZones.HalfHourDaylightSaving, TestTimeZones.HalfHourOrdinaryLocalTime), (false, false))
        ];

        public sealed record Case(string Name, (TimeZoneInfo zone, DateTime localTime) Value, (bool isInvalid, bool isAmbiguous) Expected)
            : ReturnCase<(TimeZoneInfo zone, DateTime localTime), (bool isInvalid, bool isAmbiguous)>(Name, Value, Expected);
    }

    public static class GetPlatformSampleId
    {
        private const bool Windows = true;
        private const bool NonWindows = false;

        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(Windows), Windows, "Pacific Standard Time"),
            new(nameof(NonWindows), NonWindows, "America/Los_Angeles")
        ];

        public sealed record Case(string Name, bool Value, string Expected)
            : ReturnCase<bool, string>(Name, Value, Expected);
    }

    public static class PlatformSampleId_Resolves
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(TestTimeZones.PlatformSampleId), TestTimeZones.PlatformSampleId, true),
            new(nameof(TestTimeZones.UnknownId), TestTimeZones.UnknownId, false)
        ];

        public sealed record Case(string Name, string Value, bool Expected)
            : ReturnCase<string, bool>(Name, Value, Expected);
    }

    public static class PlatformSample_ClassifiesLocalTimes
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(TestTimeZones.PlatformInvalidLocalTime), TestTimeZones.PlatformInvalidLocalTime, (true, false)),
            new(nameof(TestTimeZones.PlatformAmbiguousLocalTime), TestTimeZones.PlatformAmbiguousLocalTime, (false, true))
        ];

        public sealed record Case(string Name, DateTime Value, (bool isInvalid, bool isAmbiguous) Expected)
            : ReturnCase<DateTime, (bool isInvalid, bool isAmbiguous)>(Name, Value, Expected);
    }
}
