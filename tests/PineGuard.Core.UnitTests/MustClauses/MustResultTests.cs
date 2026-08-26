using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustResultTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustResultTestData.Ok.IntValidCases), MemberType = typeof(MustResultTestData.Ok))]
    public void Ok_SetsProperties_AndImplicitBoolConversion(MustResultTestData.Ok.IntValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.Ok(testCase.Result, testCase.InputValue, testCase.ParamName);
        bool asBool = mustResult;

        // Assert
        Assert.True(mustResult.Success);
        Assert.False(mustResult.Failed);
        Assert.Equal(string.Empty, mustResult.Code);
        Assert.Equal(string.Empty, mustResult.Message);
        Assert.Equal(string.Empty, mustResult.MessageTemplate);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);
        Assert.Equal(testCase.Result, mustResult.Result);
        Assert.True(asBool);

        IMustResult asInterface = mustResult;
        Assert.Equal(testCase.Result, asInterface.Result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Ok.StringValidCases), MemberType = typeof(MustResultTestData.Ok))]
    public void Ok_AllowsNullResults_ForReferenceTypes(MustResultTestData.Ok.StringValidCase testCase)
    {
        // Act
        var mustResult = MustResult<string?>.Ok(testCase.Result, testCase.InputValue, testCase.ParamName);

        // Assert
        Assert.True(mustResult.Success);
        Assert.Equal(testCase.Result, mustResult.Result);
        Assert.Equal(testCase.InputValue, mustResult.Value);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Fail.ValidCases), MemberType = typeof(MustResultTestData.Fail))]
    public void Fail_SetsProperties_AndFormatsMessage(MustResultTestData.Fail.ValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.Fail(testCase.Template, testCase.ParamName, testCase.InputValue);
        bool asBool = mustResult;

        // Assert
        Assert.False(mustResult.Success);
        Assert.True(mustResult.Failed);
        Assert.Equal(string.Empty, mustResult.Code);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.Template, mustResult.MessageTemplate);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);
        Assert.Equal(0, mustResult.Result);
        Assert.False(asBool);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FailCoded.ValidCases), MemberType = typeof(MustResultTestData.FailCoded))]
    public void FailCoded_SetsCodeAndMessageTemplate(MustResultTestData.FailCoded.ValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.Fail(testCase.Code, testCase.Template, testCase.ParamName, testCase.InputValue);

        // Assert
        Assert.False(mustResult.Success);
        Assert.Equal(testCase.Code, mustResult.Code);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.Template, mustResult.MessageTemplate);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FailCoded.InvalidCases), MemberType = typeof(MustResultTestData.FailCoded))]
    public void FailCoded_EmptyOrNullCode_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustResultTestData.FailCoded.InvalidCase)testCase;
        var (code, template, paramName, value) = t.Value;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustResult<int>.Fail(code, template, paramName, value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Fail.ValidCases), MemberType = typeof(MustResultTestData.Fail))]
    public void Deconstruct_ExposesAllFields(MustResultTestData.Fail.ValidCase testCase)
    {
        // Arrange
        var mustResult = MustResult<int>.Fail(testCase.Template, testCase.ParamName, testCase.InputValue);

        // Act
        mustResult.Deconstruct(out var success, out var message, out var deconstructedParamName, out var deconstructedValue, out var result);

        // Assert
        Assert.Equal(mustResult.Success, success);
        Assert.Equal(testCase.ExpectedMessage, message);
        Assert.Equal(testCase.ParamName, deconstructedParamName);
        Assert.Equal(testCase.InputValue, deconstructedValue);
        Assert.Equal(mustResult.Result, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBoolWithResult.ValidCases), MemberType = typeof(MustResultTestData.FromBoolWithResult))]
    public void FromBool_WithResult_ReturnsOkOrFail(MustResultTestData.FromBoolWithResult.ValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.FromBool(testCase.IsOk, testCase.Template, testCase.ParamName, testCase.InputValue, testCase.Result);

        // Assert
        Assert.Equal(testCase.IsSuccess, mustResult.Success);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);

        Assert.Equal(testCase.IsSuccess ? testCase.Result : 0, mustResult.Result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBoolWithoutResult.ValidCases), MemberType = typeof(MustResultTestData.FromBoolWithoutResult))]
    public void FromBool_WithoutResult_ReturnsOkOrFail(MustResultTestData.FromBoolWithoutResult.ValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.FromBool(testCase.IsOk, testCase.Template, testCase.ParamName, testCase.InputValue);

        // Assert
        Assert.Equal(testCase.IsSuccess, mustResult.Success);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);
        Assert.Equal(0, mustResult.Result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.FromBoolCoded.ValidCases), MemberType = typeof(MustResultTestData.FromBoolCoded))]
    public void FromBool_Coded_ReturnsOkOrFail(MustResultTestData.FromBoolCoded.ValidCase testCase)
    {
        // Act
        var mustResult = MustResult<int>.FromBool(testCase.IsOk, testCase.Code, testCase.Template, testCase.ParamName, testCase.InputValue, testCase.Result);

        // Assert
        Assert.Equal(testCase.IsSuccess, mustResult.Success);
        Assert.Equal(testCase.IsSuccess ? string.Empty : testCase.Code, mustResult.Code);
        Assert.Equal(testCase.ExpectedMessage, mustResult.Message);
        Assert.Equal(testCase.ParamName, mustResult.ParamName);
        Assert.Equal(testCase.InputValue, mustResult.Value);
        Assert.Equal(testCase.IsSuccess ? testCase.Result : 0, mustResult.Result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ThrowIfFailedInvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_ThrowsArgumentException_WhenFailed(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => invalidCase.MustResult.ThrowIfFailed());

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ThrowNullIfFailedInvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowNullIfFailed_ThrowsArgumentNullException_WhenFailed(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => invalidCase.MustResult.ThrowNullIfFailed());

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowNullIfFailed_DoesNotThrow_WhenSuccessful(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        testCase.MustResult.ThrowNullIfFailed();

        // Assert
        Assert.True(testCase.MustResult.Success);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ThrowIfFailedGenericInvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Generic_UsesExceptionFactory(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => invalidCase.MustResult.ThrowIfFailed(ExceptionFactory));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
        return;

        static InvalidOperationException ExceptionFactory(string message, string? paramName) =>
            new($"{paramName}:{message}");
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.NullFactoryCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Generic_NullExceptionFactory_ThrowsArgumentNullException(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => testCase.MustResult.ThrowIfFailed((Func<string, string?, InvalidOperationException>)null!));

        // Assert
        Assert.Equal("exceptionFactory", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Generic_DoesNotThrow_WhenSuccessful(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        testCase.MustResult.ThrowIfFailed(ExceptionFactory);

        // Assert
        Assert.True(testCase.MustResult.Success);
        return;

        // Arrange
        static InvalidOperationException ExceptionFactory(string message, string? paramName) =>
            new($"{paramName}:{message}");
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ThrowIfFailedResultInvalidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_ResultFactory_UsesFullResult(MustResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => testCase.MustResult.ThrowIfFailed(ResultExceptionFactory));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
        return;

        static InvalidOperationException ResultExceptionFactory(IMustResult result) =>
            new($"{result.Code}:{result.Message}");
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_ResultFactory_DoesNotThrow_WhenSuccessful(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        testCase.MustResult.ThrowIfFailed(ResultExceptionFactory);

        // Assert
        Assert.True(testCase.MustResult.Success);
        return;

        static InvalidOperationException ResultExceptionFactory(IMustResult result) =>
            new($"{result.Code}:{result.Message}");
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.NullFactoryCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_ResultFactory_NullExceptionFactory_ThrowsArgumentNullException(MustResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => testCase.MustResult.ThrowIfFailed((Func<IMustResult, InvalidOperationException>)null!));

        // Assert
        Assert.Equal("exceptionFactory", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ThrowIfFailed.ImplicitBoolCases), MemberType = typeof(MustResultTestData.ThrowIfFailed))]
    public void ImplicitBool_IsNullSafe(MustResultTestData.ThrowIfFailed.ImplicitBoolCase testCase)
    {
        // Act
        bool asBool = testCase.MustResult;

        // Assert
        Assert.Equal(testCase.Expected, asBool);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.ValidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_ReturnsResult_WhenSuccessful(MustResultTestData.OrThrow.ValidCase testCase)
    {
        // Act
        Assert.NotNull(testCase.Value);
        var result = testCase.Value.OrThrow();

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.InvalidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_Throws_WhenFailed(PineGuard.Testing.Common.IThrowsCase testCase)
    {
        var t = (MustResultTestData.OrThrow.InvalidCase)testCase;

        // Act
        Assert.NotNull(t.Value);
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = t.Value.OrThrow());

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.NullResultCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_ReturnsNull_WhenSuccessfulWithNoResult(MustResultTestData.OrThrow.NullResultCase testCase)
    {
        // Act
        var result = testCase.Value.OrThrow();

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.OrThrow.FallbackValidCases), MemberType = typeof(MustResultTestData.OrThrow))]
    public void OrThrow_WithFallback_ReturnsFallbackOnlyWhenResultIsNull(MustResultTestData.OrThrow.FallbackValidCase testCase)
    {
        // Act
        var result = testCase.Value.MustResult.OrThrow(testCase.Value.Fallback);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Combine.NullCases), MemberType = typeof(MustResultTestData.Combine))]
    public void Combine_ReturnsFail_WhenResultsIsNull(MustResultTestData.Combine.NullCase testCase)
    {
        // Act
        var combined = testCase.Results!.Combine();

        // Assert
        Assert.Equal(testCase.Expected, combined.Success);
        Assert.Equal("results", combined.ParamName);
        Assert.NotEmpty(combined.Message);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Combine.ValidCases), MemberType = typeof(MustResultTestData.Combine))]
    public void Combine_ReturnsExpected_WhenResultsProvided(MustResultTestData.Combine.ValidCase testCase)
    {
        // Act
        var combined = testCase.Value.Combine();

        var expected = testCase.Expected;

        // Assert
        Assert.Equal(expected.Success, combined.Success);
        Assert.Equal(expected.Result, combined.Result);
        Assert.Equal(expected.Value, combined.Value);
        Assert.Equal(expected.ParamName, combined.ParamName);

        if (expected.ExpectedMessage is not null)
        {
            Assert.Equal(expected.ExpectedMessage, combined.Message);
        }

        foreach (var messagePart in expected.MessageContains)
        {
            Assert.Contains(messagePart, combined.Message, StringComparison.Ordinal);
        }
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.CombineCoded.ValidCases), MemberType = typeof(MustResultTestData.CombineCoded))]
    public void Combine_CarriesFirstFailureCodeAndMessageTemplate(MustResultTestData.CombineCoded.ValidCase testCase)
    {
        // Act
        var combined = testCase.Value.Combine();

        // Assert
        Assert.Equal(testCase.Expected.ExpectedCode, combined.Code);
        Assert.Equal(testCase.Expected.ExpectedMessageTemplate, combined.MessageTemplate);
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

    [Theory]
    [MemberData(nameof(MustResultTestData.AndThen.ValidCases), MemberType = typeof(MustResultTestData.AndThen))]
    public void AndThen_ChainsOnSuccess_PropagatesOnFailure(MustResultTestData.AndThen.ValidCase testCase)
    {
        // Act
        var chained = testCase.Value.AndThen(v => MustResult<string>.Ok((v * 2).ToString()));

        // Assert
        Assert.Equal(testCase.Expected.Success, chained.Success);
        Assert.Equal(testCase.Expected.Result, chained.Result);
        Assert.Equal(testCase.Expected.ExpectedCode, chained.Code);
        Assert.Equal(testCase.Expected.ExpectedMessageTemplate, chained.MessageTemplate);
        Assert.Equal(testCase.Expected.ExpectedParamName, chained.ParamName);
        Assert.Equal(testCase.Expected.ExpectedValue, chained.Value);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.When.Cases), MemberType = typeof(MustResultTestData.When))]
    public void When_KeepsOrDropsFailure(MustResultTestData.When.Case testCase)
    {
        // Act
        var result = testCase.Value.When(testCase.Condition);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.Unless.Cases), MemberType = typeof(MustResultTestData.Unless))]
    public void Unless_KeepsOrDropsFailure(MustResultTestData.Unless.Case testCase)
    {
        // Act
        var result = testCase.Value.Unless(testCase.Condition);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.ToMustValidationResult.Cases), MemberType = typeof(MustResultTestData.ToMustValidationResult))]
    public void ToMustValidationResult_LiftsResultLosslessly(MustResultTestData.ToMustValidationResult.Case testCase)
    {
        // Act
        var validationResult = testCase.Value.ToMustValidationResult(testCase.PropertyPath);

        // Assert
        Assert.Equal(testCase.ExpectedSuccess, validationResult.Success);
        Assert.Equal(testCase.ExpectedFailureCount, validationResult.Failures.Count);

        if (testCase.ExpectedPropertyPath is null)
            return;

        Assert.Equal(testCase.ExpectedPropertyPath, validationResult.Failures[0].PropertyPath);
        Assert.Equal(testCase.ExpectedMessage, validationResult.Failures[0].Message);
    }

    [Theory]
    [MemberData(nameof(MustResultTestData.NullArgumentGuards.Cases), MemberType = typeof(MustResultTestData.NullArgumentGuards))]
    public void NullArguments_ThrowArgumentNullException(MustResultTestData.NullArgumentGuards.Case testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, testCase.Value);

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
