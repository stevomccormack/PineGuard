using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.GuardClauses;

public sealed record GuardExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null, string? Code = null)
    : ThrowExpected(IsValid, ExceptionType, ParamName, MessageContains);
