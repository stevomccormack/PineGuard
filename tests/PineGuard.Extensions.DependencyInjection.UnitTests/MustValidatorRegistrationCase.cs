using PineGuard.Testing.UnitTests;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed record MustValidatorRegistrationCase<TValue>(string Name, TValue Value, MustValidatorRegistrationExpected Expected)
    : ReturnCase<TValue, MustValidatorRegistrationExpected>(Name, Value, Expected);
