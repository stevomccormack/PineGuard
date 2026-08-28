using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustValidationExceptionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationExceptionTestData.Constructor.ValidCases), MemberType = typeof(MustValidationExceptionTestData.Constructor))]
    public void Constructor_SetsMessageResultAndInnerException(MustValidationExceptionTestData.Constructor.ValidCase testCase)
    {
        // Act
        var exception = testCase.Value();

        // Assert
        Assert.Same(testCase.ExpectedResult, exception.Result);
        Assert.Equal(testCase.ExpectedMessage, exception.Message);
        Assert.Same(testCase.ExpectedInnerException, exception.InnerException);
    }

    [Theory]
    [MemberData(nameof(MustValidationExceptionTestData.Constructor.InvalidCases), MemberType = typeof(MustValidationExceptionTestData.Constructor))]
    public void Constructor_NullResult_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationExceptionTestData.Constructor.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, t.Value);

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
