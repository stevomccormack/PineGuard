using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>Fake DataAnnotations attribute — must-usage invalid/ fixture. Only calls Foo; Baz is never called anywhere in this tree.</summary>
public sealed class FakeFooAttribute
{
    public bool IsValid(bool value) => Must.Be.Foo(value);
}
