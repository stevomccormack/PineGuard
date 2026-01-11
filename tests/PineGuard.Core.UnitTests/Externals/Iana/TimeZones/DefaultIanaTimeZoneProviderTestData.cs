using PineGuard.Externals.Iana.TimeZones;
using System.Collections.Frozen;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public static class DefaultIanaTimeZoneProviderTestData
{
    public static IanaTimeZone SampleZoneEuropeLondon { get; } = new("Europe/London", ["GB"], "+5130-00007", "United Kingdom");

    public static IanaTimeZone SampleZoneAmericaNewYork { get; } = new("America/New_York", ["US"], "+404251-0740023", null);

    public static FrozenDictionary<string, IanaTimeZone> SampleZonesById { get; } =
        new Dictionary<string, IanaTimeZone>
        {
            { SampleZoneEuropeLondon.Id, SampleZoneEuropeLondon },
            { SampleZoneAmericaNewYork.Id, SampleZoneAmericaNewYork },
        }.ToFrozenDictionary(StringComparer.Ordinal);

    public static FrozenDictionary<string, FrozenSet<string>> SampleZoneIdsByCountryAlpha2 { get; } =
        new Dictionary<string, FrozenSet<string>>(StringComparer.Ordinal)
        {
            { "GB", new HashSet<string>(StringComparer.Ordinal) { SampleZoneEuropeLondon.Id }.ToFrozenSet() },
            { "US", new HashSet<string>(StringComparer.Ordinal) { SampleZoneAmericaNewYork.Id }.ToFrozenSet() },
        }.ToFrozenDictionary(StringComparer.Ordinal);

    public static DefaultIanaTimeZoneProvider CreateSampleProvider() => new(
        zonesById: SampleZonesById,
        zoneIdsByCountryAlpha2: SampleZoneIdsByCountryAlpha2);

    public static class IsValidTimeZoneId
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("known", "Europe/London", true),
            new("known trims", " Europe/London ", true),
            new("known other", "America/New_York", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false),
            new("empty", "", false),
            new("space", " ", false),
            new("tabs/newlines", "\t\r\n", false),
            new("unknown", "Europe/Paris", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class TryGetById
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("known", "Europe/London", true, "Europe/London"),
            new("known trims", " Europe/London ", true, "Europe/London"),
            new("known other", "America/New_York", true, "America/New_York"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", "", false, null),
            new("space", " ", false, null),
            new("tabs/newlines", "\t\r\n", false, null),
            new("unknown", "Europe/Paris", false, null),
            new("tabs trim", "\tEurope/London\t", true, "Europe/London"),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class TryGetTimeZoneIdsByCountryAlpha2Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("GB", "GB", true, ["Europe/London"]),
            new("gb", "gb", true, ["Europe/London"]),
            new("us trims", "  us ", true, ["America/New_York"]),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("GB space", "GB ", true, ["Europe/London"]),
            new("null", null, false, []),
            new("empty", "", false, []),
            new("space", " ", false, []),
            new("tabs/newlines", "\t\r\n", false, []),
            new("unknown", "FR", false, []),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, IReadOnlyCollection<string> ExpectedOutValue)
            : TryCase<string?, IReadOnlyCollection<string>>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }
}
