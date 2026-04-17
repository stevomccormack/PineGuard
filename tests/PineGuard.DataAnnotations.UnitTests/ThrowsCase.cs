using PineGuard.Testing.Common;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed record ThrowsCase(string Name, object? Value, ExpectedException ExpectedException)
    : IThrowsCase
{
    public override string ToString() => Name;
}
