using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class MustValidationResultExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // MustValidationResultExtension.ToValidationResults
    [Theory]
    [MemberData(nameof(MustValidationResultExtensionTestData.ToValidationResults.Cases), MemberType = typeof(MustValidationResultExtensionTestData.ToValidationResults))]
    public void ToValidationResults_BehavesAsExpected(MustValidationResultExtensionTestData.ToValidationResults.Case tc)
    {
        // Act
        var results = tc.Value.ToValidationResults().ToList();

        // Assert
        AssertResults(tc.Expected, results);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultExtensionTestData.ToValidationResults.InvalidCases), MemberType = typeof(MustValidationResultExtensionTestData.ToValidationResults))]
    public void ToValidationResults_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    // MustValidationResultExtension.ToValidationResult
    [Theory]
    [MemberData(nameof(MustValidationResultExtensionTestData.ToValidationResult.Cases), MemberType = typeof(MustValidationResultExtensionTestData.ToValidationResult))]
    public void ToValidationResult_BehavesAsExpected(MustValidationResultExtensionTestData.ToValidationResult.Case tc)
    {
        // Act
        var result = tc.Value.ToValidationResult();

        // Assert
        AssertResults(tc.Expected, [result]);
    }

    [Theory]
    [MemberData(nameof(MustValidationResultExtensionTestData.ToValidationResult.InvalidCases), MemberType = typeof(MustValidationResultExtensionTestData.ToValidationResult))]
    public void ToValidationResult_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertResults(MustValidationResultExtensionTestData.ValidationResultsExpected expected, IReadOnlyList<ValidationResult> actual)
    {
        Assert.Equal(expected.IsValid, actual.Count == 0);
        Assert.Equal(expected.Results.Count, actual.Count);

        for (var i = 0; i < expected.Results.Count; i++)
        {
            var (errorMessage, memberNames) = expected.Results[i];
            Assert.Equal(errorMessage, actual[i].ErrorMessage);
            Assert.Equal(memberNames, actual[i].MemberNames);
            Assert.DoesNotContain(MustValidationResultExtensionTestData.AttemptedValue, actual[i].ErrorMessage, StringComparison.Ordinal);
        }
    }
}
