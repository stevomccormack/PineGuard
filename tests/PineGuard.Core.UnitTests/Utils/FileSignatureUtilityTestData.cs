using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FileSignatureRulesFixtures;

namespace PineGuard.Core.UnitTests.Utils;

public static class FileSignatureUtilityTestData
{
    public static class TryDetectExtension
    {
        public static TheoryData<RuleCase<(byte[]? header, string? extension)>> Cases => F.TryDetectExtension.AllScenarios.ToRuleCases();
    }

    public static class IsKnownExtension
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsKnownExtension.AllScenarios.ToRuleCases();
    }
}
