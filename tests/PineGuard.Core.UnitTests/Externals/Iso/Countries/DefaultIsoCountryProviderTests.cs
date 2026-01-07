using PineGuard.Externals.Iso.Countries;
using PineGuard.Iso.Countries;
using PineGuard.Testing.UnitTests;
using System.Collections.Frozen;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public sealed class DefaultIsoCountryProviderTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsAlpha2Code.ValidCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsAlpha2Code))]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsAlpha2Code.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsAlpha2Code))]
    public void ContainsAlpha2Code_ReturnsExpected(DefaultIsoCountryProviderTestData.ContainsAlpha2Code.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCountryProvider.Instance;

        // Act
        var result = provider.ContainsAlpha2Code(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(provider.GetAll().Count > 0);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsAlpha3Code.ValidCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsAlpha3Code))]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsAlpha3Code))]
    public void ContainsAlpha3Code_ReturnsExpected(DefaultIsoCountryProviderTestData.ContainsAlpha3Code.ValidCase testCase)
    {
        var provider = DefaultIsoCountryProvider.Instance;

        var result = provider.ContainsAlpha3Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsNumericCode.ValidCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsNumericCode))]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.ContainsNumericCode.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.ContainsNumericCode))]
    public void ContainsNumericCode_ReturnsExpected(DefaultIsoCountryProviderTestData.ContainsNumericCode.ValidCase testCase)
    {
        var provider = DefaultIsoCountryProvider.Instance;

        var result = provider.ContainsNumericCode(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGetByCodes.ValidCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGetByCodes))]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGetByCodes.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGetByCodes))]
    public void TryGetByCodes_WhenValid_ReturnsTrueAndCountry(DefaultIsoCountryProviderTestData.TryGetByCodes.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCountryProvider.Instance;

        // Act
        var ok2 = provider.TryGetByAlpha2Code(testCase.Alpha2, out var by2);
        var ok3 = provider.TryGetByAlpha3Code(testCase.Alpha3, out var by3);
        var okN = provider.TryGetByNumericCode(testCase.Numeric, out var byN);

        // Assert
        Assert.True(ok2);
        Assert.True(ok3);
        Assert.True(okN);

        Assert.NotNull(by2);
        Assert.NotNull(by3);
        Assert.NotNull(byN);

        Assert.Equal(by2.Alpha2Code, by3.Alpha2Code);
        Assert.Equal(by2.Alpha2Code, byN.Alpha2Code);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGetByAlpha2Code.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGetByAlpha2Code))]
    public void TryGetByAlpha2Code_ReturnsFalse_AndNull_ForEdgeCases(DefaultIsoCountryProviderTestData.TryGetByAlpha2Code.EdgeCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCountryProvider.Instance;

        // Act
        var ok = provider.TryGetByAlpha2Code(testCase.Value, out var country);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Null(country);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGetByAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGetByAlpha3Code))]
    public void TryGetByAlpha3Code_ReturnsFalse_AndNull_ForEdgeCases(DefaultIsoCountryProviderTestData.TryGetByAlpha3Code.EdgeCase testCase)
    {
        var provider = DefaultIsoCountryProvider.Instance;

        var ok = provider.TryGetByAlpha3Code(testCase.Value, out var country);

        Assert.Equal(testCase.Expected, ok);
        Assert.Null(country);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGetByNumericCode.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGetByNumericCode))]
    public void TryGetByNumericCode_ReturnsFalse_AndNull_ForEdgeCases(DefaultIsoCountryProviderTestData.TryGetByNumericCode.EdgeCase testCase)
    {
        var provider = DefaultIsoCountryProvider.Instance;

        var ok = provider.TryGetByNumericCode(testCase.Value, out var country);

        Assert.Equal(testCase.Expected, ok);
        Assert.Null(country);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGet.ValidCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGet))]
    [MemberData(nameof(DefaultIsoCountryProviderTestData.TryGet.EdgeCases), MemberType = typeof(DefaultIsoCountryProviderTestData.TryGet))]
    public void TryGet_ReturnsExpected(DefaultIsoCountryProviderTestData.TryGet.ValidCase testCase)
    {
        var provider = DefaultIsoCountryProvider.Instance;

        var ok = provider.TryGet(testCase.Value, out var country);

        Assert.Equal(testCase.Expected, ok);
        if (testCase.Expected)
        {
            Assert.NotNull(country);
            Assert.Equal(testCase.ExpectedAlpha2, country.Alpha2Code);
        }
        else
        {
            Assert.Null(country);
        }
    }

    [Fact]
    public void Constructor_UsesProvidedDictionaries_WhenNotNull()
    {
        var expected = new IsoCountry(alpha2Code: "XX", alpha3Code: "XXX", numericCode: "999", name: "Testland");

        var by2 = new Dictionary<string, IsoCountry> { ["XX"] = expected }.ToFrozenDictionary();
        var by3 = new Dictionary<string, IsoCountry> { ["XXX"] = expected }.ToFrozenDictionary();
        var byN = new Dictionary<string, IsoCountry> { ["999"] = expected }.ToFrozenDictionary();

        var provider = new DefaultIsoCountryProvider(by2, by3, byN);

        Assert.True(provider.ContainsAlpha2Code("XX"));
        Assert.True(provider.ContainsAlpha3Code("XXX"));
        Assert.True(provider.ContainsNumericCode("999"));
        Assert.Single(provider.GetAll());

        Assert.True(provider.TryGet("XX", out var found));
        Assert.NotNull(found);
        Assert.Equal("XX", found.Alpha2Code);
    }
}
