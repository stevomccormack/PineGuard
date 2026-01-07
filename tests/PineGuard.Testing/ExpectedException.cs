namespace PineGuard.Testing;

public sealed record ExpectedException(
    Type Type,
    string? ParamName = null,
    string? MessageContains = null);
