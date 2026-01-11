using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Owasp;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class OwaspUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSqlInjectionRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsSqlInjectionRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSqlInjectionRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsSqlInjectionRisk))]
    public void ContainsSqlInjectionRisk_ReturnsExpected(OwaspUtilityTestData.ContainsSqlInjectionRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsSqlInjectionRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsPathTraversalRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsPathTraversalRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsPathTraversalRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsPathTraversalRisk))]
    public void ContainsPathTraversalRisk_ReturnsExpected(OwaspUtilityTestData.ContainsPathTraversalRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsPathTraversalRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCommandInjectionRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsCommandInjectionRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCommandInjectionRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsCommandInjectionRisk))]
    public void ContainsCommandInjectionRisk_ReturnsExpected(OwaspUtilityTestData.ContainsCommandInjectionRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsCommandInjectionRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCrLfRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsCrLfRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsCrLfRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsCrLfRisk))]
    public void ContainsCrLfRisk_ReturnsExpected(OwaspUtilityTestData.ContainsCrLfRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsCrLfRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsLdapFilterRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsLdapFilterRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsLdapFilterRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsLdapFilterRisk))]
    public void ContainsLdapFilterRisk_ReturnsExpected(OwaspUtilityTestData.ContainsLdapFilterRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsLdapFilterRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsOpenRedirectRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsOpenRedirectRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsOpenRedirectRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsOpenRedirectRisk))]
    public void ContainsOpenRedirectRisk_ReturnsExpected(OwaspUtilityTestData.ContainsOpenRedirectRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsOpenRedirectRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSsrfSchemeRisk.ValidCases), MemberType = typeof(OwaspUtilityTestData.ContainsSsrfSchemeRisk))]
    [MemberData(nameof(OwaspUtilityTestData.ContainsSsrfSchemeRisk.EdgeCases), MemberType = typeof(OwaspUtilityTestData.ContainsSsrfSchemeRisk))]
    public void ContainsSsrfSchemeRisk_ReturnsExpected(OwaspUtilityTestData.ContainsSsrfSchemeRisk.Case testCase)
    {
        var result = OwaspUtility.ContainsSsrfSchemeRisk(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
