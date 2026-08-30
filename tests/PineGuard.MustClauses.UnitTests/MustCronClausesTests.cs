using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustCronClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustCronClausesTestData.CronExpression.ValidCases), MemberType = typeof(MustCronClausesTestData.CronExpression))]
    [MemberData(nameof(MustCronClausesTestData.CronExpression.InvalidCases), MemberType = typeof(MustCronClausesTestData.CronExpression))]
    public void CronExpression_BehavesAsExpected(MustCase<(string? value, CronFormat format)> tc)
    {
        // Act
        var result = Must.Be.CronExpression(tc.Value.value, tc.Value.format, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
