using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>Fake Guard clauses — must-usage invalid/ fixture. Only calls Foo; Baz is never called anywhere in this tree.</summary>
public static class FakeGuardClauses
{
    public static void AgainstFoo(bool value, string? paramName = null)
    {
        var result = Must.Be.Foo(value);
        if (!result) throw new ArgumentException("value must be true.", paramName);
    }
}
