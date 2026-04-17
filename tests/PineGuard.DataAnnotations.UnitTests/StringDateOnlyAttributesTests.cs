using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringDateOnlyAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.PastDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.PastDateOnlyString))]
    public void PastDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PastDateOnlyStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.FutureDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.FutureDateOnlyString))]
    public void FutureDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FutureDateOnlyStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.PastOrPresentDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.PastOrPresentDateOnlyString))]
    public void PastOrPresentDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PastOrPresentDateOnlyStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.FutureOrPresentDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.FutureOrPresentDateOnlyString))]
    public void FutureOrPresentDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FutureOrPresentDateOnlyStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.BeforeDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.BeforeDateOnlyString))]
    public void BeforeDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new BeforeDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotBeforeDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotBeforeDateOnlyString))]
    public void NotBeforeDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotBeforeDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.OnOrBeforeDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.OnOrBeforeDateOnlyString))]
    public void OnOrBeforeDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrBeforeDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotOnOrBeforeDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotOnOrBeforeDateOnlyString))]
    public void NotOnOrBeforeDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrBeforeDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.AfterDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.AfterDateOnlyString))]
    public void AfterDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AfterDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotAfterDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotAfterDateOnlyString))]
    public void NotAfterDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotAfterDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.OnOrAfterDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.OnOrAfterDateOnlyString))]
    public void OnOrAfterDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OnOrAfterDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotOnOrAfterDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotOnOrAfterDateOnlyString))]
    public void NotOnOrAfterDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOnOrAfterDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.SameDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.SameDateOnlyString))]
    public void SameDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SameDateOnlyStringAttribute("2000-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotSameDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotSameDateOnlyString))]
    public void NotSameDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotSameDateOnlyStringAttribute("2000-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.ChronologicalDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.ChronologicalDateOnlyString))]
    public void ChronologicalDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new ChronologicalDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotChronologicalDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotChronologicalDateOnlyString))]
    public void NotChronologicalDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotChronologicalDateOnlyStringAttribute("2001-01-01");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.OverlappingDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.OverlappingDateOnlyString))]
    public void OverlappingDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OverlappingDateOnlyStringAttribute("2020-02-28", "2020-01-01", "2020-06-30");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringDateOnlyAttributesTestData.NotOverlappingDateOnlyString.Cases), MemberType = typeof(StringDateOnlyAttributesTestData.NotOverlappingDateOnlyString))]
    public void NotOverlappingDateOnlyString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotOverlappingDateOnlyStringAttribute("2020-02-28", "2020-01-01", "2020-06-30");
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
