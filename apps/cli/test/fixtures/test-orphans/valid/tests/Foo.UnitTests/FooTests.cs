namespace Sample.Foo.UnitTests;

using Xunit;

public sealed class FooTests
{
    [Fact]
    public void Foo_HasName()
    {
        var foo = new Foo { Name = "example" };
        Assert.Equal("example", foo.Name);
    }
}
