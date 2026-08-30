using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardCronClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotCronExpression
    [Theory]
    [MemberData(nameof(GuardCronClausesTestData.NotCronExpression.ValidCases), MemberType = typeof(GuardCronClausesTestData.NotCronExpression))]
    [MemberData(nameof(GuardCronClausesTestData.NotCronExpression.InvalidCases), MemberType = typeof(GuardCronClausesTestData.NotCronExpression))]
    public void NotCronExpression_BehavesAsExpected(GuardCase<(string? value, CronFormat format)> tc)
    {
        // Arrange
        var (value, format) = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotCronExpression(value, format));
        AssertCustomMessage(tc, () => Guard.Against.NotCronExpression(value, format, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
