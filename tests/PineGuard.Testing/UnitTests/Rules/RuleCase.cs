namespace PineGuard.Testing.UnitTests.Rules;

public sealed record RuleCase<TValue>(string Name, TValue Value, RuleExpected Expected) : ReturnCase<TValue, RuleExpected>(Name, Value, Expected);
