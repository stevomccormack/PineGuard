using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

#pragma warning disable CS0618
public sealed class EnumRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefined.Cases), MemberType = typeof(EnumRulesTestData.IsDefined))]
    public void IsDefined_BehavesAsExpected(RuleCase<F.SimpleEnum?> tc)
    {
        // Act
        var result = EnumRules.IsDefined(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedValue.Cases), MemberType = typeof(EnumRulesTestData.IsDefinedValue))]
    public void IsDefinedValue_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = EnumRules.IsDefinedValue<F.SimpleEnum>(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedValueByteBacked.Cases), MemberType = typeof(EnumRulesTestData.IsDefinedValueByteBacked))]
    public void IsDefinedValue_ByteBackedEnum_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = EnumRules.IsDefinedValue<F.ByteBackedEnum>(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedName.Cases), MemberType = typeof(EnumRulesTestData.IsDefinedName))]
    public void IsDefinedName_BehavesAsExpected(RuleCase<(string? name, bool ignoreCase)> tc)
    {
        // Act
        var result = EnumRules.IsDefinedName<F.SimpleEnum>(tc.Value.name, tc.Value.ignoreCase);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnum.Cases), MemberType = typeof(EnumRulesTestData.IsFlagsEnum))]
    public void IsFlagsEnum_BehavesAsExpected(bool expectedForFlags)
    {
        if (expectedForFlags)
            Assert.True(EnumRules.IsFlagsEnum<F.FlagsEnum>());
        else
            Assert.False(EnumRules.IsFlagsEnum<F.SimpleEnum>());
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnumCombination.Cases), MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombination))]
    public void IsFlagsEnumCombination_BehavesAsExpected(RuleCase<F.FlagsEnum?> tc)
    {
        // Act
        var result = EnumRules.IsFlagsEnumCombination(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags.Cases), MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags))]
    public void IsFlagsEnumCombinationNonFlags_BehavesAsExpected(RuleCase<F.SimpleEnum?> tc)
    {
        // Act
        var result = EnumRules.IsFlagsEnumCombination(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnumCombinationNegativeMember.Cases), MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombinationNegativeMember))]
    public void IsFlagsEnumCombinationNegativeMember_BehavesAsExpected(RuleCase<F.SignedFlagsEnum?> tc)
    {
        // Act
        var result = EnumRules.IsFlagsEnumCombination(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasFlag.Cases), MemberType = typeof(EnumRulesTestData.HasFlag))]
    public void HasFlag_BehavesAsExpected(RuleCase<(F.FlagsEnum? value, F.FlagsEnum flag)> tc)
    {
        // Act
        var result = EnumRules.HasFlag(tc.Value.value, tc.Value.flag);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasDescription.Cases), MemberType = typeof(EnumRulesTestData.HasDescription))]
    public void HasDescription_BehavesAsExpected(RuleCase<F.AttributedEnum?> tc)
    {
        // Act
        var result = EnumRules.HasDescription(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasDisplay.Cases), MemberType = typeof(EnumRulesTestData.HasDisplay))]
    public void HasDisplay_BehavesAsExpected(RuleCase<F.AttributedEnum?> tc)
    {
        // Act
        var result = EnumRules.HasDisplay(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasEnumMember.Cases), MemberType = typeof(EnumRulesTestData.HasEnumMember))]
    public void HasEnumMember_BehavesAsExpected(RuleCase<F.AttributedEnum?> tc)
    {
        // Act
        var result = EnumRules.HasEnumMember(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsObsolete.Cases), MemberType = typeof(EnumRulesTestData.IsObsolete))]
    public void IsObsolete_BehavesAsExpected(RuleCase<F.AttributedEnum?> tc)
    {
        // Act
        var result = EnumRules.IsObsolete(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
#pragma warning restore CS0618
