namespace PineGuard.Testing.UnitTests;

public abstract record TryCase<TValue, TOut>(
    string Name,
    TValue? Value,
    bool ExpectedReturn,
    TOut? ExpectedOutValue)
    : ReturnWithOutCase<TValue, bool, TOut>(Name, Value, ExpectedReturn, ExpectedOutValue)
{
    public bool ExpectedSuccess => ExpectedReturn;
}
