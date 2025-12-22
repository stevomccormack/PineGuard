using Xunit;

namespace PineGuard.Tests;

public sealed class SmokeTests
{
    [Fact]
    public void ProjectLoads()
    {
        Assert.Equal("PineGuard", PineGuard.PineGuardInfo.Name);
    }
}
