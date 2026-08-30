using PineGuard.Testing.Common;

namespace PineGuard.ErrorOr.UnitTests;

public sealed record ErrorOrExpected(bool IsValid, object? Value = null, IReadOnlyList<(string code, string description, string propertyPath)>? Errors = null)
    : ReturnExpected(IsValid);
