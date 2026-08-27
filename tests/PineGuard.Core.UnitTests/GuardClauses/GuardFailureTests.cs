using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

[Collection(GuardPolicyCollection.Name)]
public sealed class GuardFailureTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(GuardFailureTestData.NullResultGuard.Cases), MemberType = typeof(GuardFailureTestData.NullResultGuard))]
    public void Throw_NullResult_ThrowsArgumentNullException(bool _) =>
        Assert.Throws<ArgumentNullException>(() => GuardFailure.Throw(null!));

    [Theory]
    [MemberData(nameof(GuardFailureTestData.DefaultException.Cases), MemberType = typeof(GuardFailureTestData.DefaultException))]
    public void Throw_NoPolicyNoCreator_ThrowsExpectedDefaultException(GuardFailureTestData.DefaultException.Case testCase)
    {
        // Arrange
        var result = MustResult<object?>.Fail("sample.always-fails", "{paramName} is bad.", "value", testCase.Value);

        // Act
        var ex = Assert.Throws(testCase.Expected, () => GuardFailure.Throw(result));

        // Assert
        Assert.Equal("value", Assert.IsAssignableFrom<ArgumentException>(ex).ParamName);
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.MessageOverride.Cases), MemberType = typeof(GuardFailureTestData.MessageOverride))]
    public void Throw_MessageOverride_UsesGivenMessageInsteadOfResultMessage(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");

        // Act
        var ex = Assert.Throws<ArgumentException>(() => GuardFailure.Throw(result, "custom message"));

        // Assert
        Assert.StartsWith("custom message", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ExceptionCreatorPrecedence.Cases), MemberType = typeof(GuardFailureTestData.ExceptionCreatorPrecedence))]
    public void Throw_ExceptionCreatorReturnsNonNull_ThrowsItDirectly_BypassingActiveMap(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");

        try
        {
            GuardExceptionPolicy.Map(_ => new InvalidOperationException("should not be used"));

            // Act
            var ex = Assert.Throws<ApplicationException>(() =>
                GuardFailure.Throw(result, exceptionCreator: () => new ApplicationException("from creator")));

            // Assert
            Assert.Equal("from creator", ex.Message);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.ExceptionCreatorReturnsNull.Cases), MemberType = typeof(GuardFailureTestData.ExceptionCreatorReturnsNull))]
    public void Throw_ExceptionCreatorReturnsNull_FallsBackToDefaultException(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");

        // Act
        var ex = Assert.Throws<ArgumentException>(() => GuardFailure.Throw(result, exceptionCreator: () => null!));

        // Assert
        Assert.Equal("value", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.MapReceivesFailure.Cases), MemberType = typeof(GuardFailureTestData.MapReceivesFailure))]
    public void Throw_MapInstalled_ReceivesFailureWithResultFieldsAndDefaultException(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");
        GuardFailure? captured = null;

        try
        {
            GuardExceptionPolicy.Map(failure =>
            {
                captured = failure;
                return new NotSupportedException("mapped: " + failure.Message);
            });

            // Act
            var ex = Assert.Throws<NotSupportedException>(() => GuardFailure.Throw(result));

            // Assert
            Assert.Equal("mapped: value is bad.", ex.Message);
            Assert.NotNull(captured);
            Assert.Equal("sample.always-fails", captured.Code);
            Assert.Equal("value is bad.", captured.Message);
            Assert.Equal("value", captured.ParamName);
            Assert.Equal("x", captured.Value);
            Assert.IsType<ArgumentException>(captured.Exception);
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.MapReturnsNull.Cases), MemberType = typeof(GuardFailureTestData.MapReturnsNull))]
    public void Throw_MapReturnsNull_ThrowsArgumentNullException(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");

        try
        {
            GuardExceptionPolicy.Map(_ => null!);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => GuardFailure.Throw(result));
        }
        finally
        {
            GuardExceptionPolicy.Clear();
        }
    }

    [Theory]
    [MemberData(nameof(GuardFailureTestData.PrintMembers.Cases), MemberType = typeof(GuardFailureTestData.PrintMembers))]
    public void ToString_ExcludesValue_ButIncludesOtherMembers(bool _)
    {
        // Arrange
        var failure = new GuardFailure("sample.always-fails", "value is bad.", "value", "super-secret", new ArgumentException("value is bad.", "value"));

        // Act
        var text = failure.ToString();

        // Assert
        Assert.Contains("sample.always-fails", text, StringComparison.Ordinal);
        Assert.Contains("value is bad.", text, StringComparison.Ordinal);
        Assert.DoesNotContain("super-secret", text, StringComparison.Ordinal);
    }
}
