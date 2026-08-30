using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.ErrorOr.UnitTests;

public sealed class ErrorOrExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrorOr.Cases), MemberType = typeof(ErrorOrExtensionTestData.ToErrorOr))]
    public void ToErrorOr_BehavesAsExpected(ErrorOrCase<MustResult<string>> tc)
    {
        // Act
        var result = tc.Value.ToErrorOr();

        // Assert
        AssertErrorOr(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrorOr.InvalidCases), MemberType = typeof(ErrorOrExtensionTestData.ToErrorOr))]
    public void ToErrorOr_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToError.Cases), MemberType = typeof(ErrorOrExtensionTestData.ToError))]
    public void ToError_BehavesAsExpected(ErrorOrCase<MustFailure> tc)
    {
        // Act
        var error = tc.Value.ToError();

        // Assert
        AssertError(tc.Expected.Errors![0], error);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToError.InvalidCases), MemberType = typeof(ErrorOrExtensionTestData.ToError))]
    public void ToError_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrors.Cases), MemberType = typeof(ErrorOrExtensionTestData.ToErrors))]
    public void ToErrors_BehavesAsExpected(ErrorOrCase<MustValidationResult> tc)
    {
        // Act
        var errors = tc.Value.ToErrors();

        // Assert
        AssertErrors(tc.Expected.Errors!, errors);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrors.InvalidCases), MemberType = typeof(ErrorOrExtensionTestData.ToErrors))]
    public void ToErrors_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrorOrWithValue.Cases), MemberType = typeof(ErrorOrExtensionTestData.ToErrorOrWithValue))]
    public void ToErrorOrWithValue_BehavesAsExpected(ErrorOrCase<(MustValidationResult result, string value)> tc)
    {
        // Arrange
        var (validationResult, value) = tc.Value;

        // Act
        var result = validationResult.ToErrorOr(value);

        // Assert
        AssertErrorOr(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ErrorOrExtensionTestData.ToErrorOrWithValue.InvalidCases), MemberType = typeof(ErrorOrExtensionTestData.ToErrorOrWithValue))]
    public void ToErrorOrWithValue_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertErrorOr<T>(ErrorOrExpected expected, global::ErrorOr.ErrorOr<T> actual)
    {
        Assert.Equal(expected.IsValid, !actual.IsError);

        if (expected.IsValid)
        {
            Assert.Equal(expected.Value, actual.Value);
            return;
        }

        AssertErrors(expected.Errors!, actual.Errors);
    }

    private static void AssertErrors(IReadOnlyList<(string code, string description, string propertyPath)> expected, IReadOnlyList<global::ErrorOr.Error> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (var i = 0; i < expected.Count; i++)
            AssertError(expected[i], actual[i]);
    }

    private static void AssertError((string code, string description, string propertyPath) expected, global::ErrorOr.Error actual)
    {
        Assert.Equal(expected.code, actual.Code);
        Assert.Equal(expected.description, actual.Description);
        Assert.Equal(global::ErrorOr.ErrorType.Validation, actual.Type);
        Assert.NotNull(actual.Metadata);
        Assert.Equal(expected.propertyPath, actual.Metadata[ErrorOrExtension.PropertyPathMetadataKey]);
    }
}
