using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>Fake DataAnnotations attribute — must-usage boundary/ fixture. Never actually calls Must.Be.Qux, only mentions it in a string literal.</summary>
public sealed class FakeQuxAttribute
{
    private const string LegacyReference = "Must.Be.Qux(value)";

    public bool IsValid(bool value) => value;
}
