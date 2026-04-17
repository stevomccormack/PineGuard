using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.ObjectRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class ObjectRulesTestData
{
    public static class IsEqualTo
    {
        public static TheoryData<RuleCase<(string? value, string? other)>> Cases => F.IsEqualTo.AllScenarios.ToRuleCases();
    }

    public static class IsOfType
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsOfType.AllScenarios.ToRuleCases();
    }

    public static class IsAssignableToType
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsAssignableToType.AllScenarios.ToRuleCases();
    }

    public static class IsSameReferenceAs
    {
        public static TheoryData<RuleCase<(object? a, object? b)>> Cases => F.IsSameReferenceAs.AllScenarios.ToRuleCases();
    }
}
