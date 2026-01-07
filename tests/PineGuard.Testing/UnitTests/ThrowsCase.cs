using PineGuard.Testing;

namespace PineGuard.Testing.UnitTests;

public abstract record ThrowsCase(
    string Name,
    ExpectedException ExpectedException)
    : BaseCase(Name), IThrowsCase;
