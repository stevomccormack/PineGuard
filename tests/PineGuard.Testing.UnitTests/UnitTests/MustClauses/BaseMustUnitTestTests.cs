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
}
