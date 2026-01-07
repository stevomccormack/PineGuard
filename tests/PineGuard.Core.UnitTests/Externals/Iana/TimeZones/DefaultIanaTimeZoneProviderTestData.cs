using PineGuard.Externals.Iana.TimeZones;
using System.Collections.Frozen;
using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public static class DefaultIanaTimeZoneProviderTestData
{
    public static IanaTimeZone SampleZoneEuropeLondon { get; } = new(
        "Europe/London",
        new[] { "GB" },
        "+5130-00007",
        "United Kingdom");

    public static IanaTimeZone SampleZoneAmericaNewYork { get; } = new(
        "America/New_York",
        new[] { "US" },
        "+404251-0740023",
        null);

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
        private static ValidCase V(string name, string? value, bool expected) => new(Name: name, Value: value, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("known", "Europe/London", expected: true) },
            { V("known trims", " Europe/London ", expected: true) },
            { V("known other", "America/New_York", expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false) },
            { V("empty", "", expected: false) },
            { V("space", " ", expected: false) },
            { V("tabs/newlines", "\t\r\n", expected: false) },
            { V("unknown", "Europe/Paris", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Value);

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : Case(Name, Value);

        public record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : Case(Name, Value);

        #endregion
    }

    public static class TryGetById
    {
        private static ValidCase V(string name, string? value, bool expected, string? expectedId) => new(Name: name, Value: value, Expected: expected, ExpectedId: expectedId);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("known", "Europe/London", expected: true, expectedId: "Europe/London") },
            { V("known trims", " Europe/London ", expected: true, expectedId: "Europe/London") },
            { V("known other", "America/New_York", expected: true, expectedId: "America/New_York") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false, expectedId: null) },
            { V("empty", "", expected: false, expectedId: null) },
            { V("space", " ", expected: false, expectedId: null) },
            { V("tabs/newlines", "\t\r\n", expected: false, expectedId: null) },
            { V("unknown", "Europe/Paris", expected: false, expectedId: null) },
            { V("tabs trim", "\tEurope/London\t", expected: true, expectedId: "Europe/London") },
        };

        #region Cases

        public record Case(string Name, string? Value);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedId)
            : Case(Name, Value);

        public record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : Case(Name, Value);

        #endregion
    }

    public static class TryGetTimeZoneIdsByCountryAlpha2Code
    {
        private static ValidCase V(string name, string? value, bool expected, string[] expectedIds) => new(Name: name, Value: value, Expected: expected, ExpectedIds: expectedIds);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("GB", "GB", expected: true, expectedIds: ["Europe/London"]) },
            { V("gb", "gb", expected: true, expectedIds: ["Europe/London"]) },
            { V("us trims", "  us ", expected: true, expectedIds: ["America/New_York"]) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("GB space", "GB ", expected: true, expectedIds: ["Europe/London"]) },
            { V("null", null, expected: false, expectedIds: Array.Empty<string>()) },
            { V("empty", "", expected: false, expectedIds: Array.Empty<string>()) },
            { V("space", " ", expected: false, expectedIds: Array.Empty<string>()) },
            { V("tabs/newlines", "\t\r\n", expected: false, expectedIds: Array.Empty<string>()) },
            { V("unknown", "FR", expected: false, expectedIds: Array.Empty<string>()) },
        };

        #region Cases

        public record Case(string Name, string? Value);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string[] ExpectedIds)
            : Case(Name, Value);

        public record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : Case(Name, Value);

        #endregion
    }
}
