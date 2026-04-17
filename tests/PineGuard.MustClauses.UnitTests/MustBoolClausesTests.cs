using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustBoolClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustBoolClausesTestData.True.ValidCases), MemberType = typeof(MustBoolClausesTestData.True))]
    [MemberData(nameof(MustBoolClausesTestData.True.InvalidCases), MemberType = typeof(MustBoolClausesTestData.True))]
    public void True_BehavesAsExpected(MustCase<bool> tc)
    {
        // Act
        var result = Must.Be.True(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBoolClausesTestData.False.ValidCases), MemberType = typeof(MustBoolClausesTestData.False))]
    [MemberData(nameof(MustBoolClausesTestData.False.InvalidCases), MemberType = typeof(MustBoolClausesTestData.False))]
    public void False_BehavesAsExpected(MustCase<bool> tc)
    {
        // Act
        var result = Must.Be.False(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
