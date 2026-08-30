using PineGuard.Testing.UnitTests;

namespace PineGuard.OneOf.UnitTests;

public sealed record OneOfCase<TValue>(string Name, TValue Value, OneOfExpected Expected)
    : ReturnCase<TValue, OneOfExpected>(Name, Value, Expected);
