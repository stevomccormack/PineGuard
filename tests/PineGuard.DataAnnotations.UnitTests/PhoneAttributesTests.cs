using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class PhoneAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(PhoneAttributesTestData.PhoneNumberTypeMismatch.Cases), MemberType = typeof(PhoneAttributesTestData.PhoneNumberTypeMismatch))]
    public void PhoneNumber_TypeMismatch_ThrowsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(PhoneAttributesTestData.DefaultPhoneNumber.Cases), MemberType = typeof(PhoneAttributesTestData.DefaultPhoneNumber))]
    public void DefaultPhoneNumber_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new PhoneNumberAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(PhoneAttributesTestData.CustomPhoneNumber.Cases), MemberType = typeof(PhoneAttributesTestData.CustomPhoneNumber))]
    public void CustomPhoneNumber_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new CustomPhoneNumberAttribute(3, 5);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
