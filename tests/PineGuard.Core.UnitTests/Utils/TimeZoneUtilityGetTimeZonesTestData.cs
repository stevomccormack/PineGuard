using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class TimeZoneUtilityGetTimeZonesTestData
{
    public sealed class FakeIanaTimeZoneProvider(Dictionary<string, IReadOnlyCollection<string>> zoneIdsByCountryAlpha2)
        : IIanaTimeZoneProvider
    {
        public bool IsValidTimeZoneId(string? value) => false;

        public bool TryGetById(string? value, out IanaTimeZone? timeZone)
        {
            timeZone = null;
            return false;
        }

        public IReadOnlyCollection<IanaTimeZone> GetAll() => [];

        public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
        {
            timeZoneIds = [];

            if (string.IsNullOrWhiteSpace(isoCountryAlpha2Code))
                return false;

            var key = isoCountryAlpha2Code.Trim().ToUpperInvariant();
            if (zoneIdsByCountryAlpha2.TryGetValue(key, out var result))
            {
                timeZoneIds = result;
                return true;
            }

            return false;
        }
    }

    public static class GetTimeZones
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Skips invalid/whitespace zones and dedupes", " gb ", new FakeIanaTimeZoneProvider(new Dictionary<string, IReadOnlyCollection<string>>
            {
                // Includes: whitespace -> skip, invalid -> skip, duplicates -> dedupe by tz.Id
                ["GB"] = ["   ", "No/SuchZone", "UTC", " UTC ", "UTC"],
            }), ["UTC"]),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Unknown country returns empty", "ZZ", new FakeIanaTimeZoneProvider([]), []),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(
            string Name,
            string? IsoCountryAlpha2Code,
            FakeIanaTimeZoneProvider Provider,
            string[] ExpectedTimeZoneIds)
            : ReturnCase<(string? IsoCountryAlpha2Code, FakeIanaTimeZoneProvider Provider), string[]>(
                Name,
                (IsoCountryAlpha2Code, Provider),
                ExpectedTimeZoneIds);

        #endregion Cases
    }
}
