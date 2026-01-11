using System.Globalization;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CultureInfoUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    public void TryGetCultureName_WithoutRegionOverload_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCase testCase)
    {
        var result = CultureInfoUtility.TryGetCultureName(testCase.IsoLanguageAlpha2Code, out var cultureName);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    public void TryGetCultureInfo_WithoutRegionOverload_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCase testCase)
    {
        var result = CultureInfoUtility.TryGetCultureInfo(testCase.IsoLanguageAlpha2Code, out var cultureInfo);

        Assert.Equal(testCase.ExpectedReturn, result);

        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(cultureInfo);
            Assert.Equal(testCase.ExpectedOutValue, cultureInfo.Name);
        }
        else
        {
            Assert.Null(cultureInfo);
        }
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureName.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureName))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureName.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureName))]
    public void TryGetCultureName_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureName.ValidCase testCase)
    {
        var result = CultureInfoUtility.TryGetCultureName(testCase.IsoLanguageAlpha2Code, testCase.RegionCode, out var cultureName);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion))]
    public void TryGetCultureNameWithDefaultRegion_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.ValidCase testCase)
    {
        var result = CultureInfoUtility.TryGetCultureNameWithDefaultRegion(testCase.IsoLanguageAlpha2Code, out var cultureName);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Fact]
    public void TryGetCultureInfo_ReturnsCultureInfo_ForValidCulture()
    {
        var result = CultureInfoUtility.TryGetCultureInfo("en", "US", out var cultureInfo);

        Assert.True(result);
        Assert.NotNull(cultureInfo);
        Assert.Equal("en-US", cultureInfo.Name);
    }

    [Fact]
    public void TryGetCultureInfo_ReturnsFalse_ForInvalidCulture()
    {
        var result = CultureInfoUtility.TryGetCultureInfo("no-such-culture", regionCode: null, out var cultureInfo);

        Assert.False(result);
        Assert.Null(cultureInfo);
    }

    [Fact]
    public void GetRegionCodes_ReturnsNonEmpty_ForCommonLanguage()
    {
        var regions = CultureInfoUtility.GetRegionCodes("en");

        Assert.NotEmpty(regions);
    }

    [Fact]
    public void GetRegionCodes_ReturnsEmpty_ForInvalidLanguage()
    {
        var regions = CultureInfoUtility.GetRegionCodes("zz");

        Assert.Empty(regions);
    }

    [Fact]
    public void GetRegionCodes_ReturnsEmpty_ForNullOrWhitespace()
    {
        Assert.Empty(CultureInfoUtility.GetRegionCodes(null));
        Assert.Empty(CultureInfoUtility.GetRegionCodes(""));
        Assert.Empty(CultureInfoUtility.GetRegionCodes("  "));
    }

    [Fact]
    public void GetCultures_ReturnsEmpty_ForInvalidLanguage()
    {
        var cultures = CultureInfoUtility.GetCultures("zz");

        Assert.Empty(cultures);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.GetCultures.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.GetCultures))]
    public void GetCultures_ReturnsEmpty_ForNullOrWhitespace(CultureInfoUtilityTestData.GetCultures.ValidCase testCase)
    {
        var cultures = CultureInfoUtility.GetCultures(testCase.IsoLanguageAlpha2Code);
        Assert.Empty(cultures);
    }

    [Fact]
    public void TryGetTwoLetterIsoRegionName_ReturnsExpected()
    {
        Assert.True(CultureInfoUtility.TryGetTwoLetterIsoRegionName(CultureInfo.GetCultureInfo("en-US"), out var region));
        Assert.Equal("US", region);

        Assert.False(CultureInfoUtility.TryGetTwoLetterIsoRegionName(CultureInfo.InvariantCulture, out var invalid));
        Assert.Equal(string.Empty, invalid);
    }

    [Fact]
    public void GetCultures_ReturnsSortedCultureNames_IgnoringCase()
    {
        var cultures = CultureInfoUtility.GetCultures("en");

        Assert.NotEmpty(cultures);

        var names = cultures.Select(c => c.Name).ToArray();
        var sorted = names.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToArray();

        Assert.Equal(sorted, names);
    }
}
