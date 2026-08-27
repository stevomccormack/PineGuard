using FluentValidation.Results;
using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.FluentValidation;

public abstract class BaseFluentUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage)
    {
        Assert.Equal(expected.IsValid, actualIsValid);
        if (expected.Message is not null)
            Assert.Equal(expected.Message, actualMessage);
    }

    protected static void AssertResult<TValue>(FluentCase<TValue> testCase, ValidationResult result)
    {
        AssertReturn(testCase.Expected, result.IsValid, result.IsValid ? null : result.Errors[0].ErrorMessage);
        if (testCase.Expected.PropertyName is not null)
            Assert.Equal(testCase.Expected.PropertyName, result.Errors[0].PropertyName);
        if (testCase.Expected.Code is not null)
            Assert.Equal(testCase.Expected.Code, result.Errors[0].ErrorCode);
    }
}
