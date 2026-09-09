namespace Sample.Grouped;

// Family-per-file convention: one source file declares several related types
// and no type shares the file's own name. This is the shape of every
// PineGuard.DataAnnotations source file, and the DataAnnotations addendum's
// own canonical example pairs exactly this with a GroupedAttributesTests.cs
// (docs/ai/specs/data-annotations/unit-test.md, "Canonical Example
// (StringBoolAttributes)": StringBoolAttributes.cs <-> StringBoolAttributesTests.cs,
// where the declared types are TrueStringAttribute / FalseStringAttribute).
// The rule must resolve the test file against the source *file* name here.
public sealed class AlphaAttribute
{
    public string Name { get; init; } = string.Empty;
}

public sealed class BetaAttribute
{
    public string Name { get; init; } = string.Empty;
}
