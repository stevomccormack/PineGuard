using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringGuidClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringGuidClausesTestData.Guid.ValidCases), MemberType = typeof(MustStringGuidClausesTestData.Guid))]
    [MemberData(nameof(MustStringGuidClausesTestData.Guid.InvalidCases), MemberType = typeof(MustStringGuidClausesTestData.Guid))]
    public void Guid_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Guid(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGuidClausesTestData.NotEmptyGuid.ValidCases), MemberType = typeof(MustStringGuidClausesTestData.NotEmptyGuid))]
    [MemberData(nameof(MustStringGuidClausesTestData.NotEmptyGuid.InvalidCases), MemberType = typeof(MustStringGuidClausesTestData.NotEmptyGuid))]
    public void NotEmptyGuid_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotEmptyGuid(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }
}
