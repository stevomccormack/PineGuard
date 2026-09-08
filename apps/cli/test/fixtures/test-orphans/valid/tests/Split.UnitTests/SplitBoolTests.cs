namespace Sample.Split.UnitTests;

using Xunit;

public sealed class SplitBoolTests
{
    [Fact]
    public void IsTrue_ReturnsInput()
    {
        Assert.True(Split.IsTrue(true));
    }
}
