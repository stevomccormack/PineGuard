using PineGuard.Externals.Iso.Languages;
using PineGuard.Testing.UnitTests;
using System.Collections.Frozen;

namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public sealed class DefaultIsoLanguageProviderTests : BaseUnitTest
{
    [Fact]
    public void Constructor_UsesProvidedDictionaries_WhenNotNull()
    {
        var testLanguage = new IsoLanguage("aa", "aaa", "Test Language");

        var alpha2 =
            new Dictionary<string, IsoLanguage>(StringComparer.Ordinal)
            {
                { "aa", testLanguage },
            }.ToFrozenDictionary(StringComparer.Ordinal);

        var alpha3 =
            new Dictionary<string, IsoLanguage>(StringComparer.Ordinal)
            {
                { "aaa", testLanguage },
            }.ToFrozenDictionary(StringComparer.Ordinal);

        var provider = new DefaultIsoLanguageProvider(alpha2, alpha3);

        Assert.True(provider.TryGetByAlpha2Code("aa", out var byAlpha2));
        Assert.Same(testLanguage, byAlpha2);

        Assert.True(provider.TryGetByAlpha3Code("aaa", out var byAlpha3));
        Assert.Same(testLanguage, byAlpha3);

        Assert.Single(provider.GetAll());
    }

    [Theory]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGet.ValidCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGet))]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGet.EdgeCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGet))]
    public void TryGet_ReturnsExpected(DefaultIsoLanguageProviderTestData.TryGet.Case testCase)
    {
        // Arrange
        var provider = DefaultIsoLanguageProvider.Instance;

        // Act
        var ok = provider.TryGet(testCase.Value, out var lang);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(lang);
            Assert.True(provider.GetAll().Count > 0);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, lang.Alpha2Code);
        }
        else
        {
            Assert.Null(lang);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }

    [Theory]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.ContainsAlpha2Code.ValidCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.ContainsAlpha2Code))]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.ContainsAlpha2Code.EdgeCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.ContainsAlpha2Code))]
    public void ContainsAlpha2Code_ReturnsExpected(DefaultIsoLanguageProviderTestData.ContainsAlpha2Code.Case testCase)
    {
        // Arrange
        var provider = DefaultIsoLanguageProvider.Instance;

        // Act
        var contains = provider.ContainsAlpha2Code(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, contains);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.ContainsAlpha3Code.ValidCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.ContainsAlpha3Code))]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.ContainsAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.ContainsAlpha3Code))]
    public void ContainsAlpha3Code_ReturnsExpected(DefaultIsoLanguageProviderTestData.ContainsAlpha3Code.Case testCase)
    {
        var provider = DefaultIsoLanguageProvider.Instance;

        var contains = provider.ContainsAlpha3Code(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, contains);
    }

    [Theory]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGetByAlpha2Code.ValidCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGetByAlpha2Code))]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGetByAlpha2Code.EdgeCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGetByAlpha2Code))]
    public void TryGetByAlpha2Code_ReturnsExpected(DefaultIsoLanguageProviderTestData.TryGetByAlpha2Code.Case testCase)
    {
        var provider = DefaultIsoLanguageProvider.Instance;

        var ok = provider.TryGetByAlpha2Code(testCase.Value, out var lang);

        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(lang);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, lang.Alpha2Code);
        }
        else
        {
            Assert.Null(lang);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }

    [Theory]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGetByAlpha3Code.ValidCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGetByAlpha3Code))]
    [MemberData(nameof(DefaultIsoLanguageProviderTestData.TryGetByAlpha3Code.EdgeCases), MemberType = typeof(DefaultIsoLanguageProviderTestData.TryGetByAlpha3Code))]
    public void TryGetByAlpha3Code_ReturnsExpected(DefaultIsoLanguageProviderTestData.TryGetByAlpha3Code.Case testCase)
    {
        var provider = DefaultIsoLanguageProvider.Instance;

        var ok = provider.TryGetByAlpha3Code(testCase.Value, out var lang);

        Assert.Equal(testCase.ExpectedReturn, ok);
        if (testCase.ExpectedReturn)
        {
            Assert.NotNull(lang);
            Assert.NotNull(testCase.ExpectedOutValue);
            Assert.Equal(testCase.ExpectedOutValue, lang.Alpha3Code);
        }
        else
        {
            Assert.Null(lang);
            Assert.Null(testCase.ExpectedOutValue);
        }
    }

    [Fact]
    public void TryGetByAlpha3Code_WhenValid_ReturnsLanguage()
    {
        // Arrange
        var provider = DefaultIsoLanguageProvider.Instance;

        // Act
        var ok = provider.TryGetByAlpha3Code("eng", out var lang);

        // Assert
        Assert.True(ok);
        Assert.NotNull(lang);
        Assert.Equal("eng", lang.Alpha3Code);
    }
}
