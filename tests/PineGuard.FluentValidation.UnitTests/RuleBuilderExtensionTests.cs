using FluentValidation;
using FluentValidation.Results;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;
using Order = PineGuard.FluentValidation.UnitTests.RuleBuilderExtensionTestData.Order;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class RuleBuilderExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // RuleBuilderExtension.SetMustValidator
    [Theory]
    [MemberData(nameof(RuleBuilderExtensionTestData.SetMustValidator.Cases), MemberType = typeof(RuleBuilderExtensionTestData.SetMustValidator))]
    public void SetMustValidator_BehavesAsExpected(RuleBuilderExtensionTestData.SetMustValidator.Case tc)
    {
        // Arrange
        var (validator, order) = tc.Value;

        // Act
        var result = validator.Validate(order);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // RuleBuilderExtension.SetMustValidator (asynchronous parent)
    [Theory]
    [MemberData(nameof(RuleBuilderExtensionTestData.SetMustValidator.AsyncCases), MemberType = typeof(RuleBuilderExtensionTestData.SetMustValidator))]
    public async Task SetMustValidatorAsync_BehavesAsExpected(RuleBuilderExtensionTestData.SetMustValidator.Case tc)
    {
        // Arrange
        var (validator, order) = tc.Value;

        // Act
        var result = await validator.ValidateAsync(order, CancellationToken.None);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // RuleBuilderExtension.SetMustValidator
    [Theory]
    [MemberData(nameof(RuleBuilderExtensionTestData.SetMustValidator.InvalidCases), MemberType = typeof(RuleBuilderExtensionTestData.SetMustValidator))]
    public void SetMustValidator_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
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
