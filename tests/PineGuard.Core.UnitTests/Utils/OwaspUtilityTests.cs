using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class OwaspUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSqlInjectionRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsSqlInjectionRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSqlInjectionRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsSqlInjectionRisk))]
    public void ContainsSqlInjectionRisk_ReturnsExpected(OwaspUtilityTestData.ContainsSqlInjectionRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsSqlInjectionRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsPathTraversalRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsPathTraversalRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsPathTraversalRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsPathTraversalRisk))]
    public void ContainsPathTraversalRisk_ReturnsExpected(OwaspUtilityTestData.ContainsPathTraversalRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsPathTraversalRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCommandInjectionRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsCommandInjectionRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCommandInjectionRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsCommandInjectionRisk))]
    public void ContainsCommandInjectionRisk_ReturnsExpected(OwaspUtilityTestData.ContainsCommandInjectionRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsCommandInjectionRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCrLfRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsCrLfRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCrLfRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsCrLfRisk))]
    public void ContainsCrLfRisk_ReturnsExpected(OwaspUtilityTestData.ContainsCrLfRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsCrLfRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsLdapFilterRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsLdapFilterRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsLdapFilterRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsLdapFilterRisk))]
    public void ContainsLdapFilterRisk_ReturnsExpected(OwaspUtilityTestData.ContainsLdapFilterRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsLdapFilterRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsOpenRedirectRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsOpenRedirectRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsOpenRedirectRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsOpenRedirectRisk))]
    public void ContainsOpenRedirectRisk_ReturnsExpected(OwaspUtilityTestData.ContainsOpenRedirectRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsOpenRedirectRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSsrfSchemeRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsSsrfSchemeRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSsrfSchemeRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsSsrfSchemeRisk))]
    public void ContainsSsrfSchemeRisk_ReturnsExpected(OwaspUtilityTestData.ContainsSsrfSchemeRisk.ValidCase testCase)
    {
        // Act
        var result = OwaspUtility.ContainsSsrfSchemeRisk(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
