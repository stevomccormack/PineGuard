using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.MustClauses;

public abstract class BaseMustValidationUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
{
    protected static void AssertResult<TValue>(MustValidationCase<TValue> testCase, MustValidationResult result)
    {
        Assert.Equal(testCase.Expected.IsValid, result.Success);

        if (testCase.Expected.FailureCount is int expectedCount)
            Assert.Equal(expectedCount, result.Failures.Count);

        if (result.Failures.Count == 0)
            return;

        var failure = result.Failures[0];

        if (testCase.Expected.PropertyPath is not null)
            Assert.Equal(testCase.Expected.PropertyPath, failure.PropertyPath);

        if (testCase.Expected.Code is not null)
            Assert.Equal(testCase.Expected.Code, failure.Code);

        if (testCase.Expected.Message is not null)
            Assert.Equal(testCase.Expected.Message, failure.Message);
    }
}
