using System.Diagnostics.CodeAnalysis;
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

    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(BaseMustValidationUnitTestTestData.Constructor.ValidCases), MemberType = typeof(BaseMustValidationUnitTestTestData.Constructor))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: constructor completes without exception")]
        public static void BehavesAsExpected(BaseMustValidationUnitTestTestData.Constructor.Case testCase)
        {
            _ = testCase;
            _ = new Testable();
        }
    }
}
