namespace PineGuard.Fake.UnitTests;

// Boundary: uses the verbose FactAttribute form (spelled out below) rather
// than its short bracketed alias. The original exact-parity port of the
// legacy regex required the word Fact to be immediately followed (allowing
// only whitespace) by an opening paren or a closing bracket — here it was
// followed by "Attribute" plus a closing bracket, so it did NOT match. That
// was a known, intentionally-preserved legacy blind spot at the time this
// fixture was written. P4.3 (plan §9.2 row P4.3, finding #1) flagged it
// against the root spec (§1/§11: "[Fact] ... disallowed"/"no [Fact] in
// *Tests.cs") and the orchestrator decided to close it (zero occurrences on
// `main`, so widening was a no-op there) — `FACT_ATTRIBUTE_PATTERN` now
// matches the `FactAttribute` alias too, so this file MUST be flagged.
// NOTE: do not spell the short alias out in brackets anywhere in this file's
// comments (including this one) — the pattern matches raw text with no
// comment-awareness, so writing it literally would trip this check from an
// unrelated line.
public sealed class VerboseFactAliasTests
{
    [FactAttribute]
    public void VerboseFactAlias_BehavesAsExpected()
    {
        Assert.True(VerboseFactAliasTestData.AlwaysTrue);
    }
}
