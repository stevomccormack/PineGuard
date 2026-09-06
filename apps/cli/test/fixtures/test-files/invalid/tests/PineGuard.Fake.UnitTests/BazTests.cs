namespace PineGuard.Fake.UnitTests;

// Violation (b): properly paired with BazTestData.cs below, but uses [Fact]
// instead of [Theory] — must be flagged [FactNotAllowed].
public sealed class BazTests
{
    [Fact]
    public void Baz_BehavesAsExpected()
    {
        Assert.True(BazTestData.AlwaysTrue);
    }
}
