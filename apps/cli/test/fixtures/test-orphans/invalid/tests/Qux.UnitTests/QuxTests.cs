namespace Sample.Qux.UnitTests;

using Xunit;

public sealed class QuxTests
{
    [Fact]
    public void Qux_HasName()
    {
        var qux = new Qux { Name = "example" };
        Assert.Equal("example", qux.Name);
    }
}
