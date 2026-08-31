using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustVersionClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustVersionClausesTestData.SemVer.ValidCases), MemberType = typeof(MustVersionClausesTestData.SemVer))]
    [MemberData(nameof(MustVersionClausesTestData.SemVer.InvalidCases), MemberType = typeof(MustVersionClausesTestData.SemVer))]
    public void SemVer_BehavesAsExpected(MustCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act
        var result = Must.Be.SemVer(value);

        // Assert
        AssertResult(tc, result);
    }
}
