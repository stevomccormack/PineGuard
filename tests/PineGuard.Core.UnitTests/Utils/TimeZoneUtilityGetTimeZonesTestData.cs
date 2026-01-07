using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class TimeZoneUtilityGetTimeZonesTestData
{
    public sealed class FakeIanaTimeZoneProvider : IIanaTimeZoneProvider
    {
        private readonly Dictionary<string, IReadOnlyCollection<string>> _zoneIdsByCountryAlpha2;

        public FakeIanaTimeZoneProvider(Dictionary<string, IReadOnlyCollection<string>> zoneIdsByCountryAlpha2)
        {
            _zoneIdsByCountryAlpha2 = zoneIdsByCountryAlpha2;
        }

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
            if (_zoneIdsByCountryAlpha2.TryGetValue(key, out var result) && result is not null)
            {
                timeZoneIds = result;
                return true;
            }

            return false;
        }
    }

    public static class GetTimeZones
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V(
                "Skips invalid/whitespace zones and dedupes",
                " gb ",
                new FakeIanaTimeZoneProvider(new Dictionary<string, IReadOnlyCollection<string>>
                {
                    // Includes: whitespace -> skip, invalid -> skip, duplicates -> dedupe by tz.Id
                    ["GB"] = new[] { "   ", "No/SuchZone", "UTC", " UTC ", "UTC" },
                }),
                new[] { "UTC" }),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V(
                "Unknown country returns empty",
                "ZZ",
                new FakeIanaTimeZoneProvider(new Dictionary<string, IReadOnlyCollection<string>>()),
                Array.Empty<string>()),
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        private static Case V(
            string name,
            string? isoCountryAlpha2Code,
            FakeIanaTimeZoneProvider provider,
            string[] expectedTimeZoneIds) => new(name, isoCountryAlpha2Code, provider, expectedTimeZoneIds);

        #region Cases

        public sealed record Case(
            string Name,
            string? IsoCountryAlpha2Code,
            FakeIanaTimeZoneProvider Provider,
            string[] ExpectedTimeZoneIds) : BaseCase(Name);

        #endregion Cases
    }
}
