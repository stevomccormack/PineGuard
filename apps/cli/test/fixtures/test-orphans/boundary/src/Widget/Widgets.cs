namespace Sample.Widget;

// Singular/plural near-miss: the type is "Widgets" (plural), but
// WidgetTests.cs's own name implies "Widget" (singular). This is exactly
// the shape of real drift the old rule correctly caught
// (MustStringNumberClausesTests vs. MustStringNumbersClauses.cs) — the
// rewrite must not add plural-tolerant fuzzy matching to paper over it.
public sealed class Widgets
{
    public int Count { get; init; }
}
