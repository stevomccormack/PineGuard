namespace PineGuard.Testing.Common;

public abstract record ReturnExpected(bool IsValid, string? Message = null) : IExpectedResult;
