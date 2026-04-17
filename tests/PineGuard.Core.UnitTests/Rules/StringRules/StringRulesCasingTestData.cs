using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesCasingTestData
{
    public static class IsCaseStyle
    {
        public static TheoryData<RuleCase<(string? value, StringCasing style)>> Cases => F.IsCaseStyle.AllScenarios.ToRuleCases();
    }

    public static class IsCamelCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsCamelCase.AllScenarios.ToRuleCases();
    }

    public static class IsPascalCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsPascalCase.AllScenarios.ToRuleCases();
    }

    public static class IsSnakeCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsSnakeCase.AllScenarios.ToRuleCases();
    }

    public static class IsUpperSnakeCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsUpperSnakeCase.AllScenarios.ToRuleCases();
    }

    public static class IsKebabCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsKebabCase.AllScenarios.ToRuleCases();
    }

    public static class IsTrainCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsTrainCase.AllScenarios.ToRuleCases();
    }

    public static class IsDotCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsDotCase.AllScenarios.ToRuleCases();
    }

    public static class IsSpaceCase
    {
        public static TheoryData<RuleCase<string>> Cases => F.IsSpaceCase.AllScenarios.ToRuleCases();
    }

    public static class IsUpperInvariant
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsUpperInvariant.AllScenarios.ToRuleCases();
    }

    public static class IsLowerInvariant
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsLowerInvariant.AllScenarios.ToRuleCases();
    }
}
