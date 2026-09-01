using PineGuard.Testing.Common;

namespace PineGuard.AspNetCore.UnitTests;

public sealed record ProblemDetailsExpected(
    bool IsValid,
    int? Status = null,
    string[]? ErrorKeys = null,
    string[]? Codes = null,
    string[]? Messages = null,
    string? Title = null)
    : ReturnExpected(IsValid);
