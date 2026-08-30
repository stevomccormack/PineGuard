using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentResults.UnitTests;

public sealed record FluentResultsCase<TValue>(string Name, TValue Value, FluentResultsExpected Expected)
    : ReturnCase<TValue, FluentResultsExpected>(Name, Value, Expected);
