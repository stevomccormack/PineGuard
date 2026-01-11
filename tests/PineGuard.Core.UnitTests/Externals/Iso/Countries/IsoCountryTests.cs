using PineGuard.Externals.Iso.Countries;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public sealed class IsoCountryTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCountryTestData.Constructor.ValidCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    [MemberData(nameof(IsoCountryTestData.Constructor.EdgeCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    public void Ctor_WhenValid_NormalizesAndSetsProperties(IsoCountryTestData.Constructor.ValidCase testCase)
    {
        // Act
        var country = new IsoCountry(
            testCase.Value.Alpha2,
            testCase.Value.Alpha3,
            testCase.Value.Numeric,
            testCase.Value.CountryName);

        // Assert
        Assert.Equal(testCase.Value.Alpha2.ToUpperInvariant(), country.Alpha2Code);
        Assert.Equal(testCase.Value.Alpha3.ToUpperInvariant(), country.Alpha3Code);
        Assert.Equal(testCase.Value.Numeric, country.NumericCode);
        Assert.Equal(testCase.Value.CountryName, country.Name);

        Assert.Matches(IsoCountry.Alpha2CodeRegex(), country.Alpha2Code);
        Assert.Matches(IsoCountry.Alpha3CodeRegex(), country.Alpha3Code);
        Assert.Matches(IsoCountry.NumericCodeRegex(), country.NumericCode);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.Constructor.InvalidCases), MemberType = typeof(IsoCountryTestData.Constructor))]
    public void Ctor_WhenInvalid_ThrowsArgumentException(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoCountryTestData.Constructor.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(
            invalidCase.ExpectedException.Type,
            () => _ = new IsoCountry(
                invalidCase.Value.Alpha2,
                invalidCase.Value.Alpha3,
                invalidCase.Value.Numeric,
                invalidCase.Value.CountryName));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.TryParse.ValidCases), MemberType = typeof(IsoCountryTestData.TryParse))]
    public void TryParse_ParsesByAnySupportedCode(IsoCountryTestData.TryParse.ValidCase testCase)
    {
        // Act
        var okAlpha2 = IsoCountry.TryParse(testCase.Value.Alpha2, out var byAlpha2);
        var okAlpha3 = IsoCountry.TryParse(testCase.Value.Alpha3, out var byAlpha3);
        var okNumeric = IsoCountry.TryParse(testCase.Value.Numeric, out var byNumeric);

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
        // Act
        var ok = IsoCountry.TryParse(testCase.Value, out var country);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Null(country);
    }

    [Theory]
    [MemberData(nameof(IsoCountryTestData.Parse.InvalidCases), MemberType = typeof(IsoCountryTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsFormatException(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoCountryTestData.Parse.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = IsoCountry.Parse(invalidCase.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
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
