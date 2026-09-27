using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DateTimeOffsetUtilityTestData
{
    public static class TryGetDateAtOffset
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ZeroOffset", (new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero), TimeSpan.Zero), true, new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)),
            new("PositiveOffsetCrossesToNextDay", (new DateTimeOffset(2026, 06, 15, 20, 0, 0, TimeSpan.Zero), TestTimeZones.PlusTen.BaseUtcOffset), true, new DateTime(2026, 06, 16, 0, 0, 0, DateTimeKind.Unspecified)),
            new("NegativeOffsetCrossesToPreviousDay", (new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.Zero), TestTimeZones.MinusEight.BaseUtcOffset), true, new DateTime(2026, 06, 14, 0, 0, 0, DateTimeKind.Unspecified)),
            new("MaxOffsetExactBoundary", (new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromHours(14)), true, new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)),
            new("MinOffsetExactBoundary", (new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromHours(-14)), true, new DateTime(2026, 06, 14, 0, 0, 0, DateTimeKind.Unspecified)),
            new("MinValueWithPositiveOffsetSucceeds", (DateTimeOffset.MinValue, TimeSpan.FromHours(14)), true, new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)),
            new("MaxValueWithNegativeOffsetSucceeds", (DateTimeOffset.MaxValue, TimeSpan.FromHours(-14)), true, new DateTime(9999, 12, 31, 0, 0, 0, DateTimeKind.Unspecified))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("OffsetAboveFourteenHours", (new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromHours(14) + TimeSpan.FromMinutes(1)), false, default),
            new("OffsetBelowNegativeFourteenHours", (new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromHours(-14) - TimeSpan.FromMinutes(1)), false, default),
            new("OffsetNotWholeMinute", (new DateTimeOffset(2026, 06, 15, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(30)), false, default),
            new("MinValueUnderflowsWithNegativeOffset", (DateTimeOffset.MinValue, TimeSpan.FromHours(-14)), false, default),
            new("MaxValueOverflowsWithPositiveOffset", (DateTimeOffset.MaxValue, TimeSpan.FromHours(14)), false, default)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset instant, TimeSpan offset) Value, bool Expected, DateTime ExpectedOutValue)
            : TryCase<(DateTimeOffset instant, TimeSpan offset), DateTime>(Name, Value, Expected, ExpectedOutValue);
    }
}
