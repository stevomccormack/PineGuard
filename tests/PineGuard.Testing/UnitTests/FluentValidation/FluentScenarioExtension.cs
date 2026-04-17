using PineGuard.Testing.UnitTests.Rules;
using Xunit;

namespace PineGuard.Testing.UnitTests.FluentValidation;

public static class FluentScenarioExtension
{
    public static TheoryData<FluentCase<T>> ToFluentCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<FluentCase<T>>();
        foreach (var s in scenarios)
            data.Add(new FluentCase<T>(s.Name, s.Inputs, new FluentExpected(s.IsValid)));
        return data;
    }

    public static TheoryData<FluentCase<T>> ToFluentCases<T>(this RuleScenario<T>[] scenarios, Func<RuleScenario<T>, FluentExpected> expectedFactory)
    {
        var data = new TheoryData<FluentCase<T>>();
        foreach (var s in scenarios)
            data.Add(new FluentCase<T>(s.Name, s.Inputs, expectedFactory(s)));
        return data;
    }
}
