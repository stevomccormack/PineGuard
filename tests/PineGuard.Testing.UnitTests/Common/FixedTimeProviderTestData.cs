using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Common;

public static class FixedTimeProviderTestData
{
    public static class GetUtcNow
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("midday", new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)),
            new("midnight", new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero)),
            new("leap day", new DateTimeOffset(2024, 02, 29, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 23, 59, 59, TimeSpan.Zero)),
            new("min value", DateTimeOffset.MinValue, DateTimeOffset.MinValue),
            new("max value", DateTimeOffset.MaxValue, DateTimeOffset.MaxValue),
            new("nonzero positive offset normalizes to UTC", new DateTimeOffset(2026, 06, 15, 14, 0, 0, TimeSpan.FromHours(2)), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)),
            new("nonzero negative offset normalizes to UTC", new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.FromHours(-8)), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero))
        ];

        public sealed record Case(string Name, DateTimeOffset Value, DateTimeOffset Expected)
            : ReturnCase<DateTimeOffset, DateTimeOffset>(Name, Value, Expected);
    }

    public static class Default
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(FixedTimeProvider.Default), FixedTimeProvider.Default, new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero))
        ];

        public sealed record Case(string Name, FixedTimeProvider Value, DateTimeOffset Expected)
            : ReturnCase<FixedTimeProvider, DateTimeOffset>(Name, Value, Expected);
    }

    public static class LocalTimeZone
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(FixedTimeProvider.Default), FixedTimeProvider.Default, TimeZoneInfo.Utc),
            new(nameof(FixedTimeProvider.DefaultAheadOfUtc), FixedTimeProvider.DefaultAheadOfUtc, TestTimeZones.PlusTen),
            new(nameof(FixedTimeProvider.DefaultBehindUtc), FixedTimeProvider.DefaultBehindUtc, TestTimeZones.MinusEight),
            new("explicit null zone falls through to UTC", new FixedTimeProvider(new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero), null), TimeZoneInfo.Utc)
        ];

        public sealed record Case(string Name, FixedTimeProvider Value, TimeZoneInfo Expected)
            : ReturnCase<FixedTimeProvider, TimeZoneInfo>(Name, Value, Expected);
    }
}
