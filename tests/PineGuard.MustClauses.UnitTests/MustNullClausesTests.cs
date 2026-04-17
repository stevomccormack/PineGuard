using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustNullClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustNullClausesTestData.Null.ValidCases), MemberType = typeof(MustNullClausesTestData.Null))]
    [MemberData(nameof(MustNullClausesTestData.Null.InvalidCases), MemberType = typeof(MustNullClausesTestData.Null))]
    public void Null_BehavesAsExpected(MustCase<object?> tc)
    {
        var value = tc.Value;
        var result = Must.Be.Null(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustNullClausesTestData.NotNull.ValidCases), MemberType = typeof(MustNullClausesTestData.NotNull))]
    [MemberData(nameof(MustNullClausesTestData.NotNull.InvalidCases), MemberType = typeof(MustNullClausesTestData.NotNull))]
    public void NotNull_BehavesAsExpected(MustCase<object?> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotNull(value);
        AssertResult(tc, result);
    }
}
