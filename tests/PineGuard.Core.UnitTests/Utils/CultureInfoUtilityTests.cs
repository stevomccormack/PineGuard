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
        // Act
        var result = CultureInfoUtility.TryGetCultureName(testCase.IsoLanguageAlpha2Code, out var cultureName);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload))]
    public void TryGetCultureInfo_WithoutRegionOverload_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureNameWithoutRegionOverload.ValidCase testCase)
    {
        // Act
        var result = CultureInfoUtility.TryGetCultureInfo(testCase.IsoLanguageAlpha2Code, out var cultureInfo);

        // Assert
        Assert.Equal(testCase.Expected, result);

        if (testCase.Expected)
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
        // Act
        var result = CultureInfoUtility.TryGetCultureName(testCase.IsoLanguageAlpha2Code, testCase.RegionCode, out var cultureName);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion))]
    public void TryGetCultureNameWithDefaultRegion_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureNameWithDefaultRegion.ValidCase testCase)
    {
        // Act
        var result = CultureInfoUtility.TryGetCultureNameWithDefaultRegion(testCase.IsoLanguageAlpha2Code, out var cultureName);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, cultureName);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureInfo.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureInfo))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetCultureInfo.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetCultureInfo))]
    public void TryGetCultureInfo_ReturnsExpected(CultureInfoUtilityTestData.TryGetCultureInfo.ValidCase testCase)
    {
        // Act
        var result = CultureInfoUtility.TryGetCultureInfo(testCase.IsoLanguageAlpha2Code, testCase.RegionCode, out var cultureInfo);

        // Assert
        Assert.Equal(testCase.Expected, result);

        if (testCase.Expected)
        {
            Assert.NotNull(cultureInfo);
            Assert.Equal(testCase.ExpectedCultureName, cultureInfo.Name);
        }
        else
        {
            Assert.Null(cultureInfo);
        }
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.GetRegionCodes.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.GetRegionCodes))]
    [MemberData(nameof(CultureInfoUtilityTestData.GetRegionCodes.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.GetRegionCodes))]
    public void GetRegionCodes_ReturnsExpected(CultureInfoUtilityTestData.GetRegionCodes.ValidCase testCase)
    {
        // Act
        var regions = CultureInfoUtility.GetRegionCodes(testCase.IsoLanguageAlpha2Code);

        // Assert
        if (testCase.ExpectedNonEmpty)
        {
            Assert.NotEmpty(regions);
        }
        else
        {
            Assert.Empty(regions);
        }
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.GetCultures.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.GetCultures))]
    [MemberData(nameof(CultureInfoUtilityTestData.GetCultures.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.GetCultures))]
    public void GetCultures_ReturnsExpected(CultureInfoUtilityTestData.GetCultures.ValidCase testCase)
    {
        // Act
        var cultures = CultureInfoUtility.GetCultures(testCase.IsoLanguageAlpha2Code);

        // Assert
        if (testCase.ExpectedNonEmpty)
        {
            Assert.NotEmpty(cultures);

            // Replaces "GetCultures_ReturnsSortedCultureNames_IgnoringCase" logic
            var names = cultures.Select(c => c.Name).ToArray();
            var sorted = names.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToArray();
            Assert.Equal(sorted, names);
        }
        else
        {
            Assert.Empty(cultures);
        }
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetTwoLetterIsoRegionName.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetTwoLetterIsoRegionName))]
    [MemberData(nameof(CultureInfoUtilityTestData.TryGetTwoLetterIsoRegionName.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.TryGetTwoLetterIsoRegionName))]
    public void TryGetTwoLetterIsoRegionName_ReturnsExpected(CultureInfoUtilityTestData.TryGetTwoLetterIsoRegionName.ValidCase testCase)
    {
        // Act
        var result = CultureInfoUtility.TryGetTwoLetterIsoRegionName(testCase.Culture, out var region);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedRegion, region);
    }

    [Theory]
    [MemberData(nameof(CultureInfoUtilityTestData.AddRegionCode.ValidCases), MemberType = typeof(CultureInfoUtilityTestData.AddRegionCode))]
    [MemberData(nameof(CultureInfoUtilityTestData.AddRegionCode.EdgeCases), MemberType = typeof(CultureInfoUtilityTestData.AddRegionCode))]
    public void AddRegionCode_CollectsOnlyResolvableRegions(CultureInfoUtilityTestData.AddRegionCode.ValidCase testCase)
    {
        // Arrange
        var regions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Act
        CultureInfoUtility.AddRegionCode(regions, testCase.Culture);

        // Assert
        if (testCase.Expected is null)
        {
            Assert.Empty(regions);
            return;
        }

        Assert.Equal([testCase.Expected], regions);
    }
}
