namespace PineGuard.Testing.UnitTests.MustClauses;

public sealed record MustValidationCase<TValue>(string Name, TValue Value, MustValidationExpected Expected) : ReturnCase<TValue, MustValidationExpected>(Name, Value, Expected);
