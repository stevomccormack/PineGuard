using PineGuard.Externals.Iso.Currencies;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public sealed class IsoCurrencyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Constructor.ValidCases), MemberType = typeof(IsoCurrencyTestData.Constructor))]
    public void Ctor_WhenValid_NormalizesAndSetsProperties(IsoCurrencyTestData.Constructor.ValidCase testCase)
    {
        // Act
        var currency = new IsoCurrency(
            testCase.Value.Alpha3,
            testCase.Value.Numeric,
            testCase.Value.DecimalPlaces,
            testCase.Value.CurrencyName);

        // Assert
        Assert.Equal(testCase.Value.Alpha3.ToUpperInvariant(), currency.Alpha3Code);
        Assert.Equal(testCase.Value.Numeric, currency.NumericCode);
        Assert.Equal(testCase.Value.DecimalPlaces, currency.DecimalPlaces);
        Assert.Equal(testCase.Value.CurrencyName, currency.Name);

        Assert.Matches(IsoCurrency.Alpha3CodeRegex(), currency.Alpha3Code);
        Assert.Matches(IsoCurrency.NumericCodeRegex(), currency.NumericCode);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Constructor.InvalidCases), MemberType = typeof(IsoCurrencyTestData.Constructor))]
    public void Ctor_WhenInvalid_Throws(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoCurrencyTestData.Constructor.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(
            invalidCase.ExpectedException.Type,
            () => _ = new IsoCurrency(
                invalidCase.Value.Alpha3,
                invalidCase.Value.Numeric,
                invalidCase.Value.DecimalPlaces,
                invalidCase.Value.CurrencyName));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.TryParse.ValidCases), MemberType = typeof(IsoCurrencyTestData.TryParse))]
    public void TryParse_ParsesByAlpha3OrNumeric(IsoCurrencyTestData.TryParse.ValidCase testCase)
    {
        // Act
        var ok = IsoCurrency.TryParse(testCase.Value, out var currency);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.NotNull(currency);
        Assert.Equal(testCase.ExpectedOutValue, currency.Alpha3Code);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.TryParse.EdgeCases), MemberType = typeof(IsoCurrencyTestData.TryParse))]
    public void TryParse_WhenNullOrInvalid_ReturnsFalse(IsoCurrencyTestData.TryParse.EdgeCase testCase)
    {
        // Act
        var ok = IsoCurrency.TryParse(testCase.Value, out var currency);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Null(currency);
        Assert.Null(testCase.ExpectedOutValue);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyTestData.Parse.InvalidCases), MemberType = typeof(IsoCurrencyTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsFormatException(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoCurrencyTestData.Parse.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = IsoCurrency.Parse(invalidCase.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
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
