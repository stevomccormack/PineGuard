using PineGuard.Externals.Iso.Currencies;
using PineGuard.Testing.UnitTests;
using System.Collections.Frozen;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public sealed class DefaultIsoCurrencyProviderTests : BaseUnitTest
{
    [Fact]
    public void Constructor_UsesProvidedDictionaries_WhenNotNull()
    {
        var testCurrency = new IsoCurrency("AAA", "000", 2, "Test Currency");

        var alpha3 =
            new Dictionary<string, IsoCurrency>(StringComparer.Ordinal)
            {
                { "AAA", testCurrency },
            }.ToFrozenDictionary(StringComparer.Ordinal);

        var numeric =
            new Dictionary<string, IsoCurrency>(StringComparer.Ordinal)
            {
                { "000", testCurrency },
            }.ToFrozenDictionary(StringComparer.Ordinal);

        var provider = new DefaultIsoCurrencyProvider(alpha3, numeric);

        Assert.True(provider.TryGetByAlpha3Code("AAA", out var byAlpha3));
        Assert.Same(testCurrency, byAlpha3);

        Assert.True(provider.TryGetByNumericCode("000", out var byNumeric));
        Assert.Same(testCurrency, byNumeric);

        Assert.Single(provider.GetAll());
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGet.ValidCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGet))]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGet.EdgeCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGet))]
    public void TryGet_ReturnsExpected(DefaultIsoCurrencyProviderTestData.TryGet.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCurrencyProvider.Instance;

        // Act
        var ok = provider.TryGet(testCase.Value, out var currency);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(currency);
            Assert.True(provider.GetAll().Count > 0);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, currency.Alpha3Code);
        }
        else
        {
            Assert.Null(currency);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.ContainsAlpha3Code.ValidCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.ContainsAlpha3Code))]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.ContainsAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.ContainsAlpha3Code))]
    public void ContainsAlpha3Code_ReturnsExpected(DefaultIsoCurrencyProviderTestData.ContainsAlpha3Code.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCurrencyProvider.Instance;

        // Act
        var contains = provider.ContainsAlpha3Code(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, contains);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.ContainsNumericCode.ValidCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.ContainsNumericCode))]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.ContainsNumericCode.EdgeCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.ContainsNumericCode))]
    public void ContainsNumericCode_ReturnsExpected(DefaultIsoCurrencyProviderTestData.ContainsNumericCode.ValidCase testCase)
    {
        var provider = DefaultIsoCurrencyProvider.Instance;

        var contains = provider.ContainsNumericCode(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, contains);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGetByAlpha3Code.ValidCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGetByAlpha3Code))]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGetByAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGetByAlpha3Code))]
    public void TryGetByAlpha3Code_ReturnsExpected(DefaultIsoCurrencyProviderTestData.TryGetByAlpha3Code.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIsoCurrencyProvider.Instance;

        // Act
        var ok = provider.TryGetByAlpha3Code(testCase.Value, out var currency);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(currency);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, currency.Alpha3Code);
        }
        else
        {
            Assert.Null(currency);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }

    [Theory]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGetByNumericCode.ValidCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGetByNumericCode))]
    [MemberData(nameof(DefaultIsoCurrencyProviderTestData.TryGetByNumericCode.EdgeCases), MemberType = typeof(DefaultIsoCurrencyProviderTestData.TryGetByNumericCode))]
    public void TryGetByNumericCode_ReturnsExpected(DefaultIsoCurrencyProviderTestData.TryGetByNumericCode.ValidCase testCase)
    {
        var provider = DefaultIsoCurrencyProvider.Instance;

        var ok = provider.TryGetByNumericCode(testCase.Value, out var currency);

        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(currency);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, currency.NumericCode);
        }
        else
        {
            Assert.Null(currency);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }
}
