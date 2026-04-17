using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustIdentifierClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustIdentifierClausesTestData.Slug.ValidCases), MemberType = typeof(MustIdentifierClausesTestData.Slug))]
    [MemberData(nameof(MustIdentifierClausesTestData.Slug.InvalidCases), MemberType = typeof(MustIdentifierClausesTestData.Slug))]
    public void Slug_BehavesAsExpected(MustCase<string?> tc)
    {
        var value = tc.Value;
        var result = Must.Be.Slug(value);
        AssertResult(tc, result);
    }
}
