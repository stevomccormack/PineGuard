namespace PineGuard.Testing.UnitTests;

public abstract record TryCase<TValue, TOut>(
    string Name,
    TValue? Value,
    bool ExpectedReturn,
    TOut? ExpectedOutValue)
    : ReturnOutCase<TValue, bool, TOut>(Name, Value, ExpectedReturn, ExpectedOutValue)
{
}
