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
/// Fake Must clauses for the must-usage rule's invalid/ fixture. <see cref="Foo"/>
/// is called from every layer below, but <see cref="Baz"/> is never called from
/// any of them — this is the exact "cannot fail" gap the legacy PowerShell tool's
/// Rules 03/04/05 had for months (plan §2.2): this must produce >=1 finding.
/// </summary>
public static class FakeMustClauses
{
    /// <summary>Validates that <paramref name="value"/> is true.</summary>
    public static bool Foo(this IMustClause _, bool value)
    {
        return value;
    }

    /// <summary>Never called from any layer in this fixture tree.</summary>
    public static bool Baz(this IMustClause _, bool value)
    {
        return !value;
    }
}
