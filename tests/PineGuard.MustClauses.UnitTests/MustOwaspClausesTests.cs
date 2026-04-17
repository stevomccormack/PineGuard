using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustOwaspClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.OwaspSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.OwaspSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.OwaspSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.OwaspSafe))]
    public void OwaspSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.OwaspSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.XssSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.XssSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.XssSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.XssSafe))]
    public void XssSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.XssSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.SqlInjectionSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.SqlInjectionSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.SqlInjectionSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.SqlInjectionSafe))]
    public void SqlInjectionSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.SqlInjectionSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.PathTraversalSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.PathTraversalSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.PathTraversalSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.PathTraversalSafe))]
    public void PathTraversalSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.PathTraversalSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.CommandInjectionSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.CommandInjectionSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.CommandInjectionSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.CommandInjectionSafe))]
    public void CommandInjectionSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.CommandInjectionSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.CrLfSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.CrLfSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.CrLfSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.CrLfSafe))]
    public void CrLfSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.CrLfSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.LdapFilterSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.LdapFilterSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.LdapFilterSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.LdapFilterSafe))]
    public void LdapFilterSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.LdapFilterSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.OpenRedirectSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.OpenRedirectSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.OpenRedirectSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.OpenRedirectSafe))]
    public void OpenRedirectSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.OpenRedirectSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustOwaspClausesTestData.SsrfSchemeSafe.ValidCases), MemberType = typeof(MustOwaspClausesTestData.SsrfSchemeSafe))]
    [MemberData(nameof(MustOwaspClausesTestData.SsrfSchemeSafe.InvalidCases), MemberType = typeof(MustOwaspClausesTestData.SsrfSchemeSafe))]
    public void SsrfSchemeSafe_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.SsrfSchemeSafe(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
