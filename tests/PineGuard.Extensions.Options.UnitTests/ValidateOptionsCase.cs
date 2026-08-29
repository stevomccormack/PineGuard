using PineGuard.Testing.UnitTests;

namespace PineGuard.Extensions.Options.UnitTests;

public sealed record ValidateOptionsCase<TValue>(string Name, TValue Value, ValidateOptionsExpected Expected)
    : ReturnCase<TValue, ValidateOptionsExpected>(Name, Value, Expected);
