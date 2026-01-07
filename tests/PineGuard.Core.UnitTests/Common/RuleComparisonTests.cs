using System.Reflection;
using PineGuard.Common;
using PineGuard.Testing;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class RuleComparisonTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Equality.ValidCases), MemberType = typeof(RuleComparisonTestData.Equality))]
    public void Equals_Generic_ComparesByCompareTo(RuleComparisonTestData.Equality.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = InvokeGenericStatic<bool>("Equals", testCase.Value, testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.ValidCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.EdgeCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_HandlesInclusiveAndExclusive(RuleComparisonTestData.IsBetween.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = InvokeGenericStatic<bool>(
            "IsBetween",
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.EdgeCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_HandlesEdgeCases(RuleComparisonTestData.IsBetween.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = InvokeGenericStatic<bool>(
            "IsBetween",
            testCase.Value,
            testCase.Min,
            testCase.Max,
            testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.ValidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsGreaterThan_AndIsLessThan_RespectInclusion(RuleComparisonTestData.Boundaries.ValidCase testCase)
    {
        // Arrange

        // Act
        var greater = InvokeGenericStatic<bool>("IsGreaterThan", testCase.Value, testCase.Boundary, testCase.Inclusion);
        var less = InvokeGenericStatic<bool>("IsLessThan", testCase.Value, testCase.Boundary, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedGreaterThan, greater);
        Assert.Equal(testCase.ExpectedLessThan, less);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.InvalidCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_Throws_ForInvalidInclusion(RuleComparisonTestData.IsBetween.InvalidCase testCase)
    {
        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>("IsBetween", testCase.Value, testCase.Min, testCase.Max, testCase.Inclusion));

        AssertExpectedInnerException(ex, testCase.ExpectedException);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.InvalidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsGreaterThan_Throws_ForInvalidInclusion(RuleComparisonTestData.Boundaries.InvalidCase testCase)
    {
        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>("IsGreaterThan", testCase.Value, testCase.Boundary, testCase.Inclusion));

        AssertExpectedInnerException(ex, testCase.ExpectedException);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.InvalidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsLessThan_Throws_ForInvalidInclusion(RuleComparisonTestData.Boundaries.InvalidCase testCase)
    {
        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>("IsLessThan", testCase.Value, testCase.Boundary, testCase.Inclusion));

        AssertExpectedInnerException(ex, testCase.ExpectedException);
    }

    private static void AssertExpectedInnerException(TargetInvocationException ex, ExpectedException expected)
    {
        Assert.NotNull(ex.InnerException);
        Assert.IsType(expected.Type, ex.InnerException);

        if (expected.ParamName is not null)
        {
            Assert.Equal(expected.ParamName, ((ArgumentOutOfRangeException)ex.InnerException!).ParamName);
        }

        if (expected.MessageContains is not null)
        {
            Assert.Contains(expected.MessageContains, ex.InnerException!.Message);
        }
    }

    private static TResult InvokeGenericStatic<TResult>(string methodName, params object[] args)
    {
        var ruleComparisonType = typeof(Inclusion).Assembly.GetType("PineGuard.Common.RuleComparison", throwOnError: true)!;

        var method = ruleComparisonType
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(m => string.Equals(m.Name, methodName, StringComparison.Ordinal) && m.IsGenericMethodDefinition);

        var constructed = method.MakeGenericMethod(typeof(int));
        var value = constructed.Invoke(null, args);
        return (TResult)value!;
    }
}
