using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class EnumRulesTestData
{
    public static class IsDefined
    {
        public static TheoryData<RuleCase<F.SimpleEnum?>> Cases => F.IsDefined.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedValue
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsDefinedValue.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedValueByteBacked
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsDefinedValueByteBacked.AllScenarios.ToRuleCases();
    }

    public static class IsDefinedName
    {
        public static TheoryData<RuleCase<(string? name, bool ignoreCase)>> Cases => F.IsDefinedName.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombination
    {
        public static TheoryData<RuleCase<F.FlagsEnum?>> Cases => F.IsFlagsEnumCombination.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombinationNonFlags
    {
        public static TheoryData<RuleCase<F.SimpleEnum?>> Cases => F.IsFlagsEnumCombinationNonFlags.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnumCombinationNegativeMember
    {
        public static TheoryData<RuleCase<F.SignedFlagsEnum?>> Cases => F.IsFlagsEnumCombinationNegativeMember.AllScenarios.ToRuleCases();
    }

    public static class HasFlag
    {
        public static TheoryData<RuleCase<(F.FlagsEnum? value, F.FlagsEnum flag)>> Cases => F.HasFlag.AllScenarios.ToRuleCases();
    }

    public static class HasDescription
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasDescription.AllScenarios.ToRuleCases();
    }

    public static class HasDisplay
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasDisplay.AllScenarios.ToRuleCases();
    }

    public static class HasEnumMember
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.HasEnumMember.AllScenarios.ToRuleCases();
    }

    public static class IsObsolete
    {
        public static TheoryData<RuleCase<F.AttributedEnum?>> Cases => F.IsObsolete.AllScenarios.ToRuleCases();
    }

    public static class IsFlagsEnum
    {
        public static TheoryData<bool> Cases => [true, false];
    }
}
