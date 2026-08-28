using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.GuardClauses;

public sealed class ExceptionExtensionTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(ExceptionExtensionTestData.RoundTrip.Cases), MemberType = typeof(ExceptionExtensionTestData.RoundTrip))]
    public void TryGetMustCode_AndGetMustPropertyPath_RoundTripWhatGuardFailureStamped(bool _)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");
        var thrown = Assert.Throws<ArgumentException>(() => GuardFailure.Throw(result));

        // Act
        var found = thrown.TryGetMustCode(out var code);

        // Assert
        Assert.True(found);
        Assert.Equal("sample.always-fails", code);
        Assert.True(thrown.HasMustCode("sample.always-fails"));
        Assert.Equal("value", thrown.GetMustPropertyPath());
    }

    [Theory]
    [MemberData(nameof(ExceptionExtensionTestData.Unstamped.Cases), MemberType = typeof(ExceptionExtensionTestData.Unstamped))]
    public void TryGetMustCode_AndGetMustPropertyPath_OnUnstampedException_ReturnEmptyOrFalse(bool _)
    {
        // Arrange
        var exception = new InvalidOperationException("not stamped by a guard clause");

        // Act
        var found = exception.TryGetMustCode(out var code);

        // Assert
        Assert.False(found);
        Assert.Null(code);
        Assert.False(exception.HasMustCode("sample.always-fails"));
        Assert.Equal(string.Empty, exception.GetMustPropertyPath());
    }

    [Theory]
    [MemberData(nameof(ExceptionExtensionTestData.HasMustCode.Cases), MemberType = typeof(ExceptionExtensionTestData.HasMustCode))]
    public void HasMustCode_ComparesAgainstTheStampedCode(ExceptionExtensionTestData.HasMustCode.Case testCase)
    {
        // Arrange
        var result = MustResult<string>.Fail("sample.always-fails", "{paramName} is bad.", "value", "x");
        var thrown = Assert.Throws<ArgumentException>(() => GuardFailure.Throw(result));
        var codeToCheck = testCase.CodeMatches ? "sample.always-fails" : "sample.other-code";

        // Act
        var actual = thrown.HasMustCode(codeToCheck);

        // Assert
        Assert.Equal(testCase.Expected, actual);
    }

    [Theory]
    [MemberData(nameof(ExceptionExtensionTestData.NullArgumentGuards.Cases), MemberType = typeof(ExceptionExtensionTestData.NullArgumentGuards))]
    public void Methods_NullException_ThrowArgumentNullException(bool _)
    {
        Exception? exception = null;

        Assert.Throws<ArgumentNullException>(() => exception!.TryGetMustCode(out var code));
        Assert.Throws<ArgumentNullException>(() => exception!.HasMustCode("sample.always-fails"));
        Assert.Throws<ArgumentNullException>(() => exception!.GetMustPropertyPath());
        Assert.Throws<ArgumentNullException>(() => new InvalidOperationException().HasMustCode(null!));
    }
}
