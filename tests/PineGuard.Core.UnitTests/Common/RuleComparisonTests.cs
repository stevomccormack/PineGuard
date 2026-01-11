using System.Reflection;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class RuleComparisonTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Equality.ValidCases), MemberType = typeof(RuleComparisonTestData.Equality))]
    public void Equals_Generic_ComparesByCompareTo(RuleComparisonTestData.Equality.Case testCase)
    {
        // Act
        var result = InvokeGenericStatic<bool>("Equals", testCase.Value.Value, testCase.Value.Other);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.ValidCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.EdgeCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_HandlesInclusiveAndExclusive(RuleComparisonTestData.IsBetween.ValidCase testCase)
    {
        // Act
        var result = InvokeGenericStatic<bool>(
            "IsBetween",
            testCase.Value.Value,
            testCase.Value.Min,
            testCase.Value.Max,
            testCase.Value.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.EdgeCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_HandlesEdgeCases(RuleComparisonTestData.IsBetween.ValidCase testCase)
    {
        // Act
        var result = InvokeGenericStatic<bool>(
            "IsBetween",
            testCase.Value.Value,
            testCase.Value.Min,
            testCase.Value.Max,
            testCase.Value.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.ValidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsGreaterThan_AndIsLessThan_RespectInclusion(RuleComparisonTestData.Boundaries.ValidCase testCase)
    {
        // Act
        var greater = InvokeGenericStatic<bool>("IsGreaterThan", testCase.Value.Value, testCase.Value.Boundary, testCase.Value.Inclusion);
        var less = InvokeGenericStatic<bool>("IsLessThan", testCase.Value.Value, testCase.Value.Boundary, testCase.Value.Inclusion);

        // Assert
        Assert.Equal(testCase.ExpectedReturn.ExpectedGreaterThan, greater);
        Assert.Equal(testCase.ExpectedReturn.ExpectedLessThan, less);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.IsBetween.InvalidCases), MemberType = typeof(RuleComparisonTestData.IsBetween))]
    public void IsBetween_Throws_ForInvalidInclusion(RuleComparisonTestData.IsBetween.InvalidCase testCase)
    {
        var invalidCase = testCase;

        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>(
                "IsBetween",
                invalidCase.Value.Value,
                invalidCase.Value.Min,
                invalidCase.Value.Max,
                invalidCase.Value.Inclusion));

        Assert.NotNull(ex.InnerException);
        ThrowsCaseAssert.Expected(ex.InnerException!, invalidCase);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.InvalidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsGreaterThan_Throws_ForInvalidInclusion(RuleComparisonTestData.Boundaries.InvalidCase testCase)
    {
        var invalidCase = testCase;

        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>("IsGreaterThan", invalidCase.Value.Value, invalidCase.Value.Boundary, invalidCase.Value.Inclusion));

        Assert.NotNull(ex.InnerException);
        ThrowsCaseAssert.Expected(ex.InnerException!, invalidCase);
    }

    [Theory]
    [MemberData(nameof(RuleComparisonTestData.Boundaries.InvalidCases), MemberType = typeof(RuleComparisonTestData.Boundaries))]
    public void IsLessThan_Throws_ForInvalidInclusion(RuleComparisonTestData.Boundaries.InvalidCase testCase)
    {
        var invalidCase = testCase;

        var ex = Assert.Throws<TargetInvocationException>(() =>
            InvokeGenericStatic<bool>("IsLessThan", invalidCase.Value.Value, invalidCase.Value.Boundary, invalidCase.Value.Inclusion));

        Assert.NotNull(ex.InnerException);
        ThrowsCaseAssert.Expected(ex.InnerException!, invalidCase);
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
