using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public sealed class RuleScenarioExtensionsTests : BaseUnitTest
{
    private static T FirstCase<T>(TheoryData<T> data) where T : class
    {
        foreach (var row in (IEnumerable<object[]>)data)
            return (T)row[0];
        throw new InvalidOperationException("TheoryData is empty.");
    }

    private static List<T> AllCases<T>(TheoryData<T> data) where T : class
    {
        return (from row in (IEnumerable<object[]>)data select (T)row[0]).ToList();
    }

    // Filter combinators

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.WhereValidOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.WhereValidOps))]
    public void WhereValid_FiltersToValidOnly(RuleScenarioExtensionsTestData.WhereValidOps.Case testCase)
    {
        var (input, expectedCount) = testCase.Value;

        var result = input.WhereValid();

        Assert.Equal(expectedCount, result.Length);
        Assert.All(result, s => Assert.True(s.IsValid));
    }

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.WhereInvalidOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.WhereInvalidOps))]
    public void WhereInvalid_FiltersToInvalidOnly(RuleScenarioExtensionsTestData.WhereInvalidOps.Case testCase)
    {
        var (input, expectedCount) = testCase.Value;

        var result = input.WhereInvalid();

        Assert.Equal(expectedCount, result.Length);
        Assert.All(result, s => Assert.False(s.IsValid));
    }

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ExceptOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.ExceptOps))]
    public void Except_ExcludesByName(RuleScenarioExtensionsTestData.ExceptOps.Case testCase)
    {
        var (input, excludeNames, expectedCount) = testCase.Value;

        var result = input.Except(excludeNames);

        Assert.Equal(expectedCount, result.Length);
        Assert.All(result, s => Assert.DoesNotContain(s.Name, excludeNames));
    }

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.OnlyOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.OnlyOps))]
    public void Only_IncludesByName(RuleScenarioExtensionsTestData.OnlyOps.Case testCase)
    {
        var (input, includeNames, expectedCount) = testCase.Value;

        var result = input.Only(includeNames);

        Assert.Equal(expectedCount, result.Length);
        Assert.All(result, s => Assert.Contains(s.Name, includeNames));
    }

    // ToRuleCases

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ToRuleCasesOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.ToRuleCasesOps))]
    public void ToRuleCases_ConvertsScenarios(RuleScenarioExtensionsTestData.ToRuleCasesOps.Case testCase)
    {
        var (input, expectedCount) = testCase.Value;

        var result = input.ToRuleCases();

        Assert.Equal(expectedCount, result.Count);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToRuleCases_MapsIsValidCorrectly(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var ruleCase = FirstCase(scenarios.ToRuleCases());

        Assert.Equal(isValid, ruleCase.Expected.IsValid);
        Assert.Equal("test", ruleCase.Name);
        Assert.Equal("x", ruleCase.Value);
    }

    // ToMustCases (auto)

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ToMustCasesAutoOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.ToMustCasesAutoOps))]
    public void ToMustCases_Auto_ConvertsScenarios(RuleScenarioExtensionsTestData.ToMustCasesAutoOps.Case testCase)
    {
        var (input, expectedCount) = testCase.Value;

        var result = input.ToMustCases();

        Assert.Equal(expectedCount, result.Count);
    }

    // ToMustCases (custom factory)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToMustCases_CustomFactory_UsesFactory(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var mustCase = FirstCase(scenarios.ToMustCases(s => new MustExpected(s.IsValid, "custom", "param")));

        Assert.Equal(isValid, mustCase.Expected.IsValid);
        Assert.Equal("custom", mustCase.Expected.Message);
        Assert.Equal("param", mustCase.Expected.ParamName);
    }

    // ToMustValidationCases

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ToMustValidationCasesOps.Cases), MemberType = typeof(RuleScenarioExtensionsTestData.ToMustValidationCasesOps))]
    public void ToMustValidationCases_MapsIsValidCorrectly(RuleScenarioExtensionsTestData.ToMustValidationCasesOps.Case testCase)
    {
        var (input, expectedIsValid) = testCase.Value;

        var validationCase = FirstCase(input.ToMustValidationCases());

        Assert.Equal(expectedIsValid, validationCase.Expected.IsValid);
        Assert.Equal("test", validationCase.Name);
        Assert.Equal("x", validationCase.Value);
    }

    // Project

    [Fact]
    public void Project_TransformsInputs()
    {
        RuleScenario<(string?, int)>[] scenarios = [new("test", ("hello", 5), true)];

        var projected = scenarios.Project(t => t.Item1);

        Assert.Single(projected);
        Assert.Equal("test", projected[0].Name);
        Assert.Equal("hello", projected[0].Inputs);
        Assert.True(projected[0].IsValid);
    }

    [Fact]
    public void Project_ThenToMustCases_ExtractsProjectedValue()
    {
        RuleScenario<(string?, int)>[] scenarios = [new("test", ("hello", 5), true)];

        var mustCase = FirstCase(scenarios.Project(t => t.Item1).ToMustCases());

        Assert.Equal("hello", mustCase.Value);
        Assert.True(mustCase.Expected.IsValid);
    }

    [Fact]
    public void Project_ThenToGuardCases_ExtractsProjectedValue()
    {
        RuleScenario<(string?, int)>[] scenarios = [new("test", ("hello", 5), true)];

        var guardCase = FirstCase(scenarios.Project(t => t.Item1).ToGuardCases());

        Assert.Equal("hello", guardCase.Value);
        Assert.True(guardCase.Expected.IsValid);
    }

    // ToGuardCases (auto)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToGuardCases_Auto_MapsIsValid(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var guardCase = FirstCase(scenarios.ToGuardCases());

        Assert.Equal(isValid, guardCase.Expected.IsValid);
    }

    // ToGuardCases (paramName)

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ToGuardCasesParamNameOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.ToGuardCasesParamNameOps))]
    public void ToGuardCases_ParamName_AutoMapsExceptionTypes(RuleScenarioExtensionsTestData.ToGuardCasesParamNameOps.Case testCase)
    {
        var (input, paramName) = testCase.Value;

        var cases = AllCases(input.ToGuardCases(paramName));

        // valid-a and valid-b → IsValid = true
        var validCases = cases.Where(c => c.Expected.IsValid).ToList();
        Assert.Equal(2, validCases.Count);

        // null-input → ArgumentNullException
        var nullCase = cases.Single(c => c.Name == "null-input");
        Assert.False(nullCase.Expected.IsValid);
        Assert.Equal(typeof(ArgumentNullException), nullCase.Expected.ExceptionType);
        Assert.Equal(paramName, nullCase.Expected.ParamName);

        // invalid-a → ArgumentException
        var invalidCase = cases.Single(c => c.Name == "invalid-a");
        Assert.False(invalidCase.Expected.IsValid);
        Assert.Equal(typeof(ArgumentException), invalidCase.Expected.ExceptionType);
        Assert.Equal(paramName, invalidCase.Expected.ParamName);
    }

    // ToGuardCases (custom factory)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToGuardCases_CustomFactory_UsesFactory(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var guardCase = FirstCase(scenarios.ToGuardCases(s => new GuardExpected(s.IsValid, typeof(ArgumentException), "p", "msg")));

        Assert.Equal(isValid, guardCase.Expected.IsValid);
        Assert.Equal("p", guardCase.Expected.ParamName);
    }

    // ToFluentCases (auto)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToFluentCases_Auto_MapsIsValid(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var fluentCase = FirstCase(scenarios.ToFluentCases());

        Assert.Equal(isValid, fluentCase.Expected.IsValid);
    }

    // ToFluentCases (custom factory)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToFluentCases_CustomFactory_UsesFactory(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var fluentCase = FirstCase(scenarios.ToFluentCases(s => new FluentExpected(s.IsValid, "msg", "Prop")));

        Assert.Equal("msg", fluentCase.Expected.Message);
        Assert.Equal("Prop", fluentCase.Expected.PropertyName);
    }

    // ToDataAnnotationCases (auto)

    [Theory]
    [MemberData(nameof(RuleScenarioExtensionsTestData.ToDataAnnotationCasesAutoOps.ValidCases), MemberType = typeof(RuleScenarioExtensionsTestData.ToDataAnnotationCasesAutoOps))]
    public void ToDataAnnotationCases_Auto_ConvertsScenarios(RuleScenarioExtensionsTestData.ToDataAnnotationCasesAutoOps.Case testCase)
    {
        var (input, expectedCount) = testCase.Value;

        var result = input.ToDataAnnotationCases();

        Assert.Equal(expectedCount, result.Count);
    }

    // ToDataAnnotationCases (custom factory)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToDataAnnotationCases_CustomFactory_UsesFactory(bool isValid)
    {
        RuleScenario<string?>[] scenarios = [new("test", "x", isValid)];

        var daCase = FirstCase(scenarios.ToDataAnnotationCases(s => new DataAnnotationExpected(s.IsValid, "msg", "Member")));

        Assert.Equal("msg", daCase.Expected.Message);
        Assert.Equal("Member", daCase.Expected.MemberName);
    }

    // ToDataAnnotationCases (value extractor)

    [Fact]
    public void ToDataAnnotationCases_ValueExtractor_ExtractsValue()
    {
        RuleScenario<(string?, int)>[] scenarios = [new("test", ("hello", 5), true)];

        var daCase = FirstCase(scenarios.ToDataAnnotationCases(t => t.Item1));

        Assert.Equal("hello", daCase.Value);
    }

    // ToDataAnnotationCases (value extractor + custom factory)

    [Fact]
    public void ToDataAnnotationCases_ExtractorAndFactory_BothApplied()
    {
        RuleScenario<(string?, int)>[] scenarios = [new("test", ("hello", 5), true)];

        var daCase = FirstCase(scenarios.ToDataAnnotationCases(t => t.Item1, s => new DataAnnotationExpected(s.IsValid, "custom")));

        Assert.Equal("hello", daCase.Value);
        Assert.Equal("custom", daCase.Expected.Message);
    }
}
