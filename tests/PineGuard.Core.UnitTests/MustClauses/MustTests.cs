using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustTestData.Be.ValidCases), MemberType = typeof(MustTestData.Be))]
    public void Be_ReturnsSingletonClause(MustTestData.Be.Case testCase)
    {
        _ = testCase;

        // Act
        var first = Must.Be;
        var second = Must.Be;

        // Assert
        Assert.NotNull(first);
        Assert.Same(first, second);
    }
}
