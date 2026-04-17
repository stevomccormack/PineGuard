namespace PineGuard.Testing.UnitTests.FluentValidation;

public sealed record FluentCase<TValue>(string Name, TValue Value, FluentExpected Expected) : ReturnCase<TValue, FluentExpected>(Name, Value, Expected);
