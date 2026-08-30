using PineGuard.Testing.Common;

namespace PineGuard.FluentValidation.UnitTests;

public sealed record ValidationBridgeExpected(bool IsValid, IReadOnlyList<(string propertyPath, string code, string message)>? Failures = null, object? Value = null)
    : ReturnExpected(IsValid);
