using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class EnumAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, EnumAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.Defined.ValidCases), MemberType = typeof(EnumAttributesTestData.Defined))]
    [MemberData(nameof(EnumAttributesTestData.Defined.EdgeCases), MemberType = typeof(EnumAttributesTestData.Defined))]
    [MemberData(nameof(EnumAttributesTestData.Defined.InvalidCases), MemberType = typeof(EnumAttributesTestData.Defined))]
    public void Defined_ShouldReturnExpected(EnumAttributesTestData.ValidCase testCase)
        => Verify(new DefinedAttribute(), testCase);

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.FlagsEnumCombination.ValidCases), MemberType = typeof(EnumAttributesTestData.FlagsEnumCombination))]
    [MemberData(nameof(EnumAttributesTestData.FlagsEnumCombination.EdgeCases), MemberType = typeof(EnumAttributesTestData.FlagsEnumCombination))]
    [MemberData(nameof(EnumAttributesTestData.FlagsEnumCombination.InvalidCases), MemberType = typeof(EnumAttributesTestData.FlagsEnumCombination))]
    public void FlagsEnumCombination_ShouldReturnExpected(EnumAttributesTestData.ValidCase testCase)
        => Verify(new FlagsEnumCombinationAttribute(), testCase);

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.HasFlag.ValidCases), MemberType = typeof(EnumAttributesTestData.HasFlag))]
    [MemberData(nameof(EnumAttributesTestData.HasFlag.EdgeCases), MemberType = typeof(EnumAttributesTestData.HasFlag))]
    [MemberData(nameof(EnumAttributesTestData.HasFlag.InvalidCases), MemberType = typeof(EnumAttributesTestData.HasFlag))]
    public void HasFlag_ShouldReturnExpected(EnumAttributesTestData.ValidCase testCase)
        => Verify(new HasFlagAttribute("A"), testCase);

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.NotHasFlag.ValidCases), MemberType = typeof(EnumAttributesTestData.NotHasFlag))]
    [MemberData(nameof(EnumAttributesTestData.NotHasFlag.EdgeCases), MemberType = typeof(EnumAttributesTestData.NotHasFlag))]
    [MemberData(nameof(EnumAttributesTestData.NotHasFlag.InvalidCases), MemberType = typeof(EnumAttributesTestData.NotHasFlag))]
    public void NotHasFlag_ShouldReturnExpected(EnumAttributesTestData.ValidCase testCase)
        => Verify(new NotHasFlagAttribute("A"), testCase);

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.DefinedNonEnum.Cases), MemberType = typeof(EnumAttributesTestData.DefinedNonEnum))]
    public void Defined_WithNonEnumType_ShouldThrow(IThrowsCase testCase)
    {
        var data = ((ThrowsCase<object>)testCase).Value;
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => new DefinedAttribute().GetValidationResult(data, new ValidationContext(new object())));
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.FlagsEnumCombinationNonEnum.Cases), MemberType = typeof(EnumAttributesTestData.FlagsEnumCombinationNonEnum))]
    public void FlagsEnumCombination_WithNonEnumType_ShouldThrow(IThrowsCase testCase)
    {
        var data = ((ThrowsCase<object>)testCase).Value;
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => new FlagsEnumCombinationAttribute().GetValidationResult(data, new ValidationContext(new object())));
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.HasFlagInvalidFlagName.Cases), MemberType = typeof(EnumAttributesTestData.HasFlagInvalidFlagName))]
    public void HasFlag_WithInvalidFlagName_ShouldThrow(IThrowsCase testCase)
    {
        var data = ((ThrowsCase<object>)testCase).Value;
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => new HasFlagAttribute("NonExistentFlag").GetValidationResult(data, new ValidationContext(new object())));
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.NotHasFlagInvalidFlagName.Cases), MemberType = typeof(EnumAttributesTestData.NotHasFlagInvalidFlagName))]
    public void NotHasFlag_WithInvalidFlagName_ShouldThrow(IThrowsCase testCase)
    {
        var data = ((ThrowsCase<object>)testCase).Value;
        var ex = Assert.Throws(
            testCase.ExpectedException.Type,
            () => new NotHasFlagAttribute("NonExistentFlag").GetValidationResult(data, new ValidationContext(new object())));
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(EnumAttributesTestData.DefinedWithErrorMessage.Cases), MemberType = typeof(EnumAttributesTestData.DefinedWithErrorMessage))]
    public void Defined_WithCustomErrorMessage_ShouldReturnCustomError(EnumAttributesTestData.ValidCase testCase)
    {
        var attribute = new DefinedAttribute { ErrorMessage = "Custom: {0} is invalid" };
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        if (testCase.Expected)
        {
            Assert.Equal(ValidationResult.Success, result);
        }
        else
        {
            Assert.NotNull(result);
            Assert.Contains("Custom:", result.ErrorMessage!);
        }
    }
}
