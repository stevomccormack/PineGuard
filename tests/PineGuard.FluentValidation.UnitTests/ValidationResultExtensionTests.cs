using FluentValidation.Results;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class ValidationResultExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // ValidationResultExtension.ToMustValidationResult
    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToMustValidationResult.Cases), MemberType = typeof(ValidationResultExtensionTestData.ToMustValidationResult))]
    public void ToMustValidationResult_BehavesAsExpected(ValidationBridgeCase<ValidationResult> tc)
    {
        // Act
        var result = tc.Value.ToMustValidationResult();

        // Assert
        AssertMustValidationResult(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToMustValidationResult.InvalidCases), MemberType = typeof(ValidationResultExtensionTestData.ToMustValidationResult))]
    public void ToMustValidationResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    // ValidationResultExtension.ToMustFailure
    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToMustFailure.Cases), MemberType = typeof(ValidationResultExtensionTestData.ToMustFailure))]
    public void ToMustFailure_BehavesAsExpected(ValidationBridgeCase<ValidationFailure> tc)
    {
        // Act
        var failure = tc.Value.ToMustFailure();

        // Assert
        AssertMustFailure(tc.Expected.Failures![0], failure);
        Assert.Equal(tc.Expected.Value, failure.Value);
    }

    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToMustFailure.InvalidCases), MemberType = typeof(ValidationResultExtensionTestData.ToMustFailure))]
    public void ToMustFailure_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    // ValidationResultExtension.ToValidationResult
    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToValidationResult.Cases), MemberType = typeof(ValidationResultExtensionTestData.ToValidationResult))]
    public void ToValidationResult_BehavesAsExpected(ValidationBridgeCase<MustValidationResult> tc)
    {
        // Act
        var result = tc.Value.ToValidationResult();

        // Assert
        AssertValidationResult(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToValidationResult.InvalidCases), MemberType = typeof(ValidationResultExtensionTestData.ToValidationResult))]
    public void ToValidationResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    // ValidationResultExtension.ToValidationFailure
    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToValidationFailure.Cases), MemberType = typeof(ValidationResultExtensionTestData.ToValidationFailure))]
    public void ToValidationFailure_BehavesAsExpected(ValidationBridgeCase<MustFailure> tc)
    {
        // Act
        var failure = tc.Value.ToValidationFailure();

        // Assert
        AssertValidationFailure(tc.Expected.Failures![0], failure);
    }

    [Theory]
    [MemberData(nameof(ValidationResultExtensionTestData.ToValidationFailure.InvalidCases), MemberType = typeof(ValidationResultExtensionTestData.ToValidationFailure))]
    public void ToValidationFailure_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertMustValidationResult(ValidationBridgeExpected expected, MustValidationResult actual)
    {
        Assert.Equal(expected.IsValid, actual.Success);
        Assert.Equal(expected.Failures!.Count, actual.Failures.Count);

        for (var i = 0; i < expected.Failures.Count; i++)
            AssertMustFailure(expected.Failures[i], actual.Failures[i]);
    }

    private static void AssertMustFailure((string propertyPath, string code, string message) expected, MustFailure actual)
    {
        Assert.Equal(expected.propertyPath, actual.PropertyPath);
        Assert.Equal(expected.code, actual.Code);
        Assert.Equal(expected.message, actual.Message);
    }

    private static void AssertValidationResult(ValidationBridgeExpected expected, ValidationResult actual)
    {
        Assert.Equal(expected.IsValid, actual.IsValid);
        Assert.Equal(expected.Failures!.Count, actual.Errors.Count);

        for (var i = 0; i < expected.Failures.Count; i++)
            AssertValidationFailure(expected.Failures[i], actual.Errors[i]);
    }

    private static void AssertValidationFailure((string propertyPath, string code, string message) expected, ValidationFailure actual)
    {
        Assert.Equal(expected.propertyPath, actual.PropertyName);
        Assert.Equal(expected.code, actual.ErrorCode);
        Assert.Equal(expected.message, actual.ErrorMessage);
        Assert.Null(actual.AttemptedValue);
    }
}
