using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustTests : BaseUnitTest
{
    [Fact]
    public void Be_ReturnsSingletonClause()
    {
        var first = Must.Be;
        var second = Must.Be;

        Assert.NotNull(first);
        Assert.Same(first, second);
    }
}
