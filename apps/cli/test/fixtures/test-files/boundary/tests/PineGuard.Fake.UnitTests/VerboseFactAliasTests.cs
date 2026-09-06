namespace PineGuard.Fake.UnitTests;

// Boundary: uses the verbose FactAttribute form (spelled out below) rather
// than its short bracketed alias. The ported legacy regex requires the word
// Fact to be immediately followed (allowing only whitespace) by an opening
// paren or a closing bracket — here it is followed by "Attribute" plus a
// closing bracket, so it does NOT match. This is a known,
// intentionally-preserved blind spot in the legacy rule, ported as-is rather
// than "fixed" (see the header comment in src/audit/rules/test-files.ts).
// Still-valid: this file must NOT be flagged.
// NOTE: do not spell the short alias out in brackets anywhere in this file's
// comments (including this one) — the ported regex matches raw text with no
// comment-awareness, so writing it literally would trip the very check this
// fixture exists to prove does NOT trip.
public sealed class VerboseFactAliasTests
{
    [FactAttribute]
    public void VerboseFactAlias_BehavesAsExpected()
    {
        Assert.True(VerboseFactAliasTestData.AlwaysTrue);
    }
}
