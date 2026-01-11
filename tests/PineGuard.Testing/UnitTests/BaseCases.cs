using System.ComponentModel;
using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract record BaseCase(string Name)
{
    public override string ToString() => Name;
}

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract record ValueCase<TValue>(
    string Name,
    TValue? Value)
    : BaseCase(Name);


public abstract record ReturnCase<TValue, TResult>(
    string Name,
    TValue? Value, 
    TResult ExpectedReturn)
    : ValueCase<TValue>(Name, Value), IReturnsCase<TResult>;

public abstract record ReturnOutCase<TValue, TResult, TOut>(
    string Name,
    TValue? Value,
    TResult ExpectedReturn,
    TOut? ExpectedOutValue)
    : ReturnCase<TValue, TResult>(Name, Value, ExpectedReturn), IReturnsOutCase<TResult, TOut>;
 