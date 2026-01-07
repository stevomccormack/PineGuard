using PineGuard.Iso.Languages;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public sealed class IsoLanguageTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoLanguageTestData.Constructor.ValidCases), MemberType = typeof(IsoLanguageTestData.Constructor))]
    public void Ctor_WhenValid_NormalizesAndSetsProperties(IsoLanguageTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var lang = new IsoLanguage(testCase.Alpha2Code, testCase.Alpha3Code, testCase.NameValue);

        // Assert
        Assert.Equal(testCase.ExpectedAlpha2, lang.Alpha2Code);
        Assert.Equal(testCase.ExpectedAlpha3, lang.Alpha3Code);
        Assert.Equal(testCase.NameValue, lang.Name);

        Assert.Matches(IsoLanguage.Alpha2CodeRegex(), lang.Alpha2Code);
        Assert.Matches(IsoLanguage.Alpha3CodeRegex(), lang.Alpha3Code);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageTestData.Constructor.InvalidCases), MemberType = typeof(IsoLanguageTestData.Constructor))]
    public void Ctor_WhenInvalid_ThrowsExpected(IsoLanguageTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = new IsoLanguage(testCase.Alpha2Code, testCase.Alpha3Code, testCase.NameValue));

        // Assert
        Assert.Equal(testCase.ExpectedException.ParamName, ((ArgumentException)ex).ParamName);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageTestData.TryParse.ValidCases), MemberType = typeof(IsoLanguageTestData.TryParse))]
    [MemberData(nameof(IsoLanguageTestData.TryParse.EdgeCases), MemberType = typeof(IsoLanguageTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoLanguageTestData.TryParse.Case testCase)
    {
        // Arrange

        // Act
        var ok = IsoLanguage.TryParse(testCase.Value, out var lang);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        if (testCase.Expected)
        {
            Assert.NotNull(lang);
            Assert.Equal(testCase.ExpectedAlpha2, lang.Alpha2Code);
        }
        else
        {
            Assert.Null(lang);
        }
    }

    [Theory]
    [MemberData(nameof(IsoLanguageTestData.Parse.ValidCases), MemberType = typeof(IsoLanguageTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpected(IsoLanguageTestData.Parse.ValidCase testCase)
    {
        // Arrange

        // Act
        var lang = IsoLanguage.Parse(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedAlpha2, lang.Alpha2Code);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageTestData.Parse.InvalidCases), MemberType = typeof(IsoLanguageTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IsoLanguageTestData.Parse.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = IsoLanguage.Parse(testCase.Value!));

        // Assert
        if (testCase.ExpectedException.MessageContains is not null)
            Assert.Contains(testCase.ExpectedException.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ToString_IncludesStandardNameAndAlpha3Code()
    {
        // Arrange
        var lang = IsoLanguage.Parse("en");

        // Act
        var text = lang.ToString();

        // Assert
        Assert.Contains(IsoLanguage.IsoStandard, text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(lang.Name, text, StringComparison.Ordinal);
        Assert.Contains(lang.Alpha3Code, text, StringComparison.Ordinal);
    }

    [Fact]
    public void EqualityAndHashCode_AreStable_ForSameValues()
    {
        // Arrange
        var left = new IsoLanguage("en", "eng", "English");
        var right = left with { };

        // Act
        var equals = left.Equals(right);
        var hashLeft = left.GetHashCode();
        var hashRight = right.GetHashCode();

        // Assert
        Assert.NotSame(left, right);
        Assert.True(equals);
        Assert.Equal(hashLeft, hashRight);
    }
}
