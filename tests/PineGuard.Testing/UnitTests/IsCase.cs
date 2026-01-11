namespace PineGuard.Testing.UnitTests;

public abstract record IsCase<TValue>(
    string Name,
    TValue? Value,
    bool ExpectedReturn)
    : ReturnCase<TValue, bool>(Name, Value, ExpectedReturn)
{
}
