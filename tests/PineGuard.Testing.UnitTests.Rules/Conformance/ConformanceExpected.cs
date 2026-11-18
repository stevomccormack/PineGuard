using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.Rules.Conformance;

public sealed record ConformanceExpected(bool IsValid, bool IsValidAllowNull, DateOnly Result, string ParamName, object? Value, string ProjectedCode, string? MessageTemplate = null, string? Code = null, Type? ExceptionType = null)
    : ReturnExpected(IsValid);
