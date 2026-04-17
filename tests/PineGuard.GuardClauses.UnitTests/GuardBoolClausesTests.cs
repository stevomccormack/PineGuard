using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardBoolClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.False
    [Theory]
    [MemberData(nameof(GuardBoolClausesTestData.False.ValidCases), MemberType = typeof(GuardBoolClausesTestData.False))]
    [MemberData(nameof(GuardBoolClausesTestData.False.InvalidCases), MemberType = typeof(GuardBoolClausesTestData.False))]
    public void False_BehavesAsExpected(GuardCase<bool> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.False(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.True
    [Theory]
    [MemberData(nameof(GuardBoolClausesTestData.True.ValidCases), MemberType = typeof(GuardBoolClausesTestData.True))]
    [MemberData(nameof(GuardBoolClausesTestData.True.InvalidCases), MemberType = typeof(GuardBoolClausesTestData.True))]
    public void True_BehavesAsExpected(GuardCase<bool> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.True(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
