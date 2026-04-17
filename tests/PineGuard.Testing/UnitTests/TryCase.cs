namespace PineGuard.Testing.UnitTests;

public abstract record TryCase<TValue, TOut>(
    string Name,
    TValue Value,
    bool Expected,
    TOut ExpectedOutValue)
    : ReturnOutCase<TValue, bool, TOut>(Name, Value, Expected, ExpectedOutValue);
