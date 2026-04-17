using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.BitWiseRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class BitWiseRulesTestData
{
    public static class IsBitwiseEqualToInt
    {
        public static TheoryData<RuleCase<(int? left, int? right, int mask)>> Cases => F.IsBitwiseEqualTo.AllScenarios.ToRuleCases();
    }

    public static class HasAllBitsInt
    {
        public static TheoryData<RuleCase<(int? value, int mask)>> Cases => F.HasAllBits.AllScenarios.ToRuleCases();
    }

    public static class HasAnyBitsInt
    {
        public static TheoryData<RuleCase<(int? value, int mask)>> Cases => F.HasAnyBits.AllScenarios.ToRuleCases();
    }

    public static class HasNoBitsInt
    {
        public static TheoryData<RuleCase<(int? value, int mask)>> Cases => F.HasNoBits.AllScenarios.ToRuleCases();
    }

    public static class HasOnlyBitsInt
    {
        public static TheoryData<RuleCase<(int? value, int allowedMask)>> Cases => F.HasOnlyBits.AllScenarios.ToRuleCases();
    }

    public static class IsPowerOfTwoInt
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsPowerOfTwo.AllScenarios.ToRuleCases();
    }
}
