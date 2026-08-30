using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDecimalClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.ScaleAbove
    [Theory]
    [MemberData(nameof(GuardDecimalClausesTestData.ScaleAbove.ValidCases), MemberType = typeof(GuardDecimalClausesTestData.ScaleAbove))]
    [MemberData(nameof(GuardDecimalClausesTestData.ScaleAbove.InvalidCases), MemberType = typeof(GuardDecimalClausesTestData.ScaleAbove))]
    public void ScaleAbove_BehavesAsExpected(GuardCase<(decimal value, int scale)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.ScaleAbove(value, tc.Value.scale));
        AssertCustomMessage(tc, () => Guard.Against.ScaleAbove(value, tc.Value.scale, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.PrecisionAbove
    [Theory]
    [MemberData(nameof(GuardDecimalClausesTestData.PrecisionAbove.ValidCases), MemberType = typeof(GuardDecimalClausesTestData.PrecisionAbove))]
    [MemberData(nameof(GuardDecimalClausesTestData.PrecisionAbove.InvalidCases), MemberType = typeof(GuardDecimalClausesTestData.PrecisionAbove))]
    public void PrecisionAbove_BehavesAsExpected(GuardCase<(decimal value, int precision)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.PrecisionAbove(value, tc.Value.precision));
        AssertCustomMessage(tc, () => Guard.Against.PrecisionAbove(value, tc.Value.precision, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotWithinPrecision
    [Theory]
    [MemberData(nameof(GuardDecimalClausesTestData.NotWithinPrecision.ValidCases), MemberType = typeof(GuardDecimalClausesTestData.NotWithinPrecision))]
    [MemberData(nameof(GuardDecimalClausesTestData.NotWithinPrecision.InvalidCases), MemberType = typeof(GuardDecimalClausesTestData.NotWithinPrecision))]
    public void NotWithinPrecision_BehavesAsExpected(GuardCase<(decimal value, int precision, int scale)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotWithinPrecision(value, tc.Value.precision, tc.Value.scale));
        AssertCustomMessage(tc, () => Guard.Against.NotWithinPrecision(value, tc.Value.precision, tc.Value.scale, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
