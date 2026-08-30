using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public sealed record ValidationBridgeCase<TValue>(string Name, TValue Value, ValidationBridgeExpected Expected)
    : ReturnCase<TValue, ValidationBridgeExpected>(Name, Value, Expected);
