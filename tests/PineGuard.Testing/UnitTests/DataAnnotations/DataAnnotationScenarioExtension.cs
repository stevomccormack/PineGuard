using PineGuard.Testing.UnitTests.Rules;
using Xunit;

namespace PineGuard.Testing.UnitTests.DataAnnotations;

public static class DataAnnotationScenarioExtension
{
    public static TheoryData<DataAnnotationCase> ToDataAnnotationCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<DataAnnotationCase>();
        foreach (var s in scenarios)
            data.Add(new DataAnnotationCase(s.Name, s.Inputs, new DataAnnotationExpected(s.IsValid)));
        return data;
    }

    public static TheoryData<DataAnnotationCase> ToDataAnnotationCases<T>(this RuleScenario<T>[] scenarios, Func<RuleScenario<T>, DataAnnotationExpected> expectedFactory)
    {
        var data = new TheoryData<DataAnnotationCase>();
        foreach (var s in scenarios)
            data.Add(new DataAnnotationCase(s.Name, s.Inputs, expectedFactory(s)));
        return data;
    }

    public static TheoryData<DataAnnotationCase> ToDataAnnotationCases<T>(this RuleScenario<T>[] scenarios, Func<T, object?> valueExtractor)
    {
        var data = new TheoryData<DataAnnotationCase>();
        foreach (var s in scenarios)
            data.Add(new DataAnnotationCase(s.Name, valueExtractor(s.Inputs), new DataAnnotationExpected(s.IsValid)));
        return data;
    }

    public static TheoryData<DataAnnotationCase> ToDataAnnotationCases<T>(this RuleScenario<T>[] scenarios, Func<T, object?> valueExtractor, Func<RuleScenario<T>, DataAnnotationExpected> expectedFactory)
    {
        var data = new TheoryData<DataAnnotationCase>();
        foreach (var s in scenarios)
            data.Add(new DataAnnotationCase(s.Name, valueExtractor(s.Inputs), expectedFactory(s)));
        return data;
    }
}
