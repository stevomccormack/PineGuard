using FluentValidation.Results;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;
using Address = PineGuard.FluentValidation.UnitTests.RuleBuilderExtensionTestData.Address;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class MustChildValidatorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // MustChildValidator<T>.Validate(T?)
    [Theory]
    [MemberData(nameof(MustChildValidatorTestData.Validate.Cases), MemberType = typeof(MustChildValidatorTestData.Validate))]
    public void Validate_BehavesAsExpected(ValidationBridgeCase<Address?> tc)
    {
        // Arrange
        var validator = MustChildValidatorTestData.NewValidator();

        // Act
        var result = validator.Validate(tc.Value);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // MustChildValidator<T>.ValidateAsync(T?, CancellationToken)
    [Theory]
    [MemberData(nameof(MustChildValidatorTestData.ValidateAsync.Cases), MemberType = typeof(MustChildValidatorTestData.ValidateAsync))]
    public async Task ValidateAsync_BehavesAsExpected(ValidationBridgeCase<Address?> tc)
    {
        // Arrange
        var validator = MustChildValidatorTestData.NewAsyncValidator();

        // Act
        var result = await validator.ValidateAsync(tc.Value, CancellationToken.None);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // MustChildValidator<T>.Validate(IValidationContext) / ValidateAsync(IValidationContext, CancellationToken)
    [Theory]
    [MemberData(nameof(MustChildValidatorTestData.ValidateContext.InvalidCases), MemberType = typeof(MustChildValidatorTestData.ValidateContext))]
    public void ValidateContext_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    // MustChildValidator<T>.CanValidateInstancesOfType
    [Theory]
    [MemberData(nameof(MustChildValidatorTestData.CanValidateInstancesOfType.Cases), MemberType = typeof(MustChildValidatorTestData.CanValidateInstancesOfType))]
    public void CanValidateInstancesOfType_BehavesAsExpected(MustChildValidatorTestData.CanValidateInstancesOfType.TypeCase tc)
    {
        // Arrange
        var validator = MustChildValidatorTestData.NewValidator();

        // Act
        var result = validator.CanValidateInstancesOfType(tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    // MustChildValidator<T>.CreateDescriptor
    [Theory]
    [MemberData(nameof(MustChildValidatorTestData.CreateDescriptor.Cases), MemberType = typeof(MustChildValidatorTestData.CreateDescriptor))]
    public void CreateDescriptor_BehavesAsExpected(bool _)
    {
        // Arrange
        var validator = MustChildValidatorTestData.NewValidator();

        // Act
        var descriptor = validator.CreateDescriptor();

        // Assert
        Assert.NotNull(descriptor);
        Assert.Empty(descriptor.GetMembersWithValidators());
    }

    private static void AssertResult(ValidationBridgeExpected expected, ValidationResult actual)
    {
        Assert.Equal(expected.IsValid, actual.IsValid);
        Assert.Equal(expected.Failures!.Count, actual.Errors.Count);

        for (var i = 0; i < expected.Failures.Count; i++)
        {
            var (propertyPath, code, message) = expected.Failures[i];
            Assert.Equal(propertyPath, actual.Errors[i].PropertyName);
            Assert.Equal(code, actual.Errors[i].ErrorCode);
            Assert.Equal(message, actual.Errors[i].ErrorMessage);
        }
    }
}
