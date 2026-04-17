using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class OwaspAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    private static ValidationContext Ctx => new(new object()) { MemberName = "Value" };

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.OwaspSafeTypeMismatch.Cases), MemberType = typeof(OwaspAttributesTestData.OwaspSafeTypeMismatch))]
    public void OwaspSafe_ThrowsOnTypeMismatch(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.OwaspSafe.Cases), MemberType = typeof(OwaspAttributesTestData.OwaspSafe))]
    public void OwaspSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OwaspSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.XssSafe.Cases), MemberType = typeof(OwaspAttributesTestData.XssSafe))]
    public void XssSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new XssSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.SqlInjectionSafe.Cases), MemberType = typeof(OwaspAttributesTestData.SqlInjectionSafe))]
    public void SqlInjectionSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SqlInjectionSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.PathTraversalSafe.Cases), MemberType = typeof(OwaspAttributesTestData.PathTraversalSafe))]
    public void PathTraversalSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PathTraversalSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.CommandInjectionSafe.Cases), MemberType = typeof(OwaspAttributesTestData.CommandInjectionSafe))]
    public void CommandInjectionSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CommandInjectionSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.CrLfSafe.Cases), MemberType = typeof(OwaspAttributesTestData.CrLfSafe))]
    public void CrLfSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CrLfSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.LdapFilterSafe.Cases), MemberType = typeof(OwaspAttributesTestData.LdapFilterSafe))]
    public void LdapFilterSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LdapFilterSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.OpenRedirectSafe.Cases), MemberType = typeof(OwaspAttributesTestData.OpenRedirectSafe))]
    public void OpenRedirectSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new OpenRedirectSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(OwaspAttributesTestData.SsrfSchemeSafe.Cases), MemberType = typeof(OwaspAttributesTestData.SsrfSchemeSafe))]
    public void SsrfSchemeSafe_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new SsrfSchemeSafeAttribute();

        // Act
        var result = attr.GetValidationResult(tc.Value, Ctx);

        // Assert
        AssertResult(tc, result);
    }
}
