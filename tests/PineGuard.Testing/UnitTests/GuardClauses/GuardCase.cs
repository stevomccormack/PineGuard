namespace PineGuard.Testing.UnitTests.GuardClauses;

public sealed record GuardCase<TValue>(string Name, TValue Value, GuardExpected Expected) : ReturnCase<TValue, GuardExpected>(Name, Value, Expected);
