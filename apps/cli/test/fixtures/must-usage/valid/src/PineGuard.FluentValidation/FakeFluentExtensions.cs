using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>Fake Fluent extensions calling every Must method — must-usage valid/ fixture.</summary>
public static class FakeFluentExtensions
{
    public static bool ValidateFoo(bool value) => Must.Be.Foo(value);

    public static bool ValidateBar(bool value) => Must.Be.Bar(value);
}
