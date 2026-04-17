using PineGuard.Testing.UnitTests.Rules;
using Xunit;

namespace PineGuard.Testing.UnitTests.MustClauses;

public static class MustScenarioExtension
{
    public static TheoryData<MustCase<T>> ToMustCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<MustCase<T>>();
        foreach (var s in scenarios)
            data.Add(new MustCase<T>(s.Name, s.Inputs, new MustExpected(s.IsValid)));
        return data;
    }

    public static TheoryData<MustCase<T>> ToMustCases<T>(this RuleScenario<T>[] scenarios, Func<RuleScenario<T>, MustExpected> expectedFactory)
    {
        var data = new TheoryData<MustCase<T>>();
        foreach (var s in scenarios)
            data.Add(new MustCase<T>(s.Name, s.Inputs, expectedFactory(s)));
        return data;
    }
}
