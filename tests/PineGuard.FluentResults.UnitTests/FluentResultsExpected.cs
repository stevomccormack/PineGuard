using PineGuard.Testing.Common;

namespace PineGuard.FluentResults.UnitTests;

public sealed record FluentResultsExpected(bool IsValid, object? Value = null, IReadOnlyList<(string code, string message, string propertyPath)>? Errors = null)
    : ReturnExpected(IsValid);
