using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustChecksumClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustChecksumClausesTestData.Luhn.ValidCases), MemberType = typeof(MustChecksumClausesTestData.Luhn))]
    [MemberData(nameof(MustChecksumClausesTestData.Luhn.InvalidCases), MemberType = typeof(MustChecksumClausesTestData.Luhn))]
    public void Luhn_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Luhn(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
