namespace Demo;

// This fixture is deliberately NOT a real rule's fixture pair — it exists
// solely so runRule.test.ts can prove `expectInvalid` throws when an
// `invalid/` fixture doesn't actually violate the rule (the exact failure
// mode `expectInvalid`/the old `expectRuleCanFail` exists to catch: see
// docs/ai/plans/audit-cli-rebuild.md §2.2-2.3 and §4.6). It has no `valid/`
// or `boundary/` sibling because it is not a P2 rule fixture and the "every
// rule must have valid+invalid" mandate in §4.6 does not apply to it.
public static class CleanFile
{
    public static string Hello(string name) => $"Hello, {name}!";
}
