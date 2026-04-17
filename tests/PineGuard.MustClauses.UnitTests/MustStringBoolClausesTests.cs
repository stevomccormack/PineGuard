using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringBoolClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    // MustStringBoolClauses.True
    [Theory]
    [MemberData(nameof(MustStringBoolClausesTestData.True.ValidCases), MemberType = typeof(MustStringBoolClausesTestData.True))]
    [MemberData(nameof(MustStringBoolClausesTestData.True.InvalidCases), MemberType = typeof(MustStringBoolClausesTestData.True))]
    public void True_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.True(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    // MustStringBoolClauses.False
    [Theory]
    [MemberData(nameof(MustStringBoolClausesTestData.False.ValidCases), MemberType = typeof(MustStringBoolClausesTestData.False))]
    [MemberData(nameof(MustStringBoolClausesTestData.False.InvalidCases), MemberType = typeof(MustStringBoolClausesTestData.False))]
    public void False_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.False(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
