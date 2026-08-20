using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class ObjectAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, ObjectAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.Null.ValidCases), MemberType = typeof(ObjectAttributesTestData.Null))]
    [MemberData(nameof(ObjectAttributesTestData.Null.EdgeCases), MemberType = typeof(ObjectAttributesTestData.Null))]
    [MemberData(nameof(ObjectAttributesTestData.Null.InvalidCases), MemberType = typeof(ObjectAttributesTestData.Null))]
    public void Null_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new NullAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.NotNull.ValidCases), MemberType = typeof(ObjectAttributesTestData.NotNull))]
    [MemberData(nameof(ObjectAttributesTestData.NotNull.EdgeCases), MemberType = typeof(ObjectAttributesTestData.NotNull))]
    [MemberData(nameof(ObjectAttributesTestData.NotNull.InvalidCases), MemberType = typeof(ObjectAttributesTestData.NotNull))]
    public void NotNull_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new NotNullAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.IsDefault.ValidCases), MemberType = typeof(ObjectAttributesTestData.IsDefault))]
    [MemberData(nameof(ObjectAttributesTestData.IsDefault.EdgeCases), MemberType = typeof(ObjectAttributesTestData.IsDefault))]
    [MemberData(nameof(ObjectAttributesTestData.IsDefault.InvalidCases), MemberType = typeof(ObjectAttributesTestData.IsDefault))]
    public void IsDefault_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new IsDefaultAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.NotDefault.ValidCases), MemberType = typeof(ObjectAttributesTestData.NotDefault))]
    [MemberData(nameof(ObjectAttributesTestData.NotDefault.EdgeCases), MemberType = typeof(ObjectAttributesTestData.NotDefault))]
    [MemberData(nameof(ObjectAttributesTestData.NotDefault.InvalidCases), MemberType = typeof(ObjectAttributesTestData.NotDefault))]
    public void NotDefault_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new NotDefaultAttribute(), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.EqualTo.ValidCases), MemberType = typeof(ObjectAttributesTestData.EqualTo))]
    [MemberData(nameof(ObjectAttributesTestData.EqualTo.EdgeCases), MemberType = typeof(ObjectAttributesTestData.EqualTo))]
    [MemberData(nameof(ObjectAttributesTestData.EqualTo.InvalidCases), MemberType = typeof(ObjectAttributesTestData.EqualTo))]
    public void EqualTo_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new EqualToAttribute("abc"), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.NotEqualTo.ValidCases), MemberType = typeof(ObjectAttributesTestData.NotEqualTo))]
    [MemberData(nameof(ObjectAttributesTestData.NotEqualTo.EdgeCases), MemberType = typeof(ObjectAttributesTestData.NotEqualTo))]
    [MemberData(nameof(ObjectAttributesTestData.NotEqualTo.InvalidCases), MemberType = typeof(ObjectAttributesTestData.NotEqualTo))]
    public void NotEqualTo_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new NotEqualToAttribute("abc"), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.OfType.ValidCases), MemberType = typeof(ObjectAttributesTestData.OfType))]
    [MemberData(nameof(ObjectAttributesTestData.OfType.EdgeCases), MemberType = typeof(ObjectAttributesTestData.OfType))]
    [MemberData(nameof(ObjectAttributesTestData.OfType.InvalidCases), MemberType = typeof(ObjectAttributesTestData.OfType))]
    public void OfType_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new OfTypeAttribute(typeof(string)), testCase);

    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.NotOfType.ValidCases), MemberType = typeof(ObjectAttributesTestData.NotOfType))]
    [MemberData(nameof(ObjectAttributesTestData.NotOfType.EdgeCases), MemberType = typeof(ObjectAttributesTestData.NotOfType))]
    [MemberData(nameof(ObjectAttributesTestData.NotOfType.InvalidCases), MemberType = typeof(ObjectAttributesTestData.NotOfType))]
    public void NotOfType_ShouldReturnExpected(ObjectAttributesTestData.ValidCase testCase)
        => Verify(new NotOfTypeAttribute(typeof(string)), testCase);

    // Covers CheckArgCompatibility: null arg in args array.
    [Theory]
    [InlineData(false)]
    public void EqualTo_WithNullComparisonValue_ShouldReturnExpected(bool expected)
    {
        var result = new EqualToAttribute(null!).GetValidationResult("hello", new ValidationContext(new object()));
        Assert.Equal(expected, result == ValidationResult.Success);
    }

    // Covers ValidationAttributeBase.BuildInvokeArgs: null value inferred to a non-nullable value-type parameter.
    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.EqualToNullValueType.InvalidCases), MemberType = typeof(ObjectAttributesTestData.EqualToNullValueType))]
    public void EqualTo_WithNullValueAndValueTypeComparison_ThrowsExpected(IThrowsCase testCase)
    {
        var action = ((ThrowsCase<Action>)testCase).Value;
        var ex = Assert.Throws(testCase.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    // Covers CheckArgCompatibility: MemberName set on the ValidationContext must appear in MemberNames.
    [Theory]
    [MemberData(nameof(ObjectAttributesTestData.EqualToWithMemberName.Cases), MemberType = typeof(ObjectAttributesTestData.EqualToWithMemberName))]
    public void EqualTo_WithTypeMismatchAndMemberName_ShouldReportMemberNames(ObjectAttributesTestData.ValidCase testCase)
    {
        var ctx = new ValidationContext(new object()) { MemberName = "SomeMember" };
        var result = new EqualToAttribute("abc").GetValidationResult(testCase.Value, ctx);
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("SomeMember", result!.MemberNames);
    }

    // Covers InferValueType: MemberName set + property found on model.
    [Theory]
    [InlineData(true)]
    public void IsDefault_WithNullAndMemberName_PropertyFound_ShouldReturnExpected(bool expected)
    {
        var model = new TestModel();
        var ctx = new ValidationContext(model) { MemberName = nameof(TestModel.Name) };
        var result = new IsDefaultAttribute().GetValidationResult(null, ctx);
        Assert.Equal(expected, result == ValidationResult.Success);
    }

    // Covers InferValueType: MemberName set + property NOT found on model.
    [Theory]
    [InlineData(true)]
    public void IsDefault_WithNullAndMemberName_PropertyNotFound_ShouldReturnExpected(bool expected)
    {
        var model = new TestModel();
        var ctx = new ValidationContext(model) { MemberName = "NonExistent" };
        var result = new IsDefaultAttribute().GetValidationResult(null, ctx);
        Assert.Equal(expected, result == ValidationResult.Success);
    }

    private sealed class TestModel
    {
        public string? Name { get; set; }
    }
}
