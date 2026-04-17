using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.GuardClauses;

public sealed class BaseGuardUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseGuardUnitTest(null!)
    {
        public static TReturn InvokeAssertThrow<TReturn>(ThrowExpected expected, Func<TReturn> act) =>
            AssertThrow(expected, act);

        // ReSharper disable once UnusedMethodReturnValue.Local
        public static TReturn InvokeAssertResult<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act) =>
            AssertResult(testCase, act);
    }

    public static class AssertThrowValidOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertThrowValidOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertThrowValidOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertThrowValidOps.Case testCase)
        {
            _ = testCase;
            var result = Testable.InvokeAssertThrow(new GuardExpected(true), () => "ok");
            Assert.Equal("ok", result);
        }
    }

    public static class AssertThrowInvalidOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertThrowInvalidOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertThrowInvalidOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertThrowInvalidOps.Case testCase)
        {
            var (exceptionType, paramName, messageContains) = testCase.Value;
            var expected = new GuardExpected(false, exceptionType, paramName, messageContains);

            Exception ex = exceptionType == typeof(ArgumentException)
                ? new ArgumentException((messageContains ?? "test") + " input", paramName)
                : new InvalidOperationException(messageContains ?? "test");

            Testable.InvokeAssertThrow<string>(expected, () => throw ex);
        }
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (isValid, exceptionType, paramName) = testCase.Value;
            var guardCase = new GuardCase<string>("test", "x", new GuardExpected(isValid, exceptionType, paramName));

            Func<string> act = isValid
                ? () => "ok"
                : () => throw new ArgumentException("test", paramName);

            Testable.InvokeAssertResult(guardCase, act);
        }
    }
}
