namespace PineGuard.MustClauses;

/// <summary>Marker interface for the fluent Must.Be.* entry point (fixture stand-in for the real PineGuard.Core.MustClauses.IMustClause).</summary>
public interface IMustClause;

/// <summary>Entry point stand-in for Must.Be.* (fixture stand-in for the real PineGuard.Core.MustClauses.Must).</summary>
public static class Must
{
    private sealed class MustClauseImpl : IMustClause;

    public static IMustClause Be { get; } = new MustClauseImpl();
}

/// <summary>
/// Fake Must clauses for the must-usage rule's valid/ fixture. Every method
/// below is called from all three layers under this fixture tree (Guard,
/// Fluent, DataAnnotations) — this must produce zero findings.
/// </summary>
public static class FakeMustClauses
{
    /// <summary>Validates that <paramref name="value"/> is true.</summary>
    public static bool Foo(this IMustClause _, bool value)
    {
        return value;
    }

    /// <summary>Validates that <paramref name="value"/> is false.</summary>
    public static bool Bar(this IMustClause _, bool value)
    {
        return !value;
    }
}
