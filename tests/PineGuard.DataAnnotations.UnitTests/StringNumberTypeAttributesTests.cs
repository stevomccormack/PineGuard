using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringNumberTypeAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.DecimalString.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.DecimalString))]
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
    [MemberData(nameof(StringNumberTypeAttributesTestData.ExactDecimalString.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.ExactDecimalString))]
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
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int32String.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int32String))]
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
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int64String.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int64String))]
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
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int32InRangeStringInclusive.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int32InRangeStringInclusive))]
    public void Int32InRangeString_Inclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int32InRangeStringAttribute(1, 10);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int32InRangeStringExclusive.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int32InRangeStringExclusive))]
    public void Int32InRangeString_Exclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int32InRangeStringAttribute(1, 10, Inclusion.Exclusive);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int32OutOfRangeString.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int32OutOfRangeString))]
    public void Int32OutOfRangeString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int32OutOfRangeStringAttribute(1, 10);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int64InRangeStringInclusive.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int64InRangeStringInclusive))]
    public void Int64InRangeString_Inclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int64InRangeStringAttribute(1L, 10L);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int64InRangeStringExclusive.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int64InRangeStringExclusive))]
    public void Int64InRangeString_Exclusive_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int64InRangeStringAttribute(1L, 10L, Inclusion.Exclusive);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringNumberTypeAttributesTestData.Int64OutOfRangeString.Cases), MemberType = typeof(StringNumberTypeAttributesTestData.Int64OutOfRangeString))]
    public void Int64OutOfRangeString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Int64OutOfRangeStringAttribute(1L, 10L);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
