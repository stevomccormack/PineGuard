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
        // Act
        var result = Must.Be.Guid(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGuidClausesTestData.NotEmptyGuid.ValidCases), MemberType = typeof(MustStringGuidClausesTestData.NotEmptyGuid))]
    [MemberData(nameof(MustStringGuidClausesTestData.NotEmptyGuid.InvalidCases), MemberType = typeof(MustStringGuidClausesTestData.NotEmptyGuid))]
    public void NotEmptyGuid_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.NotEmptyGuid(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGuidClausesTestData.HasGuidVersion.ValidCases), MemberType = typeof(MustStringGuidClausesTestData.HasGuidVersion))]
    [MemberData(nameof(MustStringGuidClausesTestData.HasGuidVersion.InvalidCases), MemberType = typeof(MustStringGuidClausesTestData.HasGuidVersion))]
    public void HasGuidVersion_BehavesAsExpected(MustCase<(string? value, int version)> tc)
    {
        // Act
        var result = Must.Be.HasGuidVersion(tc.Value.value, tc.Value.version, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
