using PineGuard.GuardClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public sealed class GuardTests : BaseUnitTest
{
    [Fact]
    public void Against_ReturnsSingletonClause()
    {
        var first = Guard.Against;
        var second = Guard.Against;

        Assert.NotNull(first);
        Assert.Same(first, second);
    }
}
