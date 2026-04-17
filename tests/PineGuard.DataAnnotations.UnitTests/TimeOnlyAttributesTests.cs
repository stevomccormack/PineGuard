using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TimeOnlyAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.BetweenTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.BetweenTimeOnly))]
    public void BetweenTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BetweenTimeOnlyAttribute("10:00", "12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotBetweenTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotBetweenTimeOnly))]
    public void NotBetweenTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotBetweenTimeOnlyAttribute("10:00", "12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.BeforeTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.BeforeTimeOnly))]
    public void BeforeTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforeTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.AfterTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.AfterTimeOnly))]
    public void AfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.OnOrBeforeTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.OnOrBeforeTimeOnly))]
    public void OnOrBeforeTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforeTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.OnOrAfterTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.OnOrAfterTimeOnly))]
    public void OnOrAfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.ChronologicalTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.ChronologicalTimeOnly))]
    public void ChronologicalTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ChronologicalTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotChronologicalTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotChronologicalTimeOnly))]
    public void NotChronologicalTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotChronologicalTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotBeforeTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotBeforeTimeOnly))]
    public void NotBeforeTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotBeforeTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotOnOrBeforeTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotOnOrBeforeTimeOnly))]
    public void NotOnOrBeforeTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrBeforeTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotAfterTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotAfterTimeOnly))]
    public void NotAfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotAfterTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotOnOrAfterTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotOnOrAfterTimeOnly))]
    public void NotOnOrAfterTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrAfterTimeOnlyAttribute("12:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.OverlappingTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.OverlappingTimeOnly))]
    public void OverlappingTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OverlappingTimeOnlyAttribute("12:00", "08:00", "09:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyAttributesTestData.NotOverlappingTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.NotOverlappingTimeOnly))]
    public void NotOverlappingTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOverlappingTimeOnlyAttribute("12:00", "08:00", "09:00");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
