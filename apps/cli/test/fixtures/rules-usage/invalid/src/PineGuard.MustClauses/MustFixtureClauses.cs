namespace PineGuard.MustClauses;

/// <summary>
/// Fixture Must clauses calling only FixtureRules.IsFoo — pairs with
/// FixtureRules.cs for the rules-usage VIBE invalid/ case.
/// FixtureRules.IsBaz has no call site here and must be flagged.
/// </summary>
public static class MustFixtureClauses
{
    public static MustResult<string> Foo(this IMustClause _, string? value)
    {
        var ok = FixtureRules.IsFoo(value);
        return MustResult<string>.FromBool(ok, value);
    }
}
