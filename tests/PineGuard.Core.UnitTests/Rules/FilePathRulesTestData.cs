using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FilePathRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class FilePathRulesTestData
{
    public static class IsSafeFileName
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsSafeFileName.AllScenarios.ToRuleCases();
    }

    public static class HasFileExtension
    {
        public static TheoryData<RuleCase<(string? path, string[]? allowed)>> Cases => F.HasFileExtension.AllScenarios.ToRuleCases();
    }
}
