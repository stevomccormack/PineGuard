using PineGuard.Testing.Common;

namespace PineGuard.MediatR.UnitTests;

public sealed record MustValidationExpected(bool IsValid, Guid? Response = null, Type? ExceptionType = null, IReadOnlyList<string>? FailurePaths = null, string? Message = null)
    : ReturnExpected(IsValid, Message);
