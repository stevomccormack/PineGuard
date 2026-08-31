using PineGuard.Testing.UnitTests;

namespace PineGuard.MediatR.UnitTests;

public sealed record MustValidationCase<TValue>(string Name, TValue Value, MustValidationExpected Expected)
    : ReturnCase<TValue, MustValidationExpected>(Name, Value, Expected);
