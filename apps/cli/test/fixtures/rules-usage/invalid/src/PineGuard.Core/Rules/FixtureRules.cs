namespace PineGuard.Rules;

/// <summary>
/// Fixture Core Rules class for the rules-usage VIBE invalid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): <see cref="IsBaz"/> below is
/// never called from the paired MustFixtureClauses.cs fixture, so the rule
/// must report at least one finding — this is the mandatory "can it fail"
/// fixture (the exact gap that let the old tool's Rules 03/04/05 go silently
/// vacuous for months; see the rule's own header comment).
/// </summary>
public static class FixtureRules
{
    public static bool IsFoo(string? value) => value == "foo";

    public static bool IsBaz(string? value) => value == "baz";
}
