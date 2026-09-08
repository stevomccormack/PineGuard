namespace Sample.Baz.UnitTests;

using Xunit;

public sealed class BazTests
{
    [Fact]
    public void Baz_FormatsName()
    {
        var baz = new Baz { Name = "example" };
        Assert.Equal("example", baz.ToString());
    }
}
