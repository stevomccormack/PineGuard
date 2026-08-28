using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class EmailAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(EmailAttributesTestData.Email.Cases), MemberType = typeof(EmailAttributesTestData.Email))]
    public void Email_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new EmailAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(EmailAttributesTestData.StrictEmail.Cases), MemberType = typeof(EmailAttributesTestData.StrictEmail))]
    public void StrictEmail_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new StrictEmailAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EmailAttributesTestData.HasEmailAlias.Cases), MemberType = typeof(EmailAttributesTestData.HasEmailAlias))]
    public void HasEmailAlias_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HasEmailAliasAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(EmailAttributesTestData.NotHasEmailAlias.Cases), MemberType = typeof(EmailAttributesTestData.NotHasEmailAlias))]
    public void NotHasEmailAlias_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotHasEmailAliasAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
