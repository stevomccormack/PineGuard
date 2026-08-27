using PineGuard.GuardClauses;
using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.GuardClauses;

public abstract class BaseGuardUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    /// <summary>
    /// The canonical custom message used to verify the optional <c>message</c> argument on every guard clause.
    /// </summary>
    protected const string CustomMessage = "Custom guard message.";

    protected static TReturn AssertThrow<TReturn>(ThrowExpected expected, Func<TReturn> act)
    {
        if (expected.IsValid)
            return act();

        var ex = Assert.Throws(expected.ExceptionType!, () => act());
        if (expected.ParamName is not null && ex is ArgumentException ae)
            Assert.Equal(expected.ParamName, ae.ParamName);
        if (expected.MessageContains is not null)
            Assert.Contains(expected.MessageContains, ex.Message);
        if (expected is GuardExpected { Code: { } code } guardExpected)
        {
            Assert.Equal(code, ex.Data[GuardFailure.CodeDataKey]);
            if (guardExpected.ParamName is not null)
                Assert.Equal(guardExpected.ParamName, ex.Data[GuardFailure.PropertyPathDataKey]);
        }

        return default!;
    }

    protected static TReturn AssertResult<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act) =>
        AssertThrow(testCase.Expected, act);

    /// <summary>
    /// Asserts that a guard invoked with an explicit <c>message</c> argument surfaces that message
    /// instead of the default <c>MustResult.Message</c>. No-ops for pass-through (valid) cases.
    /// </summary>
    protected static void AssertCustomMessage<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act)
    {
        if (testCase.Expected.IsValid)
            return;

        var ex = Assert.Throws(testCase.Expected.ExceptionType!, () => act());
        Assert.Contains(CustomMessage, ex.Message, StringComparison.Ordinal);
    }
}
