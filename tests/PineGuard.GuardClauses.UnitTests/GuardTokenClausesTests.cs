using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardTokenClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotJwt
    [Theory]
    [MemberData(nameof(GuardTokenClausesTestData.NotJwt.ValidCases), MemberType = typeof(GuardTokenClausesTestData.NotJwt))]
    [MemberData(nameof(GuardTokenClausesTestData.NotJwt.InvalidCases), MemberType = typeof(GuardTokenClausesTestData.NotJwt))]
    public void NotJwt_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotJwt(value));
        AssertCustomMessage(tc, () => Guard.Against.NotJwt(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
