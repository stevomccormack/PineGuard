namespace PineGuard.Fake.UnitTests;

// Violation (a): orphan — no sibling BarTestData.cs exists anywhere in this
// directory, so this file must be flagged [MissingTestData].
public sealed class BarTests
{
    [Theory]
    [InlineData(1)]
    public void Bar_BehavesAsExpected(int value)
    {
        Assert.True(value > 0);
    }
}
