using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardVersionClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotSemVer
    [Theory]
    [MemberData(nameof(GuardVersionClausesTestData.NotSemVer.ValidCases), MemberType = typeof(GuardVersionClausesTestData.NotSemVer))]
    [MemberData(nameof(GuardVersionClausesTestData.NotSemVer.InvalidCases), MemberType = typeof(GuardVersionClausesTestData.NotSemVer))]
    public void NotSemVer_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotSemVer(value));
        AssertCustomMessage(tc, () => Guard.Against.NotSemVer(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
