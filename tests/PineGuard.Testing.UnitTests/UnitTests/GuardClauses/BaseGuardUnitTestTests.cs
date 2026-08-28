using System.Diagnostics.CodeAnalysis;
using PineGuard.GuardClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.GuardClauses;

public sealed class BaseGuardUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseGuardUnitTest(null!)
    {
        public const string ExposedCustomMessage = CustomMessage;

        public static TReturn InvokeAssertThrow<TReturn>(ThrowExpected expected, Func<TReturn> act) =>
            AssertThrow(expected, act);

        // ReSharper disable once UnusedMethodReturnValue.Local
        public static TReturn InvokeAssertResult<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act) =>
            AssertResult(testCase, act);

        public static void InvokeAssertCustomMessage<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act) =>
            AssertCustomMessage(testCase, act);
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

    public static class AssertThrowCodeOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertThrowCodeOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertThrowCodeOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertThrowCodeOps.Case testCase)
        {
            var (code, paramName) = testCase.Value;
            var expected = new GuardExpected(false, typeof(ArgumentException), paramName, null, code);

            Testable.InvokeAssertThrow<string>(expected, () => throw BuildException(code, paramName));
        }

        private static ArgumentException BuildException(string code, string? paramName)
        {
            var ex = new ArgumentException("test", paramName);
            ex.Data[GuardFailure.CodeDataKey] = code;
            ex.Data[GuardFailure.PropertyPathDataKey] = paramName ?? string.Empty;
            return ex;
        }
    }

    private sealed record NonGuardThrowExpected(bool IsValid, Type? ExceptionType) : ThrowExpected(IsValid, ExceptionType);

    public static class AssertThrowNonGuardOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertThrowNonGuardOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertThrowNonGuardOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertThrowNonGuardOps.Case testCase)
        {
            _ = testCase;
            var expected = new NonGuardThrowExpected(false, typeof(ArgumentException));

            Testable.InvokeAssertThrow<string>(expected, () => throw new ArgumentException("test"));
        }
    }

    public static class AssertCustomMessageOps
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.AssertCustomMessageOps.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.AssertCustomMessageOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseGuardUnitTestTestData.AssertCustomMessageOps.Case testCase)
        {
            var isValid = testCase.Value;
            var guardCase = new GuardCase<string>("test", "x", new GuardExpected(isValid, typeof(ArgumentException)));

            Func<string> act = isValid
                ? () => "ok"
                : () => throw new ArgumentException(Testable.ExposedCustomMessage + " suffix");

            Testable.InvokeAssertCustomMessage(guardCase, act);
        }
    }

    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(BaseGuardUnitTestTestData.Constructor.ValidCases), MemberType = typeof(BaseGuardUnitTestTestData.Constructor))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: constructor completes without exception")]
        public static void BehavesAsExpected(BaseGuardUnitTestTestData.Constructor.Case testCase)
        {
            _ = testCase;
            _ = new Testable();
        }
    }
}
