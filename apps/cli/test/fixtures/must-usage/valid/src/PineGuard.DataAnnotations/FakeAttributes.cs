using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>Fake DataAnnotations attributes calling every Must method — must-usage valid/ fixture.</summary>
public sealed class FakeFooAttribute
{
    public bool IsValid(bool value) => Must.Be.Foo(value);
}

/// <summary>Fake DataAnnotations attribute calling the other Must method — must-usage valid/ fixture.</summary>
public sealed class FakeBarAttribute
{
    public bool IsValid(bool value) => Must.Be.Bar(value);
}
