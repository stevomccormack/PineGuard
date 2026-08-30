using PineGuard.Testing.Common;

namespace PineGuard.OneOf.UnitTests;

public sealed record OneOfExpected(bool IsValid, object? Value = null, IReadOnlyList<(string code, string message, string propertyPath)>? Failures = null)
    : ReturnExpected(IsValid);
