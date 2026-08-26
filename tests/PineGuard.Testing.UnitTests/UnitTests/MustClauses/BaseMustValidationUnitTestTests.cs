using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.MustClauses;

public sealed class BaseMustValidationUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseMustValidationUnitTest(null!)
    {
        public static void InvokeAssertResult<TValue>(MustValidationCase<TValue> testCase, MustValidationResult result) =>
            AssertResult(testCase, result);
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseMustValidationUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseMustValidationUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseMustValidationUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (validationCase, result) = testCase.Value;
            Testable.InvokeAssertResult(validationCase, result);
        }
    }
}
