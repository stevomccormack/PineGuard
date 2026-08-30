using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDecimalClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDecimalClausesTestData.ScaleAtMost.ValidCases), MemberType = typeof(MustDecimalClausesTestData.ScaleAtMost))]
    [MemberData(nameof(MustDecimalClausesTestData.ScaleAtMost.InvalidCases), MemberType = typeof(MustDecimalClausesTestData.ScaleAtMost))]
    public void ScaleAtMost_BehavesAsExpected(MustCase<(decimal value, int scale)> tc)
    {
        // Act
        var result = Must.Be.ScaleAtMost(tc.Value.value, tc.Value.scale, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDecimalClausesTestData.PrecisionAtMost.ValidCases), MemberType = typeof(MustDecimalClausesTestData.PrecisionAtMost))]
    [MemberData(nameof(MustDecimalClausesTestData.PrecisionAtMost.InvalidCases), MemberType = typeof(MustDecimalClausesTestData.PrecisionAtMost))]
    public void PrecisionAtMost_BehavesAsExpected(MustCase<(decimal value, int precision)> tc)
    {
        // Act
        var result = Must.Be.PrecisionAtMost(tc.Value.value, tc.Value.precision, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDecimalClausesTestData.WithinPrecision.ValidCases), MemberType = typeof(MustDecimalClausesTestData.WithinPrecision))]
    [MemberData(nameof(MustDecimalClausesTestData.WithinPrecision.InvalidCases), MemberType = typeof(MustDecimalClausesTestData.WithinPrecision))]
    public void WithinPrecision_BehavesAsExpected(MustCase<(decimal value, int precision, int scale)> tc)
    {
        // Act
        var result = Must.Be.WithinPrecision(tc.Value.value, tc.Value.precision, tc.Value.scale, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
