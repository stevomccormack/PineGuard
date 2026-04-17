using FluentValidation.Results;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.Testing.UnitTests.UnitTests.FluentValidation;

public sealed class BaseFluentUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseFluentUnitTest(null!)
    {
        public static void InvokeAssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage) =>
            AssertReturn(expected, actualIsValid, actualMessage);

        public static void InvokeAssertResult<TValue>(FluentCase<TValue> testCase, ValidationResult result) =>
            AssertResult(testCase, result);
    }

    public static class AssertReturnOps
    {
        [Theory]
        [MemberData(nameof(BaseFluentUnitTestTestData.AssertReturnOps.ValidCases), MemberType = typeof(BaseFluentUnitTestTestData.AssertReturnOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseFluentUnitTestTestData.AssertReturnOps.Case testCase)
        {
            var (expected, actualIsValid, actualMessage) = testCase.Value;
            Testable.InvokeAssertReturn(expected, actualIsValid, actualMessage);
        }
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseFluentUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseFluentUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseFluentUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (fluentCase, result) = testCase.Value;
            Testable.InvokeAssertResult(fluentCase, result);
        }
    }
}
