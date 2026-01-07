namespace PineGuard.Testing.UnitTests;

public abstract record BaseCase(string Name)
{
    public override string ToString() => Name;
}

public abstract record ValueCase<TValue>(
    string Name,
    TValue? Value)
    : BaseCase(Name);

public abstract record ReturnCase<TValue, TResult>(
    string Name,
    TValue? Value,
    TResult ExpectedReturn)
    : ValueCase<TValue>(Name, Value);

public abstract record ReturnWithOutCase<TValue, TResult, TOut>(
    string Name,
    TValue? Value,
    TResult ExpectedReturn,
    TOut? ExpectedOutValue)
    : ReturnCase<TValue, TResult>(Name, Value, ExpectedReturn);
