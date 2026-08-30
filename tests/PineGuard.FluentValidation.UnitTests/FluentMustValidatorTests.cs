using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;
using Customer = PineGuard.FluentValidation.UnitTests.FluentMustValidatorTestData.Customer;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentMustValidatorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // FluentMustValidator<T>.Validate
    [Theory]
    [MemberData(nameof(FluentMustValidatorTestData.Validate.Cases), MemberType = typeof(FluentMustValidatorTestData.Validate))]
    public void Validate_BehavesAsExpected(ValidationBridgeCase<Customer> tc)
    {
        // Arrange
        var validator = new FluentMustValidator<Customer>(new FluentMustValidatorTestData.CustomerValidator());

        // Act
        var result = validator.Validate(tc.Value);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // FluentMustValidator<T>.ValidateAsync
    [Theory]
    [MemberData(nameof(FluentMustValidatorTestData.ValidateAsync.Cases), MemberType = typeof(FluentMustValidatorTestData.ValidateAsync))]
    public async Task ValidateAsync_BehavesAsExpected(ValidationBridgeCase<Customer> tc)
    {
        // Arrange
        var validator = new FluentMustValidator<Customer>(new FluentMustValidatorTestData.AsyncCustomerValidator());

        // Act
        var result = await validator.ValidateAsync(tc.Value, CancellationToken.None);

        // Assert
        AssertResult(tc.Expected, result);
    }

    // FluentMustValidator<T>.Validator
    [Theory]
    [MemberData(nameof(FluentMustValidatorTestData.Validator.Cases), MemberType = typeof(FluentMustValidatorTestData.Validator))]
    public void Validator_BehavesAsExpected(bool _)
    {
        // Arrange
        var inner = new FluentMustValidatorTestData.CustomerValidator();

        // Act
        var validator = new FluentMustValidator<Customer>(inner);

        // Assert
        Assert.Same(inner, validator.Validator);
        Assert.Equal(typeof(Customer), ((IMustValidator)validator).ValidatedType);
    }

    // FluentMustValidator<T>..ctor
    [Theory]
    [MemberData(nameof(FluentMustValidatorTestData.Constructor.InvalidCases), MemberType = typeof(FluentMustValidatorTestData.Constructor))]
    public void Constructor_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertResult(ValidationBridgeExpected expected, MustValidationResult actual)
    {
        Assert.Equal(expected.IsValid, actual.Success);
        Assert.Equal(expected.Failures!.Count, actual.Failures.Count);

        for (var i = 0; i < expected.Failures.Count; i++)
        {
            var (propertyPath, code, message) = expected.Failures[i];
            Assert.Equal(propertyPath, actual.Failures[i].PropertyPath);
            Assert.Equal(code, actual.Failures[i].Code);
            Assert.Equal(message, actual.Failures[i].Message);
        }
    }
}
