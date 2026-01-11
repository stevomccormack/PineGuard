using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class UriRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(UriRulesTestData.IsAbsoluteUri.ValidCases), MemberType = typeof(UriRulesTestData.IsAbsoluteUri))]
    public void IsAbsoluteUri_ReturnsExpected_ForValidCases(UriRulesTestData.IsAbsoluteUri.Case testCase)
    {
        // Act
        var result = UriRules.IsAbsoluteUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsAbsoluteUri.EdgeCases), MemberType = typeof(UriRulesTestData.IsAbsoluteUri))]
    public void IsAbsoluteUri_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsAbsoluteUri.Case testCase)
    {
        // Act
        var result = UriRules.IsAbsoluteUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsRelativeUri.ValidCases), MemberType = typeof(UriRulesTestData.IsRelativeUri))]
    public void IsRelativeUri_ReturnsExpected_ForValidCases(UriRulesTestData.IsRelativeUri.Case testCase)
    {
        // Act
        var result = UriRules.IsRelativeUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsRelativeUri.EdgeCases), MemberType = typeof(UriRulesTestData.IsRelativeUri))]
    public void IsRelativeUri_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsRelativeUri.Case testCase)
    {
        // Act
        var result = UriRules.IsRelativeUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsUrl.ValidCases), MemberType = typeof(UriRulesTestData.IsUrl))]
    public void IsUrl_ReturnsExpected_ForValidCases(UriRulesTestData.IsUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsUrl.EdgeCases), MemberType = typeof(UriRulesTestData.IsUrl))]
    public void IsUrl_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpsUrl.ValidCases), MemberType = typeof(UriRulesTestData.IsHttpsUrl))]
    public void IsHttpsUrl_ReturnsExpected_ForValidCases(UriRulesTestData.IsHttpsUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsHttpsUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpsUrl.EdgeCases), MemberType = typeof(UriRulesTestData.IsHttpsUrl))]
    public void IsHttpsUrl_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsHttpsUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsHttpsUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpUrl.ValidCases), MemberType = typeof(UriRulesTestData.IsHttpUrl))]
    public void IsHttpUrl_ReturnsExpected_ForValidCases(UriRulesTestData.IsHttpUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsHttpUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsHttpUrl.EdgeCases), MemberType = typeof(UriRulesTestData.IsHttpUrl))]
    public void IsHttpUrl_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsHttpUrl.Case testCase)
    {
        // Act
        var result = UriRules.IsHttpUrl(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFileUri.ValidCases), MemberType = typeof(UriRulesTestData.IsFileUri))]
    public void IsFileUri_ReturnsExpected_ForValidCases(UriRulesTestData.IsFileUri.Case testCase)
    {
        // Act
        var result = UriRules.IsFileUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFileUri.EdgeCases), MemberType = typeof(UriRulesTestData.IsFileUri))]
    public void IsFileUri_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsFileUri.Case testCase)
    {
        // Act
        var result = UriRules.IsFileUri(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFilePath.ValidCases), MemberType = typeof(UriRulesTestData.IsFilePath))]
    public void IsFilePath_ReturnsExpected_ForValidCases(UriRulesTestData.IsFilePath.Case testCase)
    {
        // Act
        var result = UriRules.IsFilePath(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.IsFilePath.EdgeCases), MemberType = typeof(UriRulesTestData.IsFilePath))]
    public void IsFilePath_ReturnsFalse_ForInvalidCases(UriRulesTestData.IsFilePath.Case testCase)
    {
        // Act
        var result = UriRules.IsFilePath(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.HasScheme.ValidCases), MemberType = typeof(UriRulesTestData.HasScheme))]
    public void HasScheme_IsCaseInsensitive_ForValidCases(UriRulesTestData.HasScheme.Case testCase)
    {
        // Act
        var result = UriRules.HasScheme(testCase.Value, testCase.Scheme);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(UriRulesTestData.HasScheme.EdgeCases), MemberType = typeof(UriRulesTestData.HasScheme))]
    public void HasScheme_ReturnsFalse_WhenSchemeDoesNotMatchOrParseFails(UriRulesTestData.HasScheme.Case testCase)
    {
        // Act
        var result = UriRules.HasScheme(testCase.Value, testCase.Scheme);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Fact]
    public void HasScheme_Throws_WhenSchemeIsNull()
    {
        // Act
        _ = Assert.Throws<ArgumentNullException>(() => UriRules.HasScheme("https://example.com", scheme: null!));

        // Assert
        Assert.True(true);
    }
}
