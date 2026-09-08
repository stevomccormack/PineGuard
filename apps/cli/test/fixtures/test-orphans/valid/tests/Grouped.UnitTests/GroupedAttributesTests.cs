namespace Sample.Grouped.UnitTests;

using Xunit;

public sealed class GroupedAttributesTests
{
    [Fact]
    public void Alpha_HasName()
    {
        var alpha = new AlphaAttribute { Name = "example" };
        Assert.Equal("example", alpha.Name);
    }
}
