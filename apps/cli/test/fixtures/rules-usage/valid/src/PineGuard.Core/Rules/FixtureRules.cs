namespace PineGuard.Rules;

/// <summary>
/// Fixture Core Rules class for the rules-usage VIBE valid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): every public static method here
/// is called from the paired MustFixtureClauses.cs fixture, so the rule must
/// report zero findings.
/// </summary>
public static class FixtureRules
{
    public static bool IsFoo(string? value) => value == "foo";

    public static bool IsBar(string? value) => value == "bar";
}
