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
        // Order matters: testing !isValid first keeps both operands reachable. With the
        // MemberName check leading, a null MemberName short-circuits and the isValid
        // operand is never evaluated for pass-through cases.
        if (!isValid && testCase.Expected.MemberName is not null)
            Assert.Contains(testCase.Expected.MemberName, result!.MemberNames);
    }
}
