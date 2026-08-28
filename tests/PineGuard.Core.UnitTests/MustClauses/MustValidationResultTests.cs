using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustValidationResultTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationResultTestData.Ok.Cases), MemberType = typeof(MustValidationResultTestData.Ok))]
    public void Ok_ReturnsSharedSuccessfulSingleton(MustValidationResultTestData.Ok.Case testCase)
    {
        // Arrange
        WriteLine(testCase.Name);

        // Act
        var first = MustValidationResult.Ok();
        var second = MustValidationResult.Ok();

        // Assert
        Assert.Same(first, second);
        Assert.True(first.Success);
        Assert.False(first.Failed);
        Assert.Empty(first.Failures);
        Assert.Equal(string.Empty, first.Message);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.Fail.ValidCases), MemberType = typeof(MustValidationResultTestData.Fail))]
    public void Fail_CombinesFailureAndAdditional(MustValidationResultTestData.Fail.ValidCase testCase)
    {
        // Act
        var result = MustValidationResult.Fail(testCase.Failure, testCase.Additional);

        // Assert
        Assert.False(result.Success);
        Assert.True(result.Failed);
        Assert.Equal(testCase.Expected, result.Failures);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.Fail.InvalidCases), MemberType = typeof(MustValidationResultTestData.Fail))]
    public void Fail_NullArguments_Throw(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.Fail.InvalidCase)testCase;
        var (failure, additional) = t.Value;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.Fail(failure, additional));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.FailEnumerable.ValidCases), MemberType = typeof(MustValidationResultTestData.FailEnumerable))]
    public void Fail_Enumerable_UsesProvidedSequence(MustValidationResultTestData.FailEnumerable.ValidCase testCase)
    {
        // Act
        var result = MustValidationResult.Fail(testCase.Failures);

        // Assert
        Assert.Equal(testCase.Expected, result.Failures);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.FailEnumerable.InvalidCases), MemberType = typeof(MustValidationResultTestData.FailEnumerable))]
    public void Fail_Enumerable_NullOrEmpty_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.FailEnumerable.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.Fail(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.From.ValidCases), MemberType = typeof(MustValidationResultTestData.From))]
    public void From_Array_KeepsOnlyFailures(MustValidationResultTestData.From.ValidCase testCase)
    {
        // Act
        var result = MustValidationResult.From(testCase.Results);

        // Assert
        Assert.Equal(testCase.ExpectedPropertyPaths, result.Failures.Select(f => f.PropertyPath));

        if (testCase.ExpectedPropertyPaths.Length == 0)
            Assert.Same(MustValidationResult.Ok(), result);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.From.InvalidCases), MemberType = typeof(MustValidationResultTestData.From))]
    public void From_Array_NullResults_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.From.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.From(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.FromEnumerable.ValidCases), MemberType = typeof(MustValidationResultTestData.FromEnumerable))]
    public void From_Enumerable_KeepsOnlyFailures(MustValidationResultTestData.FromEnumerable.ValidCase testCase)
    {
        // Act
        var result = MustValidationResult.From(testCase.Results);

        // Assert
        Assert.Equal(testCase.ExpectedPropertyPaths, result.Failures.Select(f => f.PropertyPath));
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.FromEnumerable.InvalidCases), MemberType = typeof(MustValidationResultTestData.FromEnumerable))]
    public void From_Enumerable_NullResults_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.FromEnumerable.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.From(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.Combine.ValidCases), MemberType = typeof(MustValidationResultTestData.Combine))]
    public void Combine_Array_MergesEveryFailure(MustValidationResultTestData.Combine.ValidCase testCase)
    {
        // Act
        var combined = MustValidationResult.Combine(testCase.Results);

        // Assert
        Assert.Equal(testCase.Expected, combined.Failures);

        if (testCase.Expected.Length == 0)
            Assert.Same(MustValidationResult.Ok(), combined);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.Combine.InvalidCases), MemberType = typeof(MustValidationResultTestData.Combine))]
    public void Combine_Array_NullResults_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.Combine.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.Combine(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.CombineEnumerable.ValidCases), MemberType = typeof(MustValidationResultTestData.CombineEnumerable))]
    public void Combine_Enumerable_MergesEveryFailure(MustValidationResultTestData.CombineEnumerable.ValidCase testCase)
    {
        // Act
        var combined = MustValidationResult.Combine(testCase.Results);

        // Assert
        Assert.Equal(testCase.Expected, combined.Failures);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.CombineEnumerable.InvalidCases), MemberType = typeof(MustValidationResultTestData.CombineEnumerable))]
    public void Combine_Enumerable_NullResults_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.CombineEnumerable.InvalidCase)testCase;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => MustValidationResult.Combine(t.Value));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.WithPropertyPathPrefix.ValidCases), MemberType = typeof(MustValidationResultTestData.WithPropertyPathPrefix))]
    public void WithPropertyPathPrefix_ReRootsFailures_OrReturnsSameInstanceOnSuccess(MustValidationResultTestData.WithPropertyPathPrefix.ValidCase testCase)
    {
        // Act
        var result = testCase.Result.WithPropertyPathPrefix(testCase.Prefix);

        // Assert
        if (testCase.Expected.ExpectedSameInstance)
        {
            Assert.Same(testCase.Result, result);
            return;
        }

        Assert.Equal(testCase.Expected.ExpectedPropertyPaths, result.Failures.Select(f => f.PropertyPath));
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.WithPropertyPathPrefix.InvalidCases), MemberType = typeof(MustValidationResultTestData.WithPropertyPathPrefix))]
    public void WithPropertyPathPrefix_NullPrefix_Throws(IThrowsCase testCase)
    {
        // Arrange
        var t = (MustValidationResultTestData.WithPropertyPathPrefix.InvalidCase)testCase;
        var (result, prefix) = t.Value;

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => result.WithPropertyPathPrefix(prefix));

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.ThrowIfFailed.ValidCases), MemberType = typeof(MustValidationResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_DoesNotThrow_WhenSuccessful(MustValidationResultTestData.ThrowIfFailed.ValidCase testCase)
    {
        // Act
        testCase.Value.ThrowIfFailed();

        // Assert
        Assert.True(testCase.Value.Success);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.ThrowIfFailed.InvalidCases), MemberType = typeof(MustValidationResultTestData.ThrowIfFailed))]
    public void ThrowIfFailed_Throws_MustValidationException_CarryingResult(MustValidationResultTestData.ThrowIfFailed.InvalidCase testCase)
    {
        // Act
        var ex = Assert.Throws<MustValidationException>(() => testCase.Value.ThrowIfFailed());

        // Assert
        Assert.Same(testCase.Value, ex.Result);
        Assert.Equal(testCase.Value.Message, ex.Message);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.ImplicitBool.Cases), MemberType = typeof(MustValidationResultTestData.ImplicitBool))]
    public void ImplicitBool_IsNullSafe(MustValidationResultTestData.ImplicitBool.Case testCase)
    {
        // Act
        bool asBool = testCase.Value;

        // Assert
        Assert.Equal(testCase.Expected, asBool);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultTestData.MessageFormatting.Cases), MemberType = typeof(MustValidationResultTestData.MessageFormatting))]
    public void Message_FormatsFailures_WithOrWithoutPropertyPath(MustValidationResultTestData.MessageFormatting.Case testCase)
    {
        // Act & Assert
        Assert.Equal(testCase.Expected, testCase.Value.Message);
        Assert.Equal(testCase.Expected, testCase.Value.ToString());
    }
}
