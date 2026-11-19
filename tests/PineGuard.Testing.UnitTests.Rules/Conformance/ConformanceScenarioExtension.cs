namespace PineGuard.Testing.UnitTests.Rules.Conformance;

public static class ConformanceScenarioExtension
{
    public static TheoryData<ConformanceCase<T>> ToConformanceCases<T>(this RuleScenario<T>[] scenarios, Func<RuleScenario<T>, ConformanceExpected> expectedFactory)
    {
        var data = new TheoryData<ConformanceCase<T>>();
        foreach (var s in scenarios)
            data.Add(new ConformanceCase<T>(s.Name, s.Inputs, expectedFactory(s)));
        return data;
    }
}
