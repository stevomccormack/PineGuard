namespace PineGuard.MustClauses;

/// <summary>
/// Pairs with the boundary/ FixtureRules.cs. <see cref="Foo"/> calls
/// FixtureRules.IsFoo for real (not flagged). <see cref="Bar"/> references
/// FixtureRules.IsBar only via <c>nameof()</c> — not an invocation, so
/// IsBar must still be flagged: the old PowerShell tool's naive
/// <c>FixtureRules\.</c> regex would have matched this nameof() argument
/// too and silently counted it as usage, exactly the false-negative shape
/// this AST-based rule does not repeat. <see cref="Qux"/> calls the nested
/// FixtureRules.Nested.IsQux through its full three-level qualified chain,
/// which must count as real usage (not flagged).
/// </summary>
public static class MustFixtureClauses
{
    public static MustResult<string> Foo(this IMustClause _, string? value)
    {
        var ok = FixtureRules.IsFoo(value);
        return MustResult<string>.FromBool(ok, value);
    }

    public static MustResult<string> Bar(this IMustClause _, string? value)
    {
        var paramName = nameof(FixtureRules.IsBar);
        return MustResult<string>.FromBool(false, paramName, value);
    }

    public static MustResult<string> Qux(this IMustClause _, string? value)
    {
        var ok = FixtureRules.Nested.IsQux(value);
        return MustResult<string>.FromBool(ok, value);
    }
}
