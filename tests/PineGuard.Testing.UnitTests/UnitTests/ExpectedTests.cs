using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class ExpectedTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(ExpectedTestData.RuleExpectedOps.ValidCases), MemberType = typeof(ExpectedTestData.RuleExpectedOps))]
    public void RuleExpected_SetsIsValid(ExpectedTestData.RuleExpectedOps.Case testCase)
    {
        var (isValid, expectedIsValid) = testCase.Value;

        var result = new RuleExpected(isValid);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [MemberData(nameof(ExpectedTestData.MustExpectedOps.ValidCases), MemberType = typeof(ExpectedTestData.MustExpectedOps))]
    [MemberData(nameof(ExpectedTestData.MustExpectedOps.EdgeCases), MemberType = typeof(ExpectedTestData.MustExpectedOps))]
    public void MustExpected_SetsAllProperties(ExpectedTestData.MustExpectedOps.Case testCase)
    {
        var (isValid, message, paramName) = testCase.Value;

        var result = new MustExpected(isValid, message, paramName);

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(message, result.Message);
        Assert.Equal(paramName, result.ParamName);
    }

    [Theory]
    [MemberData(nameof(ExpectedTestData.GuardExpectedOps.ValidCases), MemberType = typeof(ExpectedTestData.GuardExpectedOps))]
    public void GuardExpected_SetsAllProperties(ExpectedTestData.GuardExpectedOps.Case testCase)
    {
        var (isValid, exType, paramName, msgContains) = testCase.Value;

        var result = new GuardExpected(isValid, exType, paramName, msgContains);

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(exType, result.ExceptionType);
        Assert.Equal(paramName, result.ParamName);
        Assert.Equal(msgContains, result.MessageContains);
    }

    [Theory]
    [MemberData(nameof(ExpectedTestData.FluentExpectedOps.ValidCases), MemberType = typeof(ExpectedTestData.FluentExpectedOps))]
    public void FluentExpected_SetsAllProperties(ExpectedTestData.FluentExpectedOps.Case testCase)
    {
        var (isValid, message, propertyName) = testCase.Value;

        var result = new FluentExpected(isValid, message, propertyName);

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(message, result.Message);
        Assert.Equal(propertyName, result.PropertyName);
    }

    [Theory]
    [MemberData(nameof(ExpectedTestData.DataAnnotationExpectedOps.ValidCases), MemberType = typeof(ExpectedTestData.DataAnnotationExpectedOps))]
    public void DataAnnotationExpected_SetsAllProperties(ExpectedTestData.DataAnnotationExpectedOps.Case testCase)
    {
        var (isValid, message, memberName) = testCase.Value;

        var result = new DataAnnotationExpected(isValid, message, memberName);

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(message, result.Message);
        Assert.Equal(memberName, result.MemberName);
    }

    [Theory]
    [MemberData(nameof(ExpectedTestData.HierarchyOps.ValidCases), MemberType = typeof(ExpectedTestData.HierarchyOps))]
    public void AllExpected_ImplementIExpectedResult(ExpectedTestData.HierarchyOps.Case testCase)
    {
        var (expected, expectedIsValid) = testCase.Value;

        Assert.Equal(expectedIsValid, expected.IsValid);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MustExpected_IsReturnExpected(bool isValid)
    {
        ReturnExpected result = new MustExpected(isValid, "msg");

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal("msg", result.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FluentExpected_IsReturnExpected(bool isValid)
    {
        ReturnExpected result = new FluentExpected(isValid, "msg");

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal("msg", result.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DataAnnotationExpected_IsReturnExpected(bool isValid)
    {
        ReturnExpected result = new DataAnnotationExpected(isValid, "msg");

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal("msg", result.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GuardExpected_IsThrowExpected(bool isValid)
    {
        ThrowExpected result = new GuardExpected(isValid, typeof(ArgumentException), "p", "contains");

        Assert.Equal(isValid, result.IsValid);
        Assert.Equal(typeof(ArgumentException), result.ExceptionType);
        Assert.Equal("p", result.ParamName);
        Assert.Equal("contains", result.MessageContains);
    }
}
