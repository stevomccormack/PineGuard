using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustGuidClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustGuidClausesTestData.NotEmpty.ValidCases), MemberType = typeof(MustGuidClausesTestData.NotEmpty))]
    [MemberData(nameof(MustGuidClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(MustGuidClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(MustCase<Guid> tc)
    {
        // Act
        var result = Must.Be.NotEmpty(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
