using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Common;

public static class CountingTimeProviderTestData
{
    public static class GetUtcNow
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("midday utc", new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)),
            new("nonzero positive offset normalizes to UTC", new DateTimeOffset(2026, 06, 15, 14, 0, 0, TimeSpan.FromHours(2)), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)),
            new("nonzero negative offset normalizes to UTC", new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.FromHours(-8)), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero))
        ];

        public sealed record Case(string Name, DateTimeOffset Value, DateTimeOffset Expected)
            : ReturnCase<DateTimeOffset, DateTimeOffset>(Name, Value, Expected);
    }

    public static class LocalTimeZone
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null zone defaults to UTC", new CountingTimeProvider(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)), TimeZoneInfo.Utc),
            new("custom zone", new CountingTimeProvider(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), TestTimeZones.PlusTen), TestTimeZones.PlusTen)
        ];

        public sealed record Case(string Name, CountingTimeProvider Value, TimeZoneInfo Expected)
            : ReturnCase<CountingTimeProvider, TimeZoneInfo>(Name, Value, Expected);
    }
}
