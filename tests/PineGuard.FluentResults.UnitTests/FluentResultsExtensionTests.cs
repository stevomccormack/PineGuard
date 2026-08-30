using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.FluentResults.UnitTests;

public sealed class FluentResultsExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResult.Cases), MemberType = typeof(FluentResultsExtensionTestData.ToResult))]
    public void ToResult_BehavesAsExpected(FluentResultsCase<MustResult<string>> tc)
    {
        // Act
        var result = tc.Value.ToResult();

        // Assert
        AssertResult(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResult.InvalidCases), MemberType = typeof(FluentResultsExtensionTestData.ToResult))]
    public void ToResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResultFromValidationResult.Cases), MemberType = typeof(FluentResultsExtensionTestData.ToResultFromValidationResult))]
    public void ToResultFromValidationResult_BehavesAsExpected(FluentResultsCase<MustValidationResult> tc)
    {
        // Act
        var result = tc.Value.ToResult();

        // Assert
        Assert.Equal(tc.Expected.IsValid, result.IsSuccess);
        MustErrorAssert.Expected(tc.Expected.Errors!, result.Errors);
    }

    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResultFromValidationResult.InvalidCases), MemberType = typeof(FluentResultsExtensionTestData.ToResultFromValidationResult))]
    public void ToResultFromValidationResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResultFromValidationResultWithValue.Cases), MemberType = typeof(FluentResultsExtensionTestData.ToResultFromValidationResultWithValue))]
    public void ToResultFromValidationResultWithValue_BehavesAsExpected(FluentResultsCase<(MustValidationResult result, string value)> tc)
    {
        // Arrange
        var (validationResult, value) = tc.Value;

        // Act
        var result = validationResult.ToResult(value);

        // Assert
        AssertResult(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(FluentResultsExtensionTestData.ToResultFromValidationResultWithValue.InvalidCases), MemberType = typeof(FluentResultsExtensionTestData.ToResultFromValidationResultWithValue))]
    public void ToResultFromValidationResultWithValue_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertResult<T>(FluentResultsExpected expected, global::FluentResults.Result<T> actual)
    {
        Assert.Equal(expected.IsValid, actual.IsSuccess);

        if (expected.IsValid)
        {
            Assert.Equal(expected.Value, actual.Value);
            return;
        }

        MustErrorAssert.Expected(expected.Errors!, actual.Errors);
    }
}
