using PineGuard.Iso.Countries;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public sealed class IsoCountryTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCountryTestData.Constructor.ValidCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    [MemberData(nameof(IsoCountryTestData.Constructor.EdgeCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    public void Ctor_WhenValid_NormalizesAndSetsProperties(IsoCountryTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var country = new IsoCountry(testCase.Alpha2, testCase.Alpha3, testCase.Numeric, testCase.CountryName);

        // Assert
        Assert.Equal(testCase.Alpha2.ToUpperInvariant(), country.Alpha2Code);
        Assert.Equal(testCase.Alpha3.ToUpperInvariant(), country.Alpha3Code);
        Assert.Equal(testCase.Numeric, country.NumericCode);
        Assert.Equal(testCase.CountryName, country.Name);

        Assert.Matches(IsoCountry.Alpha2CodeRegex(), country.Alpha2Code);
        Assert.Matches(IsoCountry.Alpha3CodeRegex(), country.Alpha3Code);
        Assert.Matches(IsoCountry.NumericCodeRegex(), country.NumericCode);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.Constructor.InvalidCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    public void Ctor_WhenInvalid_ThrowsArgumentException(IsoCountryTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = new IsoCountry(testCase.Alpha2, testCase.Alpha3, testCase.Numeric, testCase.CountryName));

        // Assert
        if (testCase.ExpectedException.ParamName is not null)
            Assert.Equal(testCase.ExpectedException.ParamName, ((ArgumentException)ex).ParamName);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.TryParse.ValidCases), MemberType = typeof(IsoCountryTestData.TryParse))]
    public void TryParse_ParsesByAnySupportedCode(IsoCountryTestData.TryParse.ValidCase testCase)
    {
        // Arrange

        // Act
        var okAlpha2 = IsoCountry.TryParse(testCase.Alpha2, out var byAlpha2);
        var okAlpha3 = IsoCountry.TryParse(testCase.Alpha3, out var byAlpha3);
        var okNumeric = IsoCountry.TryParse(testCase.Numeric, out var byNumeric);

        // Assert
        Assert.True(okAlpha2);
        Assert.True(okAlpha3);
        Assert.True(okNumeric);

        Assert.NotNull(byAlpha2);
        Assert.NotNull(byAlpha3);
        Assert.NotNull(byNumeric);

        Assert.Equal(byAlpha2.Alpha2Code, byAlpha3.Alpha2Code);
        Assert.Equal(byAlpha2.Alpha2Code, byNumeric.Alpha2Code);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.TryParse.EdgeCases), MemberType = typeof(IsoCountryTestData.TryParse))]
    public void TryParse_WhenNullOrInvalid_ReturnsFalse(IsoCountryTestData.TryParse.EdgeCase testCase)
    {
        // Arrange

        // Act
        var ok = IsoCountry.TryParse(testCase.Value, out var country);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Null(country);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.Parse.InvalidCases), MemberType = typeof(IsoCountryTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsFormatException(IsoCountryTestData.Parse.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => _ = IsoCountry.Parse(testCase.Value));

        // Assert
        Assert.NotNull(ex);
        Assert.IsType(testCase.ExpectedException.Type, ex);
        if (testCase.ExpectedException.MessageContains is not null)
            Assert.Contains(testCase.ExpectedException.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ToString_IncludesStandardNameAndAlpha3Code()
    {
        // Arrange
        var country = IsoCountry.Parse("US");

        // Act
        var text = country.ToString();

        // Assert
        Assert.Contains(IsoCountry.IsoStandard, text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(country.Name, text, StringComparison.Ordinal);
        Assert.Contains(country.Alpha3Code, text, StringComparison.Ordinal);
    }

    [Fact]
    public void EqualityAndHashCode_AreStable_ForSameValues()
    {
        var left = IsoCountry.Parse("US");
        var right = left with { };

        Assert.NotSame(left, right);
        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
