using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Common;

public static class AdvancingTimeProviderTestData
{
    public static class GetUtcNow
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("single item sequence", [new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)], [new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)]),
            new("multiple items consumed in order", [new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 16, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 17, 12, 0, 0, TimeSpan.Zero)], [new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 16, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 17, 12, 0, 0, TimeSpan.Zero)]),
            new("nonzero offsets normalize to UTC", [new DateTimeOffset(2026, 06, 15, 14, 0, 0, TimeSpan.FromHours(2)), new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.FromHours(-8))], [new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)])
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("empty sequence throws on first read", ([], 0), new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase("single item sequence throws on resample", ([new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)], 1), new ExpectedException(typeof(InvalidOperationException)))
        ];

        public sealed record Case(string Name, DateTimeOffset[] Value, DateTimeOffset[] Expected)
            : ReturnCase<DateTimeOffset[], DateTimeOffset[]>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (DateTimeOffset[] sequence, int successfulReads) Value, ExpectedException ExpectedException)
            : ThrowsCase<(DateTimeOffset[] sequence, int successfulReads)>(Name, Value, ExpectedException);
    }

    public static class LocalTimeZone
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("null zone defaults to UTC", new AdvancingTimeProvider([new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)]), TimeZoneInfo.Utc),
            new("custom zone", new AdvancingTimeProvider([new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero)], TestTimeZones.MinusEight), TestTimeZones.MinusEight)
        ];

        public sealed record Case(string Name, AdvancingTimeProvider Value, TimeZoneInfo Expected)
            : ReturnCase<AdvancingTimeProvider, TimeZoneInfo>(Name, Value, Expected);
    }
}
