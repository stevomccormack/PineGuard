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
/// Fake Must clause for the must-usage rule's boundary/ fixture.
/// <see cref="Qux"/> is only ever *mentioned* — inside an XML doc comment,
/// a line comment, and a string literal — by the layer fixtures below, and
/// is never actually invoked as <c>Must.Be.Qux(...)</c>. This probes the
/// rule's own decision boundary: does a textual mention of the call shape
/// get mistaken for a real call site? A naive text/regex search (the
/// legacy tool's whole approach) would say yes; the AST-based
/// `invocation_expression` search this rule uses must say no, since
/// tree-sitter never parses comment or string contents as expressions.
/// Expected: still-invalid (Qux is reported as unused in every layer).
/// </summary>
public static class FakeMustClauses
{
    /// <summary>
    /// See also <c>Must.Be.Qux(value)</c> for reference — this XML doc
    /// comment text is not a real call site.
    /// </summary>
    public static bool Qux(this IMustClause _, bool value)
    {
        return value;
    }
}
