namespace PineGuard.Testing.UnitTests.MustClauses;

public sealed record MustCase<TValue>(string Name, TValue Value, MustExpected Expected) : ReturnCase<TValue, MustExpected>(Name, Value, Expected);
