using Microsoft.Extensions.DependencyInjection;
using PineGuard.Testing.Common;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed record MustValidatorRegistrationExpected(bool IsValid, Type[] ServiceTypes, ServiceLifetime Lifetime, string? Message = null)
    : ReturnExpected(IsValid, Message);
