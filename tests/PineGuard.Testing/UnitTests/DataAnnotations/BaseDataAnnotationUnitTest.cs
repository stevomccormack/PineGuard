using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.DataAnnotations;

public abstract class BaseDataAnnotationUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage)
    {
        Assert.Equal(expected.IsValid, actualIsValid);
        if (expected.Message is not null)
            Assert.Equal(expected.Message, actualMessage);
    }

    protected static void AssertResult(DataAnnotationCase testCase, ValidationResult? result)
    {
        var isValid = result == ValidationResult.Success;
        AssertReturn(testCase.Expected, isValid, isValid ? null : result!.ErrorMessage);
        if (testCase.Expected.MemberName is not null && !isValid)
            Assert.Contains(testCase.Expected.MemberName, result!.MemberNames);
    }
}
