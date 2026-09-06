using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>Fake Guard clauses calling every Must method — must-usage valid/ fixture.</summary>
public static class FakeGuardClauses
{
    public static void AgainstFoo(bool value, string? paramName = null)
    {
        var result = Must.Be.Foo(value);
        if (!result) throw new ArgumentException("value must be true.", paramName);
    }

    public static void AgainstBar(bool value, string? paramName = null)
    {
        var result = Must.Be.Bar(value);
        if (!result) throw new ArgumentException("value must be false.", paramName);
    }
}
