using PineGuard.Testing.UnitTests.Rules;
using Xunit;

namespace PineGuard.Testing.UnitTests.MustClauses;

public static class MustValidationScenarioExtension
{
    public static TheoryData<MustValidationCase<T>> ToMustValidationCases<T>(this RuleScenario<T>[] scenarios)
    {
        var data = new TheoryData<MustValidationCase<T>>();
        foreach (var s in scenarios)
            data.Add(new MustValidationCase<T>(s.Name, s.Inputs, new MustValidationExpected(s.IsValid)));
        return data;
    }
}
