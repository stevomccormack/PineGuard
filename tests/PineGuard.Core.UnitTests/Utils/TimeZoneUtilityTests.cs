using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TimeZoneUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeZoneUtilityGetTimeZonesTestData.GetTimeZones.ValidCases), MemberType = typeof(TimeZoneUtilityGetTimeZonesTestData.GetTimeZones))]
    public void GetTimeZones_WithProvider_CoversSkipFailAndDedupeBranches(
        TimeZoneUtilityGetTimeZonesTestData.GetTimeZones.Case testCase)
    {
        var zones = TimeZoneUtility.GetTimeZones(testCase.IsoCountryAlpha2Code, testCase.Provider);

        Assert.Equal(testCase.ExpectedTimeZoneIds.Length, zones.Count);

        var actual = zones.Select(z => z.Id).OrderBy(x => x, StringComparer.Ordinal).ToArray();
        var expected = testCase.ExpectedTimeZoneIds.OrderBy(x => x, StringComparer.Ordinal).ToArray();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TimeZoneUtilityGetTimeZonesTestData.GetTimeZones.EdgeCases), MemberType = typeof(TimeZoneUtilityGetTimeZonesTestData.GetTimeZones))]
    public void GetTimeZones_WithProvider_ReturnsExpected_ForEdgeCases(
        TimeZoneUtilityGetTimeZonesTestData.GetTimeZones.Case testCase)
    {
        var zones = TimeZoneUtility.GetTimeZones(testCase.IsoCountryAlpha2Code, testCase.Provider);

        var actual = zones.Select(z => z.Id).OrderBy(x => x, StringComparer.Ordinal).ToArray();
        var expected = testCase.ExpectedTimeZoneIds.OrderBy(x => x, StringComparer.Ordinal).ToArray();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetTimeZones_ReturnsEmpty_ForNullOrUnknownCountry()
    {
        Assert.Empty(TimeZoneUtility.GetTimeZones(null));
        Assert.Empty(TimeZoneUtility.GetTimeZones("  "));
        Assert.Empty(TimeZoneUtility.GetTimeZones("ZZ"));
    }

    [Fact]
    public void GetTimeZoneIds_ReturnsEmpty_ForNullOrUnknownCountry()
    {
        Assert.Empty(TimeZoneUtility.GetTimeZoneIds(null));
        Assert.Empty(TimeZoneUtility.GetTimeZoneIds("ZZ"));
    }

    [Fact]
    public void GetTimeZonesFromIsoCountryAlpha3_ReturnsEmpty_ForNullOrUnknownCountry()
    {
        Assert.Empty(TimeZoneUtility.GetTimeZonesFromIsoCountryAlpha3(null));
        Assert.Empty(TimeZoneUtility.GetTimeZonesFromIsoCountryAlpha3("   "));
        Assert.Empty(TimeZoneUtility.GetTimeZonesFromIsoCountryAlpha3("ZZZ"));
    }

    [Fact]
    public void GetTimeZoneIdsFromIsoCountryAlpha3_ReturnsEmpty_ForNullOrUnknownCountry()
    {
        Assert.Empty(TimeZoneUtility.GetTimeZoneIdsFromIsoCountryAlpha3(null));
        Assert.Empty(TimeZoneUtility.GetTimeZoneIdsFromIsoCountryAlpha3("ZZZ"));
    }

    [Fact]
    public void GetTimeZoneIds_ReturnsSome_ForGB()
    {
        var alpha2 = TimeZoneUtility.GetTimeZoneIds("GB");
        var alpha3 = TimeZoneUtility.GetTimeZoneIdsFromIsoCountryAlpha3("GBR");

        Assert.NotEmpty(alpha2);
        Assert.NotEmpty(alpha3);
    }
}
