namespace PineGuard.Testing.Common;

public sealed record ExpectedException(
    Type Type,
    string? ParamName = null,
    string? MessageContains = null);
