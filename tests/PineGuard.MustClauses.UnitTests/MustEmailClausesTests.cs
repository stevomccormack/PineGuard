using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustEmailClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustEmailClausesTestData.Email.ValidCases), MemberType = typeof(MustEmailClausesTestData.Email))]
    [MemberData(nameof(MustEmailClausesTestData.Email.InvalidCases), MemberType = typeof(MustEmailClausesTestData.Email))]
    public void Email_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Email(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEmailClausesTestData.StrictEmail.ValidCases), MemberType = typeof(MustEmailClausesTestData.StrictEmail))]
    [MemberData(nameof(MustEmailClausesTestData.StrictEmail.InvalidCases), MemberType = typeof(MustEmailClausesTestData.StrictEmail))]
    public void StrictEmail_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.StrictEmail(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEmailClausesTestData.HasEmailAlias.ValidCases), MemberType = typeof(MustEmailClausesTestData.HasEmailAlias))]
    [MemberData(nameof(MustEmailClausesTestData.HasEmailAlias.InvalidCases), MemberType = typeof(MustEmailClausesTestData.HasEmailAlias))]
    public void HasEmailAlias_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.HasEmailAlias(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEmailClausesTestData.NotHasEmailAlias.ValidCases), MemberType = typeof(MustEmailClausesTestData.NotHasEmailAlias))]
    [MemberData(nameof(MustEmailClausesTestData.NotHasEmailAlias.InvalidCases), MemberType = typeof(MustEmailClausesTestData.NotHasEmailAlias))]
    public void NotHasEmailAlias_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.NotHasEmailAlias(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
