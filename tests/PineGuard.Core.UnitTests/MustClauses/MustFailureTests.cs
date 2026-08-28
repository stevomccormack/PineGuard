using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustFailureTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustFailureTestData.From.ValidCases), MemberType = typeof(MustFailureTestData.From))]
    public void From_BuildsFailure_FromFailedResult(MustFailureTestData.From.ValidCase testCase)
    {
        // Act
        var failure = MustFailure.From(testCase.Result, testCase.PropertyPath);

        // Assert
        Assert.Equal(testCase.Expected.ExpectedPropertyPath, failure.PropertyPath);
        Assert.Equal(testCase.Expected.ExpectedCode, failure.Code);
        Assert.Equal(testCase.Expected.ExpectedMessage, failure.Message);
        Assert.Equal(testCase.Expected.ExpectedValue, failure.Value);
    }

    [Theory]
    [MemberData(nameof(MustFailureTestData.From.InvalidCases), MemberType = typeof(MustFailureTestData.From))]
    public void From_NullOrSuccessfulResult_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustFailureTestData.From.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustFailure.From(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustFailureTestData.Properties.Cases), MemberType = typeof(MustFailureTestData.Properties))]
    public void Value_RoundTrips_ButIsExcludedFromToString(MustFailureTestData.Properties.Case testCase)
    {
        // Act
        var rendered = testCase.Value.ToString();

        // Assert
        Assert.Equal(testCase.SentinelValue, testCase.Value.Value);
        Assert.DoesNotContain(testCase.SentinelValue, rendered, StringComparison.Ordinal);
        Assert.Contains(testCase.Value.PropertyPath, rendered, StringComparison.Ordinal);
        Assert.Contains(testCase.Value.Code, rendered, StringComparison.Ordinal);
        Assert.Contains(testCase.Value.Message, rendered, StringComparison.Ordinal);
    }
}
