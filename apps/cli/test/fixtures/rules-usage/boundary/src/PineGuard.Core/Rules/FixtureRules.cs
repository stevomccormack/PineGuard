namespace PineGuard.Rules;

/// <summary>
/// Fixture Core Rules class for the rules-usage VIBE boundary/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6). Probes two edges of the rule's
/// own decision boundary against the paired MustFixtureClauses.cs fixture:
/// a <c>nameof()</c>-only reference (<see cref="IsBar"/>) versus a nested
/// static helper class invoked through its full qualified chain
/// (<see cref="Nested.IsQux"/>) — the real shape `StringRules.Bool`/
/// `StringRules.Graphemes` use in src/PineGuard.Core/Rules/StringRules.*.cs.
/// </summary>
public static class FixtureRules
{
    public static bool IsFoo(string? value) => value == "foo";

    public static bool IsBar(string? value) => value == "bar";

    public static class Nested
    {
        public static bool IsQux(string? value) => value == "qux";
    }
}
