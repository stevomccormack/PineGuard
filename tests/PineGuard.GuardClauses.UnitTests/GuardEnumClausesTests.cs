using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

#pragma warning disable CS0618
public sealed class GuardEnumClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefined.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefined))]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefined.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefined))]
    public void NotDefined_BehavesAsExpected(GuardCase<F.SimpleEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDefined(value));
        AssertCustomMessage(tc, () => Guard.Against.NotDefined(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.Defined.ValidCases), MemberType = typeof(GuardEnumClausesTestData.Defined))]
    [MemberData(nameof(GuardEnumClausesTestData.Defined.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.Defined))]
    public void Defined_BehavesAsExpected(GuardCase<F.SimpleEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Defined(value));
        AssertCustomMessage(tc, () => Guard.Against.Defined(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefinedValue.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefinedValue))]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefinedValue.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefinedValue))]
    public void NotDefinedValue_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDefinedValue<F.SimpleEnum>(value));
        AssertCustomMessage(tc, () => Guard.Against.NotDefinedValue<F.SimpleEnum>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.DefinedValue.ValidCases), MemberType = typeof(GuardEnumClausesTestData.DefinedValue))]
    [MemberData(nameof(GuardEnumClausesTestData.DefinedValue.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.DefinedValue))]
    public void DefinedValue_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.DefinedValue<F.SimpleEnum>(value));
        AssertCustomMessage(tc, () => Guard.Against.DefinedValue<F.SimpleEnum>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefinedName.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefinedName))]
    [MemberData(nameof(GuardEnumClausesTestData.NotDefinedName.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotDefinedName))]
    public void NotDefinedName_BehavesAsExpected(GuardCase<(string? name, bool ignoreCase)> tc)
    {
        var name = tc.Value.name;
        var ignoreCase = tc.Value.ignoreCase;
        var result = AssertResult(tc, () => Guard.Against.NotDefinedName<F.SimpleEnum>(name, ignoreCase));
        AssertCustomMessage(tc, () => Guard.Against.NotDefinedName<F.SimpleEnum>(name, ignoreCase, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(name, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.DefinedName.ValidCases), MemberType = typeof(GuardEnumClausesTestData.DefinedName))]
    [MemberData(nameof(GuardEnumClausesTestData.DefinedName.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.DefinedName))]
    public void DefinedName_BehavesAsExpected(GuardCase<(string? name, bool ignoreCase)> tc)
    {
        var name = tc.Value.name;
        var ignoreCase = tc.Value.ignoreCase;
        var result = AssertResult(tc, () => Guard.Against.DefinedName<F.SimpleEnum>(name, ignoreCase));
        AssertCustomMessage(tc, () => Guard.Against.DefinedName<F.SimpleEnum>(name, ignoreCase, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(name, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotFlagsEnumCombination.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotFlagsEnumCombination))]
    [MemberData(nameof(GuardEnumClausesTestData.NotFlagsEnumCombination.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotFlagsEnumCombination))]
    public void NotFlagsEnumCombination_BehavesAsExpected(GuardCase<F.FlagsEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotFlagsEnumCombination(value));
        AssertCustomMessage(tc, () => Guard.Against.NotFlagsEnumCombination(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.FlagsEnumCombination.ValidCases), MemberType = typeof(GuardEnumClausesTestData.FlagsEnumCombination))]
    [MemberData(nameof(GuardEnumClausesTestData.FlagsEnumCombination.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.FlagsEnumCombination))]
    public void FlagsEnumCombination_BehavesAsExpected(GuardCase<F.FlagsEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.FlagsEnumCombination(value));
        AssertCustomMessage(tc, () => Guard.Against.FlagsEnumCombination(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasAttribute.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasAttribute))]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasAttribute.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasAttribute))]
    public void NotHasAttribute_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasAttribute<F.AttributedEnum, System.ComponentModel.DescriptionAttribute>(value));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAttribute<F.AttributedEnum, System.ComponentModel.DescriptionAttribute>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.HasAttribute.ValidCases), MemberType = typeof(GuardEnumClausesTestData.HasAttribute))]
    [MemberData(nameof(GuardEnumClausesTestData.HasAttribute.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.HasAttribute))]
    public void HasAttribute_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasAttribute<F.AttributedEnum, System.ComponentModel.DescriptionAttribute>(value));
        AssertCustomMessage(tc, () => Guard.Against.HasAttribute<F.AttributedEnum, System.ComponentModel.DescriptionAttribute>(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasFlag.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasFlag))]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasFlag.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasFlag))]
    public void NotHasFlag_BehavesAsExpected(GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)> tc)
    {
        var value = tc.Value.value;
        var flag = tc.Value.flag;
        var result = AssertResult(tc, () => Guard.Against.NotHasFlag(value, flag));
        AssertCustomMessage(tc, () => Guard.Against.NotHasFlag(value, flag, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.HasFlag.ValidCases), MemberType = typeof(GuardEnumClausesTestData.HasFlag))]
    [MemberData(nameof(GuardEnumClausesTestData.HasFlag.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.HasFlag))]
    public void HasFlag_BehavesAsExpected(GuardCase<(F.FlagsEnum value, F.FlagsEnum flag)> tc)
    {
        var value = tc.Value.value;
        var flag = tc.Value.flag;
        var result = AssertResult(tc, () => Guard.Against.HasFlag(value, flag));
        AssertCustomMessage(tc, () => Guard.Against.HasFlag(value, flag, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasDescription.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasDescription))]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasDescription.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasDescription))]
    public void NotHasDescription_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasDescription(value));
        AssertCustomMessage(tc, () => Guard.Against.NotHasDescription(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.HasDescription.ValidCases), MemberType = typeof(GuardEnumClausesTestData.HasDescription))]
    [MemberData(nameof(GuardEnumClausesTestData.HasDescription.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.HasDescription))]
    public void HasDescription_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasDescription(value));
        AssertCustomMessage(tc, () => Guard.Against.HasDescription(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasDisplay.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasDisplay))]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasDisplay.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasDisplay))]
    public void NotHasDisplay_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasDisplay(value));
        AssertCustomMessage(tc, () => Guard.Against.NotHasDisplay(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.HasDisplay.ValidCases), MemberType = typeof(GuardEnumClausesTestData.HasDisplay))]
    [MemberData(nameof(GuardEnumClausesTestData.HasDisplay.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.HasDisplay))]
    public void HasDisplay_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasDisplay(value));
        AssertCustomMessage(tc, () => Guard.Against.HasDisplay(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasEnumMember.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasEnumMember))]
    [MemberData(nameof(GuardEnumClausesTestData.NotHasEnumMember.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotHasEnumMember))]
    public void NotHasEnumMember_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasEnumMember(value));
        AssertCustomMessage(tc, () => Guard.Against.NotHasEnumMember(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.HasEnumMember.ValidCases), MemberType = typeof(GuardEnumClausesTestData.HasEnumMember))]
    [MemberData(nameof(GuardEnumClausesTestData.HasEnumMember.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.HasEnumMember))]
    public void HasEnumMember_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasEnumMember(value));
        AssertCustomMessage(tc, () => Guard.Against.HasEnumMember(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.Obsolete.ValidCases), MemberType = typeof(GuardEnumClausesTestData.Obsolete))]
    [MemberData(nameof(GuardEnumClausesTestData.Obsolete.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.Obsolete))]
#pragma warning disable CS0618
    public void Obsolete_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Obsolete(value));
        AssertCustomMessage(tc, () => Guard.Against.Obsolete(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
#pragma warning restore CS0618

    [Theory]
    [MemberData(nameof(GuardEnumClausesTestData.NotObsolete.ValidCases), MemberType = typeof(GuardEnumClausesTestData.NotObsolete))]
    [MemberData(nameof(GuardEnumClausesTestData.NotObsolete.InvalidCases), MemberType = typeof(GuardEnumClausesTestData.NotObsolete))]
#pragma warning disable CS0618
    public void NotObsolete_BehavesAsExpected(GuardCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotObsolete(value));
        AssertCustomMessage(tc, () => Guard.Against.NotObsolete(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
#pragma warning restore CS0618
}
#pragma warning restore CS0618
