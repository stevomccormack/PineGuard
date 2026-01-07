namespace PineGuard.Testing.UnitTests;

public abstract record HasCase<TValue>(
    string Name,
    TValue? Value,
    bool ExpectedReturn)
    : IsCase<TValue>(Name, Value, ExpectedReturn);
