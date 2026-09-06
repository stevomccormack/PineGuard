namespace Sample.Widget.UnitTests;

using Xunit;

public sealed class WidgetTests
{
    [Fact]
    public void Widget_HasCount()
    {
        var widgets = new Widgets();
        Assert.Equal(0, widgets.Count);
    }
}
