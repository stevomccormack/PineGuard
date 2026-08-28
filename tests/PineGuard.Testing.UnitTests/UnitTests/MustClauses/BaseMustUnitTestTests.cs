using System.Diagnostics.CodeAnalysis;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.MustClauses;

public sealed class BaseMustUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseMustUnitTest(null!)
    {
        public static void InvokeAssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage) =>
            AssertReturn(expected, actualIsValid, actualMessage);

        public static void InvokeAssertResult<TValue, TResult>(MustCase<TValue> testCase, MustResult<TResult> result) =>
            AssertResult(testCase, result);
    }

    public static class AssertReturnOps
    {
        [Theory]
        [MemberData(nameof(BaseMustUnitTestTestData.AssertReturnOps.ValidCases), MemberType = typeof(BaseMustUnitTestTestData.AssertReturnOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseMustUnitTestTestData.AssertReturnOps.Case testCase)
        {
            var (expected, actualIsValid, actualMessage) = testCase.Value;
            Testable.InvokeAssertReturn(expected, actualIsValid, actualMessage);
        }
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseMustUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseMustUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseMustUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (mustCase, result) = testCase.Value;
            Testable.InvokeAssertResult(mustCase, result);
        }
    }

    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(BaseMustUnitTestTestData.Constructor.ValidCases), MemberType = typeof(BaseMustUnitTestTestData.Constructor))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: constructor completes without exception")]
        public static void BehavesAsExpected(BaseMustUnitTestTestData.Constructor.Case testCase)
        {
            _ = testCase;
            _ = new Testable();
        }
    }
}
