namespace Sample.Enums;

// An enum is a named type a *Tests.cs file can legitimately target — the repo
// does exactly this in tests/PineGuard.Core.UnitTests/Common/InclusionTests.cs
// against src/PineGuard.Core/Common/Inclusion.cs (fixture.md §9 lists
// `Inclusion (enum)` among the constant-bearing types fixtures reference).
// The file is deliberately NOT named Mode.cs, so only the type-declaration
// lookup — not the source-file-name fallback — can resolve ModeTests.cs.
public enum Mode
{
    Inclusive = 0,
    Exclusive = 1,
}
