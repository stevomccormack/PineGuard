using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardChecksumClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotLuhn
    [Theory]
    [MemberData(nameof(GuardChecksumClausesTestData.NotLuhn.ValidCases), MemberType = typeof(GuardChecksumClausesTestData.NotLuhn))]
    [MemberData(nameof(GuardChecksumClausesTestData.NotLuhn.InvalidCases), MemberType = typeof(GuardChecksumClausesTestData.NotLuhn))]
    public void NotLuhn_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotLuhn(value));
        AssertCustomMessage(tc, () => Guard.Against.NotLuhn(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
