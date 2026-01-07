using PineGuard.Iso.Currencies;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public sealed class IsoCurrencyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Constructor.ValidCases), MemberType = typeof(IsoCurrencyTestData.Constructor))]
    public void Ctor_WhenValid_NormalizesAndSetsProperties(IsoCurrencyTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var currency = new IsoCurrency(testCase.Alpha3, testCase.Numeric, testCase.DecimalPlaces, testCase.CurrencyName);

        // Assert
        Assert.Equal(testCase.Alpha3.ToUpperInvariant(), currency.Alpha3Code);
        Assert.Equal(testCase.Numeric, currency.NumericCode);
        Assert.Equal(testCase.DecimalPlaces, currency.DecimalPlaces);
        Assert.Equal(testCase.CurrencyName, currency.Name);

        Assert.Matches(IsoCurrency.Alpha3CodeRegex(), currency.Alpha3Code);
        Assert.Matches(IsoCurrency.NumericCodeRegex(), currency.NumericCode);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Constructor.InvalidCases), MemberType = typeof(IsoCurrencyTestData.Constructor))]
    public void Ctor_WhenInvalid_Throws(IsoCurrencyTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => _ = new IsoCurrency(testCase.Alpha3, testCase.Numeric, testCase.DecimalPlaces, testCase.CurrencyName));

        // Assert
        Assert.NotNull(ex);
        Assert.IsType(testCase.ExpectedException.Type, ex);
        if (testCase.ExpectedException.ParamName is not null)
            Assert.Equal(testCase.ExpectedException.ParamName, ((ArgumentException)ex).ParamName);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.TryParse.ValidCases), MemberType = typeof(IsoCurrencyTestData.TryParse))]
    public void TryParse_ParsesByAlpha3OrNumeric(IsoCurrencyTestData.TryParse.ValidCase testCase)
    {
        // Arrange

        // Act
        var ok = IsoCurrency.TryParse(testCase.Input, out var currency);

        // Assert
        Assert.True(ok);
        Assert.NotNull(currency);
        Assert.Equal(testCase.ExpectedAlpha3, currency.Alpha3Code);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.TryParse.EdgeCases), MemberType = typeof(IsoCurrencyTestData.TryParse))]
    public void TryParse_WhenNullOrInvalid_ReturnsFalse(IsoCurrencyTestData.TryParse.EdgeCase testCase)
    {
        // Arrange

        // Act
        var ok = IsoCurrency.TryParse(testCase.Value, out var currency);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Null(currency);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Parse.InvalidCases), MemberType = typeof(IsoCurrencyTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsFormatException(IsoCurrencyTestData.Parse.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => _ = IsoCurrency.Parse(testCase.Value));

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
        var currency = IsoCurrency.Parse("USD");

        // Act
        var text = currency.ToString();

        // Assert
        Assert.Contains(IsoCurrency.IsoStandard, text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(currency.Name, text, StringComparison.Ordinal);
        Assert.Contains(currency.Alpha3Code, text, StringComparison.Ordinal);
    }

    [Fact]
    public void EqualityAndHashCode_AreStable_ForSameValues()
    {
        var left = IsoCurrency.Parse("USD");
        var right = left with { };

        Assert.NotSame(left, right);
        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }
}
