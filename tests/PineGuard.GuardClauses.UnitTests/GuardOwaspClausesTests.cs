using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardOwaspClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.OwaspUnsafe.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.OwaspUnsafe))]
    [MemberData(nameof(GuardOwaspClausesTestData.OwaspUnsafe.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.OwaspUnsafe))]
    public void OwaspUnsafe_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.OwaspUnsafe(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.Xss.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.Xss))]
    [MemberData(nameof(GuardOwaspClausesTestData.Xss.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.Xss))]
    public void Xss_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.Xss(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.SqlInjection.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.SqlInjection))]
    [MemberData(nameof(GuardOwaspClausesTestData.SqlInjection.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.SqlInjection))]
    public void SqlInjection_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.SqlInjection(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.PathTraversal.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.PathTraversal))]
    [MemberData(nameof(GuardOwaspClausesTestData.PathTraversal.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.PathTraversal))]
    public void PathTraversal_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.PathTraversal(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.CommandInjection.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.CommandInjection))]
    [MemberData(nameof(GuardOwaspClausesTestData.CommandInjection.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.CommandInjection))]
    public void CommandInjection_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.CommandInjection(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.CrLf.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.CrLf))]
    [MemberData(nameof(GuardOwaspClausesTestData.CrLf.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.CrLf))]
    public void CrLf_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.CrLf(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.LdapFilter.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.LdapFilter))]
    [MemberData(nameof(GuardOwaspClausesTestData.LdapFilter.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.LdapFilter))]
    public void LdapFilter_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.LdapFilter(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.OpenRedirect.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.OpenRedirect))]
    [MemberData(nameof(GuardOwaspClausesTestData.OpenRedirect.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.OpenRedirect))]
    public void OpenRedirect_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.OpenRedirect(tc.Value!, paramName: "value"));
    }

    [Theory]
    [MemberData(nameof(GuardOwaspClausesTestData.SsrfScheme.ValidCases), MemberType = typeof(GuardOwaspClausesTestData.SsrfScheme))]
    [MemberData(nameof(GuardOwaspClausesTestData.SsrfScheme.InvalidCases), MemberType = typeof(GuardOwaspClausesTestData.SsrfScheme))]
    public void SsrfScheme_BehavesAsExpected(GuardCase<string?> tc)
    {
        AssertResult(tc, () => Guard.Against.SsrfScheme(tc.Value!, paramName: "value"));
    }
}
