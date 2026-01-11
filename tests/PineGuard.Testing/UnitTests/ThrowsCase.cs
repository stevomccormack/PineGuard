using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests;

public abstract record ThrowsCase<TValue>(
    string Name,
    TValue? Value,
    ExpectedException ExpectedException)
    : ValueCase<TValue>(Name, Value), IThrowsCase
{
    protected ThrowsCase(string name, TValue? value, Type exceptionType)
        : this(name, value, new ExpectedException(exceptionType))
    {
    }

    protected ThrowsCase(string name, TValue? value, Type exceptionType, string? paramName)
        : this(name, value, new ExpectedException(exceptionType, paramName))
    {
    }

    protected ThrowsCase(string name, TValue? value, Type exceptionType, string? paramName, string? messageContains)
        : this(name, value, new ExpectedException(exceptionType, paramName, messageContains))
    {
    }
}