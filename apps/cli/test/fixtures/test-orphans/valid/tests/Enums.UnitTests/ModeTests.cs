namespace Sample.Enums.UnitTests;

using Xunit;

public sealed class ModeTests
{
    [Fact]
    public void Mode_HasInclusive()
    {
        Assert.Equal(0, (int)Mode.Inclusive);
    }
}
