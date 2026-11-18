namespace PineGuard.Testing.UnitTests.Rules.Conformance;

public sealed record ConformanceCase<TValue>(string Name, TValue Value, ConformanceExpected Expected)
    : ReturnCase<TValue, ConformanceExpected>(Name, Value, Expected);
