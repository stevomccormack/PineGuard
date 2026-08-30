using PineGuard.Testing.Common;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed record MustValidatorResolutionExpected(bool IsValid, int ValidatorCount, Type? ValidatorType = null, string? Message = null)
    : ReturnExpected(IsValid, Message);
