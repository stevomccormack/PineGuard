using PineGuard.Testing.UnitTests;

namespace PineGuard.ErrorOr.UnitTests;

public sealed record ErrorOrCase<TValue>(string Name, TValue Value, ErrorOrExpected Expected)
    : ReturnCase<TValue, ErrorOrExpected>(Name, Value, Expected);
