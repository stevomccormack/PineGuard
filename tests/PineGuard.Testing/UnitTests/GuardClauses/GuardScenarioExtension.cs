using PineGuard.Testing.UnitTests.Rules;
using Xunit;

namespace PineGuard.Testing.UnitTests.GuardClauses;

public static class GuardScenarioExtension
{
    public static TheoryData<GuardCase<T>> ToGuardCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<GuardCase<T>>();
        foreach (var s in scenarios)
            data.Add(new GuardCase<T>(s.Name, s.Inputs, new GuardExpected(s.IsValid)));
        return data;
    }

    public static TheoryData<GuardCase<T>> ToGuardCases<T>(this RuleScenario<T>[] scenarios, string paramName)
    {
        var data = new TheoryData<GuardCase<T>>();
        foreach (var s in scenarios)
        {
            var expected = s.IsValid
                ? new GuardExpected(true)
                : s.IsNull
                    ? new GuardExpected(false, typeof(ArgumentNullException), paramName)
                    : new GuardExpected(false, typeof(ArgumentException), paramName);
            data.Add(new GuardCase<T>(s.Name, s.Inputs, expected));
        }
        return data;
    }

    public static TheoryData<GuardCase<T>> ToGuardCases<T>(this RuleScenario<T>[] scenarios, Func<RuleScenario<T>, GuardExpected> expectedFactory)
    {
        var data = new TheoryData<GuardCase<T>>();
        foreach (var s in scenarios)
            data.Add(new GuardCase<T>(s.Name, s.Inputs, expectedFactory(s)));
        return data;
    }
}
