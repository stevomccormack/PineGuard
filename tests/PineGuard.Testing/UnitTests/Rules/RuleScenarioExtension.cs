using Xunit;

namespace PineGuard.Testing.UnitTests.Rules;

public static class RuleScenarioExtension
{
    // Filter combinators
    public static RuleScenario<T>[] WhereValid<T>(this RuleScenario<T>[] scenarios) =>
        scenarios.Where(s => s.IsValid).ToArray();

    public static RuleScenario<T>[] WhereInvalid<T>(this RuleScenario<T>[] scenarios) =>
        scenarios.Where(s => !s.IsValid).ToArray();

    public static RuleScenario<T>[] Except<T>(this RuleScenario<T>[] scenarios, params string[] names) =>
        scenarios.Where(s => !names.Contains(s.Name, StringComparer.Ordinal)).ToArray();

    public static RuleScenario<T>[] Only<T>(this RuleScenario<T>[] scenarios, params string[] names) =>
        scenarios.Where(s => names.Contains(s.Name, StringComparer.Ordinal)).ToArray();

    // Projection
    public static RuleScenario<V>[] Project<T, V>(this RuleScenario<T>[] scenarios, Func<T, V> selector) =>
        scenarios.Select(s => new RuleScenario<V>(s.Name, selector(s.Inputs), s.IsValid)).ToArray();

    // Rules layer
    public static TheoryData<RuleCase<T>> ToRuleCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<RuleCase<T>>();
        foreach (var s in scenarios)
            data.Add(new RuleCase<T>(s.Name, s.Inputs, new RuleExpected(s.IsValid)));
        return data;
    }
}
