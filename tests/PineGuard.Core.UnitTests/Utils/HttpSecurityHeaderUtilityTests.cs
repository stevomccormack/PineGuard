using PineGuard.Testing.UnitTests;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class HttpSecurityHeaderUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(HttpSecurityHeaderUtilityTestData.TrySplitSemicolonSeparatedSegments.ValidCases), MemberType = typeof(HttpSecurityHeaderUtilityTestData.TrySplitSemicolonSeparatedSegments))]
    [MemberData(nameof(HttpSecurityHeaderUtilityTestData.TrySplitSemicolonSeparatedSegments.EdgeCases), MemberType = typeof(HttpSecurityHeaderUtilityTestData.TrySplitSemicolonSeparatedSegments))]
    public void TrySplitSemicolonSeparatedSegments_ReturnsExpected(HttpSecurityHeaderUtilityTestData.TrySplitSemicolonSeparatedSegments.ValidCase testCase)
    {
        // Act
        var ok = HttpSecurityHeaderUtility.TrySplitSemicolonSeparatedSegments(testCase.Value, out var segments);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, segments);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderUtilityTestData.ParseHstsDirectives.ValidCases), MemberType = typeof(HttpSecurityHeaderUtilityTestData.ParseHstsDirectives))]
    [MemberData(nameof(HttpSecurityHeaderUtilityTestData.ParseHstsDirectives.EdgeCases), MemberType = typeof(HttpSecurityHeaderUtilityTestData.ParseHstsDirectives))]
    public void ParseHstsDirectives_ReturnsExpected(HttpSecurityHeaderUtilityTestData.ParseHstsDirectives.Case testCase)
    {
        // Act
        var result = HttpSecurityHeaderUtility.ParseHstsDirectives(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected.MaxAgeSeconds, result.MaxAgeSeconds);
        Assert.Equal(testCase.Expected.IncludeSubDomains, result.IncludeSubDomains);
        Assert.Equal(testCase.Expected.Preload, result.Preload);
    }

    [Theory]
    [MemberData(nameof(HttpSecurityHeaderUtilityTestData.HstsDirectivesWithExpression.Cases), MemberType = typeof(HttpSecurityHeaderUtilityTestData.HstsDirectivesWithExpression))]
    public void HstsDirectives_WithExpression_MutatesAllFields(HttpSecurityHeaderUtilityTestData.HstsDirectivesWithExpression.Case testCase)
    {
        // Arrange
        var original = new HttpSecurityHeaderUtility.HstsDirectives(
            testCase.Value.MaxAgeSeconds,
            testCase.Value.IncludeSubDomains,
            testCase.Value.Preload);

        // Act
        // ReSharper disable once WithExpressionModifiesAllMembers
        var mutated = original with
        {
            MaxAgeSeconds = testCase.Mutated.MaxAgeSeconds,
            IncludeSubDomains = testCase.Mutated.IncludeSubDomains,
            Preload = testCase.Mutated.Preload
        };

        // Assert
        Assert.Equal(testCase.Mutated.MaxAgeSeconds, mutated.MaxAgeSeconds);
        Assert.Equal(testCase.Mutated.IncludeSubDomains, mutated.IncludeSubDomains);
        Assert.Equal(testCase.Mutated.Preload, mutated.Preload);
    }
}
