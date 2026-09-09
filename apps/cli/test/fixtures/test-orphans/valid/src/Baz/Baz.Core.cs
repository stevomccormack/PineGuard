namespace Sample.Baz;

// Partial-class split, mirroring the repo's own convention (e.g.
// StringRules.Bool.cs). Neither this file nor Baz.Formatting.cs is named
// "Baz.cs" — the old filename-heuristic rule would have found no exact or
// dot-normalised basename match here at all. The new AST-based rule finds
// "Baz" because this file's own declaration carries the type keyword.
public partial class Baz
{
    public string Name { get; init; } = string.Empty;
}
