using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests;

public sealed class CaseRecordTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CaseRecordTestData.RuleCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.RuleCaseOps))]
    public void RuleCase_SetsProperties(CaseRecordTestData.RuleCaseOps.Case testCase)
    {
        var (value, isValid) = testCase.Value;

        var ruleCase = new RuleCase<string?>("test", value, new RuleExpected(isValid));

        Assert.Equal("test", ruleCase.Name);
        Assert.Equal(value, ruleCase.Value);
        Assert.Equal(isValid, ruleCase.Expected.IsValid);
        Assert.IsType<IReturnsCase<RuleExpected>>(ruleCase, exactMatch: false);
    }

    [Theory]
    [MemberData(nameof(CaseRecordTestData.MustCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.MustCaseOps))]
    public void MustCase_SetsProperties(CaseRecordTestData.MustCaseOps.Case testCase)
    {
        var (value, isValid, message) = testCase.Value;

        var mustCase = new MustCase<string?>("test", value, new MustExpected(isValid, message));

        Assert.Equal(value, mustCase.Value);
        Assert.Equal(isValid, mustCase.Expected.IsValid);
        Assert.Equal(message, mustCase.Expected.Message);
        Assert.IsType<IReturnsCase<MustExpected>>(mustCase, exactMatch: false);
    }

    [Theory]
    [MemberData(nameof(CaseRecordTestData.GuardCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.GuardCaseOps))]
    public void GuardCase_SetsProperties(CaseRecordTestData.GuardCaseOps.Case testCase)
    {
        var (value, isValid, exType) = testCase.Value;

        var guardCase = new GuardCase<string?>("test", value, new GuardExpected(isValid, exType));

        Assert.Equal(value, guardCase.Value);
        Assert.Equal(isValid, guardCase.Expected.IsValid);
        Assert.Equal(exType, guardCase.Expected.ExceptionType);
        Assert.IsType<IReturnsCase<GuardExpected>>(guardCase, exactMatch: false);
    }

    [Theory]
    [MemberData(nameof(CaseRecordTestData.FluentCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.FluentCaseOps))]
    public void FluentCase_SetsProperties(CaseRecordTestData.FluentCaseOps.Case testCase)
    {
        var (value, isValid, message) = testCase.Value;

        var fluentCase = new FluentCase<string?>("test", value, new FluentExpected(isValid, message));

        Assert.Equal(value, fluentCase.Value);
        Assert.Equal(isValid, fluentCase.Expected.IsValid);
        Assert.Equal(message, fluentCase.Expected.Message);
        Assert.IsType<IReturnsCase<FluentExpected>>(fluentCase, exactMatch: false);
    }

    [Theory]
    [MemberData(nameof(CaseRecordTestData.DataAnnotationCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.DataAnnotationCaseOps))]
    public void DataAnnotationCase_SetsProperties(CaseRecordTestData.DataAnnotationCaseOps.Case testCase)
    {
        var (value, isValid, message) = testCase.Value;

        var daCase = new DataAnnotationCase("test", value, new DataAnnotationExpected(isValid, message));

        Assert.Equal(value, daCase.Value);
        Assert.Equal(isValid, daCase.Expected.IsValid);
        Assert.Equal(message, daCase.Expected.Message);
        Assert.IsType<IReturnsCase<DataAnnotationExpected>>(daCase, exactMatch: false);
    }

    [Theory]
    [MemberData(nameof(CaseRecordTestData.MustValidationCaseOps.ValidCases), MemberType = typeof(CaseRecordTestData.MustValidationCaseOps))]
    public void MustValidationCase_SetsProperties(CaseRecordTestData.MustValidationCaseOps.Case testCase)
    {
        var (value, isValid, failureCount) = testCase.Value;

        var validationCase = new MustValidationCase<string?>("test", value, new MustValidationExpected(isValid, FailureCount: failureCount));

        Assert.Equal(value, validationCase.Value);
        Assert.Equal(isValid, validationCase.Expected.IsValid);
        Assert.Equal(failureCount, validationCase.Expected.FailureCount);
        Assert.IsType<IReturnsCase<MustValidationExpected>>(validationCase, exactMatch: false);
    }

    [Theory]
    [InlineData("my-case")]
    [InlineData("")]
    public void RuleCase_ToStringReturnsName(string name)
    {
        var ruleCase = new RuleCase<string?>(name, "x", new RuleExpected(true));

        Assert.Equal(name, ruleCase.ToString());
    }
}
