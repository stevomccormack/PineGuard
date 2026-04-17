using System.ComponentModel;
using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract record BaseCase(string Name)
{
    public sealed override string ToString() => Name;
}

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract record ValueCase<TValue>(
    string Name,
    TValue Value)
    : BaseCase(Name);

public abstract record ReturnCase<TValue, TExpected>(
    string Name,
    TValue Value,
    TExpected Expected)
    : ValueCase<TValue>(Name, Value), IReturnsCase<TExpected>;

public abstract record ReturnOutCase<TValue, TExpected, TOut>(
    string Name,
    TValue Value,
    TExpected Expected,
    TOut ExpectedOutValue)
    : ReturnCase<TValue, TExpected>(Name, Value, Expected), IReturnsOutCase<TExpected, TOut>;
