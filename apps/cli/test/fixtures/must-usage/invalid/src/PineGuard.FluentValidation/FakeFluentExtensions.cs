using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>Fake Fluent extensions — must-usage invalid/ fixture. Only calls Foo; Baz is never called anywhere in this tree.</summary>
public static class FakeFluentExtensions
{
    public static bool ValidateFoo(bool value) => Must.Be.Foo(value);
}
