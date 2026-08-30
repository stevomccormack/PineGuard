using Microsoft.Extensions.DependencyInjection;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed record MustValidatorResolutionCase(string Name, (Action<IServiceCollection> configureServices, Type validatedType) Value, MustValidatorResolutionExpected Expected)
    : ReturnCase<(Action<IServiceCollection> configureServices, Type validatedType), MustValidatorResolutionExpected>(Name, Value, Expected);
