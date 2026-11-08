using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringNumberTypesAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.DecimalString.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.DecimalString))]
    public void DecimalString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new DecimalStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.ExactDecimalString.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.ExactDecimalString))]
    public void ExactDecimalString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ExactDecimalStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.Int32String.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.Int32String))]
    public void Int32String_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int32StringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.Int64String.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.Int64String))]
    public void Int64String_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int64StringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.InRangeInt32StringInclusive.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.InRangeInt32StringInclusive))]
    public void InRangeInt32String_Inclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeInt32StringAttribute attr = new InRangeInt32StringAttribute(1, 10);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.InRangeInt32StringExclusive.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.InRangeInt32StringExclusive))]
    public void InRangeInt32String_Exclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeInt32StringAttribute attr = new InRangeInt32StringAttribute(1, 10, Inclusion.Exclusive);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.OutOfRangeInt32String.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.OutOfRangeInt32String))]
    public void OutOfRangeInt32String_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        OutOfRangeInt32StringAttribute attr = new OutOfRangeInt32StringAttribute(1, 10);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.InRangeInt64StringInclusive.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.InRangeInt64StringInclusive))]
    public void InRangeInt64String_Inclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeInt64StringAttribute attr = new InRangeInt64StringAttribute(1L, 10L);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.InRangeInt64StringExclusive.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.InRangeInt64StringExclusive))]
    public void InRangeInt64String_Exclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeInt64StringAttribute attr = new InRangeInt64StringAttribute(1L, 10L, Inclusion.Exclusive);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypesAttributesTestData.OutOfRangeInt64String.Cases), MemberType = typeof(StringNumberTypesAttributesTestData.OutOfRangeInt64String))]
    public void OutOfRangeInt64String_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        OutOfRangeInt64StringAttribute attr = new OutOfRangeInt64StringAttribute(1L, 10L);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
