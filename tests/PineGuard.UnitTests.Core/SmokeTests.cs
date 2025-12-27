using Xunit;

namespace PineGuard.Tests;

public sealed class SmokeTests
{
    [Fact]
    public void ProjectLoads()
    {
        Assert.Equal("PineGuard.Core", typeof(PineGuard.Rules.StringRules).Assembly.GetName().Name);
    }
}
