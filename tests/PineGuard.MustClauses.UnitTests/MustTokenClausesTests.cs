using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTokenClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTokenClausesTestData.Jwt.ValidCases), MemberType = typeof(MustTokenClausesTestData.Jwt))]
    [MemberData(nameof(MustTokenClausesTestData.Jwt.InvalidCases), MemberType = typeof(MustTokenClausesTestData.Jwt))]
    public void Jwt_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Jwt(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
