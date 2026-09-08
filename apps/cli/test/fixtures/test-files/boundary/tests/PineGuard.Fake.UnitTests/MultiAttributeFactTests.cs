namespace PineGuard.Fake.UnitTests;

// Boundary: [Fact, Trait(...)] — a multi-attribute-in-one-bracket form the
// original exact-parity port of the legacy regex also missed (it only ever
// looked for Fact immediately followed by "(" or "]", never by ","). P4.3
// (plan §9.2 row P4.3, finding #1) flagged this against the root spec
// (§1/§11) and the orchestrator decided to close it — this file MUST now be
// flagged, in both attribute orderings.
public sealed class MultiAttributeFactTests
{
    [Fact, Trait("Category", "Smoke")]
    public void FactFirst_BehavesAsExpected()
    {
        Assert.True(MultiAttributeFactTestData.AlwaysTrue);
    }

    [Trait("Category", "Smoke"), Fact]
    public void FactLast_BehavesAsExpected()
    {
        Assert.True(MultiAttributeFactTestData.AlwaysTrue);
    }
}
