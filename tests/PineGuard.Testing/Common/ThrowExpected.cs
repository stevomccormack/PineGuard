namespace PineGuard.Testing.Common;

public abstract record ThrowExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null) : IExpectedResult;
