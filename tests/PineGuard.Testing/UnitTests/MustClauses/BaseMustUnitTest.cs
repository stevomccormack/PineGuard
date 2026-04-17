using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.MustClauses;

public abstract class BaseMustUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage)
    {
        Assert.Equal(expected.IsValid, actualIsValid);
        if (expected.Message is not null)
            Assert.Equal(expected.Message, actualMessage);
    }

    protected static void AssertResult<TValue, TResult>(MustCase<TValue> testCase, MustResult<TResult> result)
    {
        AssertReturn(testCase.Expected, result.Success, result.Message);
        if (testCase.Expected.ParamName is not null)
            Assert.Equal(testCase.Expected.ParamName, result.ParamName);
    }
}
