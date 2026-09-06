namespace PineGuard.Fake.UnitTests;

// Boundary: uses [Fact(DisplayName = "...")] — the parenthesized form. After
// "Fact" comes "(", which the ported regex's `\s*\(` alternative matches, so
// this IS caught, confirming the parenthesized [Fact(...)] form (unlike the
// verbose [FactAttribute] alias next to this file) is still-invalid.
public sealed class ParameterizedFactTests
{
    [Fact(DisplayName = "still caught")]
    public void ParameterizedFact_BehavesAsExpected()
    {
        Assert.True(ParameterizedFactTestData.AlwaysTrue);
    }
}
