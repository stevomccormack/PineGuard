using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class GuidRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsEmpty.ValidCases), MemberType = typeof(GuidRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsTrue_WhenGuidIsEmpty(GuidRulesTestData.IsEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsEmpty.EdgeCases), MemberType = typeof(GuidRulesTestData.IsEmpty))]
    public void IsEmpty_ReturnsFalse_WhenNullOrNonEmpty(GuidRulesTestData.IsEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsNullOrEmpty.ValidCases), MemberType = typeof(GuidRulesTestData.IsNullOrEmpty))]
    public void IsNullOrEmpty_ReturnsTrue_WhenNullOrEmpty(GuidRulesTestData.IsNullOrEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsNullOrEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsNullOrEmpty.EdgeCases), MemberType = typeof(GuidRulesTestData.IsNullOrEmpty))]
    public void IsNullOrEmpty_ReturnsFalse_WhenNonEmpty(GuidRulesTestData.IsNullOrEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsNullOrEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsGuid.ValidCases), MemberType = typeof(GuidRulesTestData.IsGuid))]
    public void IsGuid_ReturnsExpected(GuidRulesTestData.IsGuid.Case testCase)
    {
        // Act
        var result = GuidRules.IsGuid(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsGuid.EdgeCases), MemberType = typeof(GuidRulesTestData.IsGuid))]
    public void IsGuid_ReturnsFalse_ForInvalidInputs(GuidRulesTestData.IsGuid.Case testCase)
    {
        // Act
        var result = GuidRules.IsGuid(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsGuidEmpty.ValidCases), MemberType = typeof(GuidRulesTestData.IsGuidEmpty))]
    public void IsGuidEmpty_ReturnsTrue_WhenParsedGuidIsEmpty(GuidRulesTestData.IsGuidEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsGuidEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(GuidRulesTestData.IsGuidEmpty.EdgeCases), MemberType = typeof(GuidRulesTestData.IsGuidEmpty))]
    public void IsGuidEmpty_ReturnsFalse_WhenParseFailsOrNonEmpty(GuidRulesTestData.IsGuidEmpty.Case testCase)
    {
        // Act
        var result = GuidRules.IsGuidEmpty(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
