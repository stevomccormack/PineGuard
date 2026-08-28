using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.Testing.UnitTests.UnitTests.DataAnnotations;

public sealed class BaseDataAnnotationUnitTestTests
{
    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class Testable() : BaseDataAnnotationUnitTest(null!)
    {
        public static void InvokeAssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage) =>
            AssertReturn(expected, actualIsValid, actualMessage);

        public static void InvokeAssertResult(DataAnnotationCase testCase, ValidationResult? result) =>
            AssertResult(testCase, result);

        public static void InvokeAssertResult(DataAnnotationCase testCase, ValidationResult? result, string? actualCode) =>
            AssertResult(testCase, result, actualCode);
    }

    public static class AssertReturnOps
    {
        [Theory]
        [MemberData(nameof(BaseDataAnnotationUnitTestTestData.AssertReturnOps.ValidCases), MemberType = typeof(BaseDataAnnotationUnitTestTestData.AssertReturnOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseDataAnnotationUnitTestTestData.AssertReturnOps.Case testCase)
        {
            var (expected, actualIsValid, actualMessage) = testCase.Value;
            Testable.InvokeAssertReturn(expected, actualIsValid, actualMessage);
        }
    }

    public static class AssertResultOps
    {
        [Theory]
        [MemberData(nameof(BaseDataAnnotationUnitTestTestData.AssertResultOps.ValidCases), MemberType = typeof(BaseDataAnnotationUnitTestTestData.AssertResultOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseDataAnnotationUnitTestTestData.AssertResultOps.Case testCase)
        {
            var (daCase, result) = testCase.Value;
            Testable.InvokeAssertResult(daCase, result);
        }
    }

    public static class AssertResultWithCodeOps
    {
        [Theory]
        [MemberData(nameof(BaseDataAnnotationUnitTestTestData.AssertResultWithCodeOps.ValidCases), MemberType = typeof(BaseDataAnnotationUnitTestTestData.AssertResultWithCodeOps))]
        public static void ValidAndEdge_BehavesAsExpected(BaseDataAnnotationUnitTestTestData.AssertResultWithCodeOps.Case testCase)
        {
            var (daCase, result, actualCode) = testCase.Value;
            Testable.InvokeAssertResult(daCase, result, actualCode);
        }
    }

    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(BaseDataAnnotationUnitTestTestData.Constructor.ValidCases), MemberType = typeof(BaseDataAnnotationUnitTestTestData.Constructor))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Implicit assertion: constructor completes without exception")]
        public static void BehavesAsExpected(BaseDataAnnotationUnitTestTestData.Constructor.Case testCase)
        {
            _ = testCase;
            _ = new Testable();
        }
    }
}
