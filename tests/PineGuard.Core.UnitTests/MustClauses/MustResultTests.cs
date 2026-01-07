using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustResultTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustResultTestData.Ok.IntValidCases), MemberType = typeof(MustResultTestData.Ok))]
    public void Ok_SetsProperties_AndImplicitBoolConversion(MustResultTestData.Ok.IntValidCase testCase)
    {
        // Arrange

        // Act
        var mustResult = MustResult<int>.Ok(testCase.Result, testCase.Value, testCase.ParamName);
        bool asBool = mustResult;

        // Assert
        Assert.True(mustResult.Success);
        Assert.False(mustResult.Failed);
        Assert.Equal(string.Empty, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.Value, mustResult.Value);
        Assert.Equal(testCase.Result, mustResult.Result);
        Assert.True(asBool);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Ok.StringValidCases), MemberType = typeof(MustResultTestData.Ok))]
    public void Ok_AllowsNullResults_ForReferenceTypes(MustResultTestData.Ok.StringValidCase testCase)
    {
        // Arrange

        // Act
        var mustResult = MustResult<string?>.Ok(testCase.Result, testCase.Value, testCase.ParamName);

        // Assert
        Assert.True(mustResult.Success);
        Assert.Equal(testCase.Result, mustResult.Result);
        Assert.Equal(testCase.Value, mustResult.Value);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Fail.ValidCases), MemberType = typeof(MustResultTestData.Fail))]
    public void Fail_SetsProperties_AndFormatsMessage(MustResultTestData.Fail.ValidCase testCase)
    {
        // Arrange

        // Act
        var mustResult = MustResult<int>.Fail(testCase.Template, testCase.ParamName, testCase.Value);
        bool asBool = mustResult;

        // Assert
        Assert.False(mustResult.Success);
        Assert.True(mustResult.Failed);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.Value, mustResult.Value);
        Assert.Equal(default(int), mustResult.Result);
        Assert.False(asBool);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Fail.ValidCases), MemberType = typeof(MustResultTestData.Fail))]
    public void Deconstruct_ExposesAllFields(MustResultTestData.Fail.ValidCase testCase)
    {
        // Arrange
        var mustResult = MustResult<int>.Fail(testCase.Template, testCase.ParamName, testCase.Value);

        // Act
        mustResult.Deconstruct(out var success, out var message, out var deconstructedParamName, out var deconstructedValue, out var result);

        // Assert
        Assert.Equal(mustResult.Success, success);
        Assert.Equal(testCase.ExpectedMessage, message);
        Assert.Equal(testCase.ParamName, deconstructedParamName);
        Assert.Equal(testCase.Value, deconstructedValue);
        Assert.Equal(mustResult.Result, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBoolWithResult.ValidCases), MemberType = typeof(MustResultTestData.FromBoolWithResult))]
    public void FromBool_WithResult_ReturnsOkOrFail(MustResultTestData.FromBoolWithResult.ValidCase testCase)
    {
        // Arrange

        // Act
        var mustResult = MustResult<int>.FromBool(testCase.Ok, testCase.Template, testCase.ParamName, testCase.Value, testCase.Result);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, mustResult.Success);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.Value, mustResult.Value);

        if (testCase.ExpectedSuccess)
        {
            Assert.Equal(testCase.Result, mustResult.Result);
        }
        else
        {
            Assert.Equal(default(int), mustResult.Result);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBoolWithoutResult.ValidCases), MemberType = typeof(MustResultTestData.FromBoolWithoutResult))]
    public void FromBool_WithoutResult_ReturnsOkOrFail(MustResultTestData.FromBoolWithoutResult.ValidCase testCase)
    {
        // Arrange

        // Act
        var mustResult = MustResult<int>.FromBool(testCase.Ok, testCase.Template, testCase.ParamName, testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, mustResult.Success);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.Value, mustResult.Value);

        if (testCase.ExpectedSuccess)
        {
            Assert.Equal(default(int), mustResult.Result);
        }
        else
        {
            Assert.Equal(default(int), mustResult.Result);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_ThrowsArgumentException_WhenFailed(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentException>(() => testCase.MustResult.ThrowIfFailed());

        // Assert
        Assert.Equal(testCase.MustResult.ParamName, ex.ParamName);
        Assert.Contains(testCase.MustResult.Message, ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowNullIfFailed_ThrowsArgumentNullException_WhenFailed(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => testCase.MustResult.ThrowNullIfFailed());

        // Assert
        Assert.Equal(testCase.MustResult.ParamName, ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowNullIfFailed_DoesNotThrow_WhenSuccessful(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Arrange

        // Act
        testCase.MustResult.ThrowNullIfFailed();

        // Assert
        Assert.True(testCase.MustResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.InvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Generic_UsesExceptionFactory(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange
        static InvalidOperationException ExceptionFactory(string message, string? paramName) =>
            new($"{paramName}:{message}");

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => testCase.MustResult.ThrowIfFailed(ExceptionFactory));

        // Assert
        Assert.Contains(testCase.MustResult.Message, ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Generic_DoesNotThrow_WhenSuccessful(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Arrange
        static InvalidOperationException ExceptionFactory(string message, string? paramName) =>
            new($"{paramName}:{message}");

        // Act
        testCase.MustResult.ThrowIfFailed(ExceptionFactory);

        // Assert
        Assert.True(testCase.MustResult.Success);
    }

    [Fact]
    public void OrThrow_ReturnsResult_WhenSuccessful()
    {
        // Arrange
        var mustResult = MustResult<int>.Ok(7, value: "original", paramName: "value");

        // Act
        var result = mustResult.OrThrow();

        // Assert
        Assert.Equal(7, result);
    }

    [Fact]
    public void OrThrow_Throws_WhenFailed()
    {
        // Arrange
        var mustResult = MustResult<int>.Fail("{paramName} must be valid.", "value", 123);

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _ = mustResult.OrThrow());

        // Assert
        Assert.Equal("value", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.FallbackValidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_WithFallback_ReturnsFallbackOnlyWhenResultIsNull(MustResultTestData.OrThrow.FallbackValidCase testCase)
    {
        // Arrange

        // Act
        var result = testCase.MustResult.OrThrow(testCase.Fallback);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Combine.NullCases), MemberType = typeof(MustResultTestData.Combine))]
    public void Combine_ReturnsFail_WhenResultsIsNull(MustResultTestData.Combine.NullCase testCase)
    {
        // Arrange

        // Act
        var combined = testCase.Results!.Combine();

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, combined.Success);
        Assert.Equal("results", combined.ParamName);
        Assert.NotEmpty(combined.Message);
    }

    [Fact]
    public void Combine_ReturnsFirstSuccessfulResult_WhenNoFailures()
    {
        // Arrange
        var first = MustResult<int>.Ok(1, value: "v1", paramName: "p1");
        var second = MustResult<int>.Ok(2, value: "v2", paramName: "p2");

        // Act
        var combined = new[] { first, second }.Combine();

        // Assert
        Assert.True(combined.Success);
        Assert.Equal(1, combined.Result);
        Assert.Equal("v1", combined.Value);
        Assert.Equal("p1", combined.ParamName);
    }

    [Fact]
    public void Combine_ReturnsDefaultOk_WhenEmpty()
    {
        // Arrange
        var results = Array.Empty<MustResult<int>>();

        // Act
        var combined = results.Combine();

        // Assert
        Assert.True(combined.Success);
        Assert.Equal(0, combined.Result);
        Assert.Null(combined.Value);
    }

    [Fact]
    public void Combine_JoinsFailureMessages_AndUsesFirstFailureParamName()
    {
        // Arrange
        var ok = MustResult<int>.Ok(1, value: "v1", paramName: "p1");
        var fail1 = MustResult<int>.Fail("{paramName} failed.", "a", 1);
        var fail2 = MustResult<int>.Fail("{paramName} failed.", "b", 2);

        // Act
        var combined = new[] { ok, fail1, fail2 }.Combine();

        // Assert
        Assert.False(combined.Success);
        Assert.Equal("a", combined.ParamName);
        Assert.Contains("a failed.", combined.Message, StringComparison.Ordinal);
        Assert.Contains("b failed.", combined.Message, StringComparison.Ordinal);
        Assert.Contains("; ", combined.Message, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfAnyFailed.Cases), MemberType = typeof(MustResultTestData.ThrowIfAnyFailed))]
    public void ThrowIfAnyFailed_ThrowsOnlyWhenAnyFailed(MustResultTestData.ThrowIfAnyFailed.Case testCase)
    {
        // Arrange
        var results = testCase.AnyFailed
            ? new[] { MustResult<int>.Ok(1), MustResult<int>.Fail("{paramName} failed.", "x", 1) }
            : new[] { MustResult<int>.Ok(1), MustResult<int>.Ok(2) };

        // Act
        if (testCase.AnyFailed)
        {
            Assert.Throws<ArgumentException>(() => results.ThrowIfAnyFailed());
        }
        else
        {
            results.ThrowIfAnyFailed();
        }

        // Assert
        Assert.True(results.Length >= 2);
    }
}
