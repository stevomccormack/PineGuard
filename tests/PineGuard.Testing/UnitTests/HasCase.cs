using System.ComponentModel;

namespace PineGuard.Testing.UnitTests;

[Description("Use RuleCase<T> for rules.")]
public abstract record HasCase<TValue>(
    string Name,
    TValue Value,
    bool Expected)
    : ReturnCase<TValue, bool>(Name, Value, Expected);
