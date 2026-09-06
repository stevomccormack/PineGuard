namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses calling every FixtureRules method — pairs with
/// FixtureRules.cs for the rules-usage VIBE valid/ case.
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
        var ok = FixtureRules.IsBar(value);
        return MustResult<string>.FromBool(ok, value);
    }
}
