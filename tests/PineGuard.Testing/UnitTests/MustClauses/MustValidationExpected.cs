using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.MustClauses;

public sealed record MustValidationExpected(bool IsValid, string? Message = null, int? FailureCount = null, string? PropertyPath = null, string? Code = null) : ReturnExpected(IsValid, Message);
