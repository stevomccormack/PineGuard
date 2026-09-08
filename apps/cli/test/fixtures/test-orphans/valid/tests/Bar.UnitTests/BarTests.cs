namespace Sample.Bar.UnitTests;

using Xunit;

public sealed class BarTests
{
    [Fact]
    public void Bar_HasName()
    {
        var bar = new Bar { Name = "example" };
        Assert.Equal("example", bar.Name);
    }
}
