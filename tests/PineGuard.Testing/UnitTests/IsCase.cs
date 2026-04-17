using System.ComponentModel;

namespace PineGuard.Testing.UnitTests;

[Description("Use RuleCase<T> for rules.")]
public abstract record IsCase<TValue>(
    string Name,
    TValue Value,
    bool Expected)
    : ReturnCase<TValue, bool>(Name, Value, Expected);
