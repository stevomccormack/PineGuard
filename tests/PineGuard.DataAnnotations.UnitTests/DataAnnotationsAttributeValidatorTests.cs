using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DataAnnotationsAttributeValidatorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // DataAnnotationsAttributeValidator.Validate
    [Theory]
    [MemberData(nameof(DataAnnotationsAttributeValidatorTestData.Validate.Cases), MemberType = typeof(DataAnnotationsAttributeValidatorTestData.Validate))]
    public void Validate_BehavesAsExpected(DataAnnotationsAttributeValidatorTestData.Validate.Case tc)
    {
        // Act
        var result = DataAnnotationsAttributeValidator.Validate(tc.Value);

        // Assert
        AssertFailures(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DataAnnotationsAttributeValidatorTestData.Validate.InvalidCases), MemberType = typeof(DataAnnotationsAttributeValidatorTestData.Validate))]
    public void Validate_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertFailures(DataAnnotationsAttributeValidatorTestData.Validate.ValidateExpected expected, MustValidationResult actual)
    {
        Assert.Equal(expected.IsValid, actual.Success);
        Assert.Equal(expected.Failures.Count, actual.Failures.Count);

        for (var i = 0; i < expected.Failures.Count; i++)
        {
            var failure = expected.Failures[i];
            Assert.Equal(failure.PropertyPath, actual.Failures[i].PropertyPath);
            Assert.Equal(failure.Code, actual.Failures[i].Code);

            if (failure.Message is { } message)
                Assert.Equal(message, actual.Failures[i].Message);
            else
                Assert.False(string.IsNullOrEmpty(actual.Failures[i].Message));
        }
    }
}
