using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringTimeOnlyAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.InRangeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.InRangeTimeOnlyString))]
    public void InRangeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        InRangeTimeOnlyStringAttribute attr = new InRangeTimeOnlyStringAttribute(new TimeOnly(10, 0).ToString("HH:mm"), new TimeOnly(14, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.BeforeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.BeforeTimeOnlyString))]
    public void BeforeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforeTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.AfterTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.AfterTimeOnlyString))]
    public void AfterTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.OutOfRangeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.OutOfRangeTimeOnlyString))]
    public void OutOfRangeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        OutOfRangeTimeOnlyStringAttribute attr = new OutOfRangeTimeOnlyStringAttribute(new TimeOnly(10, 0).ToString("HH:mm"), new TimeOnly(14, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.InRangeTimeOnlyStringWrap.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.InRangeTimeOnlyStringWrap))]
    public void InRangeTimeOnlyStringWrap_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new InRangeTimeOnlyStringAttribute("22:00", "02:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.OutOfRangeTimeOnlyStringWrap.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.OutOfRangeTimeOnlyStringWrap))]
    public void OutOfRangeTimeOnlyStringWrap_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OutOfRangeTimeOnlyStringAttribute("22:00", "02:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotBeforeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotBeforeTimeOnlyString))]
    public void NotBeforeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotBeforeTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.OnOrBeforeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.OnOrBeforeTimeOnlyString))]
    public void OnOrBeforeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforeTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotOnOrBeforeTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotOnOrBeforeTimeOnlyString))]
    public void NotOnOrBeforeTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrBeforeTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotAfterTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotAfterTimeOnlyString))]
    public void NotAfterTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotAfterTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.OnOrAfterTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.OnOrAfterTimeOnlyString))]
    public void OnOrAfterTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotOnOrAfterTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotOnOrAfterTimeOnlyString))]
    public void NotOnOrAfterTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrAfterTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.SameTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.SameTimeOnlyString))]
    public void SameTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SameTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotSameTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotSameTimeOnlyString))]
    public void NotSameTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotSameTimeOnlyStringAttribute(new TimeOnly(12, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringTimeOnlyAttributesTestData.NotChronologicalTimeOnlyString.Cases), MemberType = typeof(StringTimeOnlyAttributesTestData.NotChronologicalTimeOnlyString))]
    public void NotChronologicalTimeOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotChronologicalTimeOnlyStringAttribute(new TimeOnly(14, 0).ToString("HH:mm"));
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
