using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.GuardClauses;

public abstract class BaseGuardUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static TReturn AssertThrow<TReturn>(ThrowExpected expected, Func<TReturn> act)
    {
        if (expected.IsValid)
            return act();

        var ex = Assert.Throws(expected.ExceptionType!, () => act());
        if (expected.ParamName is not null && ex is ArgumentException ae)
            Assert.Equal(expected.ParamName, ae.ParamName);
        if (expected.MessageContains is not null)
            Assert.Contains(expected.MessageContains, ex.Message);
        return default!;
    }

    protected static TReturn AssertResult<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act) =>
        AssertThrow(testCase.Expected, act);
}
