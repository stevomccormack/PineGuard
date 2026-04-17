using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TaskRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TaskRulesTestData
{
    public static class IsCompleted
    {
        public static TheoryData<RuleCase<Func<Task?>>> Cases => F.IsCompleted.AllScenarios.ToRuleCases();
    }

    public static class IsCanceled
    {
        public static TheoryData<RuleCase<Func<Task?>>> Cases => F.IsCanceled.AllScenarios.ToRuleCases();
    }

    public static class IsFaulted
    {
        public static TheoryData<RuleCase<Func<Task?>>> Cases => F.IsFaulted.AllScenarios.ToRuleCases();
    }
}
