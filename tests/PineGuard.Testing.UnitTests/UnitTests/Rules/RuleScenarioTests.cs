using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public sealed class RuleScenarioTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(RuleScenarioTestData.Construction.ValidCases), MemberType = typeof(RuleScenarioTestData.Construction))]
    [MemberData(nameof(RuleScenarioTestData.Construction.EdgeCases), MemberType = typeof(RuleScenarioTestData.Construction))]
    public void Construction_SetsProperties(RuleScenarioTestData.Construction.Case testCase)
    {
        var (scenarioName, inputs, isValid) = testCase.Value;

        var scenario = new RuleScenario<string?>(scenarioName, inputs, isValid);

        Assert.Equal(scenarioName, scenario.Name);
        Assert.Equal(inputs, scenario.Inputs);
        Assert.Equal(isValid, scenario.IsValid);
    }

    [Theory]
    [MemberData(nameof(RuleScenarioTestData.IsNull.ValidCases), MemberType = typeof(RuleScenarioTestData.IsNull))]
    [MemberData(nameof(RuleScenarioTestData.IsNull.EdgeCases), MemberType = typeof(RuleScenarioTestData.IsNull))]
    public void IsNull_ReflectsInputsNullability(RuleScenarioTestData.IsNull.Case testCase)
    {
        var (inputs, expectedIsNull) = testCase.Value;

        var scenario = new RuleScenario<string?>("test", inputs, false);

        Assert.Equal(expectedIsNull, scenario.IsNull);
    }
}
