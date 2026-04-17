using System.ComponentModel;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;
using F = PineGuard.Testing.Fixtures.EnumRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

#pragma warning disable CS0618
public sealed class MustEnumClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.Defined.ValidCases), MemberType = typeof(MustEnumClausesTestData.Defined))]
    [MemberData(nameof(MustEnumClausesTestData.Defined.InvalidCases), MemberType = typeof(MustEnumClausesTestData.Defined))]
    public void Defined_BehavesAsExpected(MustCase<F.SimpleEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.Defined(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotDefined.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotDefined))]
    [MemberData(nameof(MustEnumClausesTestData.NotDefined.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotDefined))]
    public void NotDefined_BehavesAsExpected(MustCase<F.SimpleEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotDefined(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.DefinedValue.ValidCases), MemberType = typeof(MustEnumClausesTestData.DefinedValue))]
    [MemberData(nameof(MustEnumClausesTestData.DefinedValue.InvalidCases), MemberType = typeof(MustEnumClausesTestData.DefinedValue))]
    public void DefinedValue_BehavesAsExpected(MustCase<int> tc)
    {
        var value = tc.Value;
        var result = Must.Be.DefinedValue<F.SimpleEnum>(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotDefinedValue.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotDefinedValue))]
    [MemberData(nameof(MustEnumClausesTestData.NotDefinedValue.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotDefinedValue))]
    public void NotDefinedValue_BehavesAsExpected(MustCase<int> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotDefinedValue<F.SimpleEnum>(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.DefinedName.ValidCases), MemberType = typeof(MustEnumClausesTestData.DefinedName))]
    [MemberData(nameof(MustEnumClausesTestData.DefinedName.InvalidCases), MemberType = typeof(MustEnumClausesTestData.DefinedName))]
    public void DefinedName_BehavesAsExpected(MustCase<(string? name, bool ignoreCase)> tc)
    {
        var name = tc.Value.name;
        var ignoreCase = tc.Value.ignoreCase;
        var result = Must.Be.DefinedName<F.SimpleEnum>(name, ignoreCase);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotDefinedName.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotDefinedName))]
    [MemberData(nameof(MustEnumClausesTestData.NotDefinedName.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotDefinedName))]
    public void NotDefinedName_BehavesAsExpected(MustCase<(string? name, bool ignoreCase)> tc)
    {
        var name = tc.Value.name;
        var ignoreCase = tc.Value.ignoreCase;
        var result = Must.Be.NotDefinedName<F.SimpleEnum>(name, ignoreCase);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.FlagsEnumCombination.ValidCases), MemberType = typeof(MustEnumClausesTestData.FlagsEnumCombination))]
    [MemberData(nameof(MustEnumClausesTestData.FlagsEnumCombination.InvalidCases), MemberType = typeof(MustEnumClausesTestData.FlagsEnumCombination))]
    public void FlagsEnumCombination_BehavesAsExpected(MustCase<F.FlagsEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.FlagsEnumCombination(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotFlagsEnumCombination.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotFlagsEnumCombination))]
    [MemberData(nameof(MustEnumClausesTestData.NotFlagsEnumCombination.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotFlagsEnumCombination))]
    public void NotFlagsEnumCombination_BehavesAsExpected(MustCase<F.FlagsEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotFlagsEnumCombination(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.HasAttribute.ValidCases), MemberType = typeof(MustEnumClausesTestData.HasAttribute))]
    [MemberData(nameof(MustEnumClausesTestData.HasAttribute.InvalidCases), MemberType = typeof(MustEnumClausesTestData.HasAttribute))]
    public void HasAttribute_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.HasAttribute<F.AttributedEnum, DescriptionAttribute>(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotHasAttribute.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotHasAttribute))]
    [MemberData(nameof(MustEnumClausesTestData.NotHasAttribute.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotHasAttribute))]
    public void NotHasAttribute_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotHasAttribute<F.AttributedEnum, DescriptionAttribute>(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.HasFlag.ValidCases), MemberType = typeof(MustEnumClausesTestData.HasFlag))]
    [MemberData(nameof(MustEnumClausesTestData.HasFlag.InvalidCases), MemberType = typeof(MustEnumClausesTestData.HasFlag))]
    public void HasFlag_BehavesAsExpected(MustCase<(F.FlagsEnum value, F.FlagsEnum flag)> tc)
    {
        var value = tc.Value.value;
        var flag = tc.Value.flag;
        var result = Must.Be.HasFlag(value, flag);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotHasFlag.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotHasFlag))]
    [MemberData(nameof(MustEnumClausesTestData.NotHasFlag.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotHasFlag))]
    public void NotHasFlag_BehavesAsExpected(MustCase<(F.FlagsEnum value, F.FlagsEnum flag)> tc)
    {
        var value = tc.Value.value;
        var flag = tc.Value.flag;
        var result = Must.Be.NotHasFlag(value, flag);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.HasDescription.ValidCases), MemberType = typeof(MustEnumClausesTestData.HasDescription))]
    [MemberData(nameof(MustEnumClausesTestData.HasDescription.InvalidCases), MemberType = typeof(MustEnumClausesTestData.HasDescription))]
    public void HasDescription_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.HasDescription(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotHasDescription.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotHasDescription))]
    [MemberData(nameof(MustEnumClausesTestData.NotHasDescription.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotHasDescription))]
    public void NotHasDescription_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotHasDescription(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.HasDisplay.ValidCases), MemberType = typeof(MustEnumClausesTestData.HasDisplay))]
    [MemberData(nameof(MustEnumClausesTestData.HasDisplay.InvalidCases), MemberType = typeof(MustEnumClausesTestData.HasDisplay))]
    public void HasDisplay_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.HasDisplay(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotHasDisplay.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotHasDisplay))]
    [MemberData(nameof(MustEnumClausesTestData.NotHasDisplay.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotHasDisplay))]
    public void NotHasDisplay_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotHasDisplay(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.HasEnumMember.ValidCases), MemberType = typeof(MustEnumClausesTestData.HasEnumMember))]
    [MemberData(nameof(MustEnumClausesTestData.HasEnumMember.InvalidCases), MemberType = typeof(MustEnumClausesTestData.HasEnumMember))]
    public void HasEnumMember_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.HasEnumMember(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotHasEnumMember.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotHasEnumMember))]
    [MemberData(nameof(MustEnumClausesTestData.NotHasEnumMember.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotHasEnumMember))]
    public void NotHasEnumMember_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotHasEnumMember(value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.Obsolete.ValidCases), MemberType = typeof(MustEnumClausesTestData.Obsolete))]
    [MemberData(nameof(MustEnumClausesTestData.Obsolete.InvalidCases), MemberType = typeof(MustEnumClausesTestData.Obsolete))]
#pragma warning disable CS0618
    public void Obsolete_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.Obsolete(value);
        AssertResult(tc, result);
    }
#pragma warning restore CS0618

    [Theory]
    [MemberData(nameof(MustEnumClausesTestData.NotObsolete.ValidCases), MemberType = typeof(MustEnumClausesTestData.NotObsolete))]
    [MemberData(nameof(MustEnumClausesTestData.NotObsolete.InvalidCases), MemberType = typeof(MustEnumClausesTestData.NotObsolete))]
#pragma warning disable CS0618
    public void NotObsolete_BehavesAsExpected(MustCase<F.AttributedEnum> tc)
    {
        var value = tc.Value;
        var result = Must.Be.NotObsolete(value);
        AssertResult(tc, result);
    }
#pragma warning restore CS0618
}
#pragma warning restore CS0618
