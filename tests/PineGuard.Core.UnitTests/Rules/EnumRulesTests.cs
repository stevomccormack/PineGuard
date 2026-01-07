using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class EnumRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefined.ValidCases), MemberType = typeof(EnumRulesTestData.IsDefined))]
    public void IsDefined_ReturnsTrue_ForDefinedValue(EnumRulesTestData.IsDefined.Case testCase)
    {
        var result = EnumRules.IsDefined<EnumRulesTestData.SimpleEnum>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefined.EdgeCases), MemberType = typeof(EnumRulesTestData.IsDefined))]
    public void IsDefined_ReturnsFalse_ForNullOrUndefined(EnumRulesTestData.IsDefined.Case testCase)
    {
        var result = EnumRules.IsDefined<EnumRulesTestData.SimpleEnum>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedValue.ValidCases), MemberType = typeof(EnumRulesTestData.IsDefinedValue))]
    public void IsDefinedValue_ReturnsTrue_ForDefinedValue(EnumRulesTestData.IsDefinedValue.Case testCase)
    {
        var result = EnumRules.IsDefinedValue<EnumRulesTestData.SimpleEnum>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedValue.EdgeCases), MemberType = typeof(EnumRulesTestData.IsDefinedValue))]
    public void IsDefinedValue_ReturnsFalse_ForNullOrUndefinedValue(EnumRulesTestData.IsDefinedValue.Case testCase)
    {
        var result = EnumRules.IsDefinedValue<EnumRulesTestData.SimpleEnum>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedName.ValidCases), MemberType = typeof(EnumRulesTestData.IsDefinedName))]
    public void IsDefinedName_ReturnsTrue_WhenNameParsesToDefinedEnum(EnumRulesTestData.IsDefinedName.Case testCase)
    {
        var result = EnumRules.IsDefinedName<EnumRulesTestData.SimpleEnum>(testCase.Value, testCase.IgnoreCase);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsDefinedName.EdgeCases), MemberType = typeof(EnumRulesTestData.IsDefinedName))]
    public void IsDefinedName_ReturnsFalse_WhenNameDoesNotParseToDefinedEnum(EnumRulesTestData.IsDefinedName.Case testCase)
    {
        var result = EnumRules.IsDefinedName<EnumRulesTestData.SimpleEnum>(testCase.Value, testCase.IgnoreCase);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Fact]
    public void IsFlagsEnum_ReturnsExpected()
    {
        Assert.True(EnumRules.IsFlagsEnum<EnumRulesTestData.SampleFlags>());
        Assert.False(EnumRules.IsFlagsEnum<EnumRulesTestData.SimpleEnum>());
    }

    [Theory]
    [MemberData(
        nameof(EnumRulesTestData.IsFlagsEnumCombination.ValidCases),
        MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombination))]
    public void IsFlagsEnumCombination_ReturnsExpected_ForValidCombinations(EnumRulesTestData.IsFlagsEnumCombination.Case testCase)
    {
        var result = EnumRules.IsFlagsEnumCombination<EnumRulesTestData.SampleFlags>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(
        nameof(EnumRulesTestData.IsFlagsEnumCombination.EdgeCases),
        MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombination))]
    public void IsFlagsEnumCombination_ReturnsFalse_ForUndefinedBits(EnumRulesTestData.IsFlagsEnumCombination.Case testCase)
    {
        var result = EnumRules.IsFlagsEnumCombination<EnumRulesTestData.SampleFlags>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags.ValidCases), MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags))]
    [MemberData(nameof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags.EdgeCases), MemberType = typeof(EnumRulesTestData.IsFlagsEnumCombinationNonFlags))]
    public void IsFlagsEnumCombination_ForNonFlagsEnum_ReturnsExpected(
        EnumRulesTestData.IsFlagsEnumCombinationNonFlags.Case testCase)
    {
        var result = EnumRules.IsFlagsEnumCombination<EnumRulesTestData.SimpleEnum>(testCase.Value);

        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasFlag.ValidCases), MemberType = typeof(EnumRulesTestData.HasFlag))]
    public void HasFlag_ReturnsTrue_WhenFlagPresent(EnumRulesTestData.HasFlag.Case testCase)
    {
        var result = EnumRules.HasFlag(testCase.Value, testCase.Flag);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasFlag.EdgeCases), MemberType = typeof(EnumRulesTestData.HasFlag))]
    public void HasFlag_ReturnsFalse_WhenFlagMissingOrNull(EnumRulesTestData.HasFlag.Case testCase)
    {
        var result = EnumRules.HasFlag(testCase.Value, testCase.Flag);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasDescription.ValidCases), MemberType = typeof(EnumRulesTestData.HasDescription))]
    public void HasDescription_ReturnsTrue_WhenAttributePresent(EnumRulesTestData.HasDescription.Case testCase)
    {
        var result = EnumRules.HasDescription(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(
        nameof(EnumRulesTestData.HasDescription.EdgeCases),
        MemberType = typeof(EnumRulesTestData.HasDescription))]
    public void HasDescription_ReturnsFalse_WhenAttributeMissingOrValueInvalid(EnumRulesTestData.HasDescription.Case testCase)
    {
        var result = EnumRules.HasDescription(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasDisplay.ValidCases), MemberType = typeof(EnumRulesTestData.HasDisplay))]
    public void HasDisplay_ReturnsTrue_WhenAttributePresent(EnumRulesTestData.HasDisplay.Case testCase)
    {
        var result = EnumRules.HasDisplay(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasDisplay.EdgeCases), MemberType = typeof(EnumRulesTestData.HasDisplay))]
    public void HasDisplay_ReturnsFalse_WhenAttributeMissingOrValueInvalid(EnumRulesTestData.HasDisplay.Case testCase)
    {
        var result = EnumRules.HasDisplay(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasEnumMember.ValidCases), MemberType = typeof(EnumRulesTestData.HasEnumMember))]
    public void HasEnumMember_ReturnsTrue_WhenAttributePresent(EnumRulesTestData.HasEnumMember.Case testCase)
    {
        var result = EnumRules.HasEnumMember(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.HasEnumMember.EdgeCases), MemberType = typeof(EnumRulesTestData.HasEnumMember))]
    public void HasEnumMember_ReturnsFalse_WhenAttributeMissingOrValueInvalid(EnumRulesTestData.HasEnumMember.Case testCase)
    {
        var result = EnumRules.HasEnumMember(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsObsolete.ValidCases), MemberType = typeof(EnumRulesTestData.IsObsolete))]
    public void IsObsolete_ReturnsTrue_WhenAttributePresent(EnumRulesTestData.IsObsolete.Case testCase)
    {
        var result = EnumRules.IsObsolete(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(EnumRulesTestData.IsObsolete.EdgeCases), MemberType = typeof(EnumRulesTestData.IsObsolete))]
    public void IsObsolete_ReturnsFalse_WhenAttributeMissingOrValueInvalid(EnumRulesTestData.IsObsolete.Case testCase)
    {
        var result = EnumRules.IsObsolete(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }
}
