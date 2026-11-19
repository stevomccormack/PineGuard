using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FluentValidation;
using PineGuard.DataAnnotations;
using PineGuard.FluentValidation;
using PineGuard.GuardClauses;
using PineGuard.MustClauses;
using PineGuard.Rules;
using Xunit.Abstractions;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace PineGuard.Testing.UnitTests.Rules.Conformance;

public sealed class StringPastDateOnlyRuleTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class PastDateOnlyValidator : AbstractValidator<Model>
    {
        public PastDateOnlyValidator(TimeProvider? timeProvider, DateTimeStyles styles) => RuleFor(x => x.Value).PastDateOnly(timeProvider, styles);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Evaluate.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.Evaluate))]
    public void Evaluate_BehavesAsExpected(ConformanceCase<(string? value, DateTimeStyles styles, TimeProvider? timeProvider)> tc)
    {
        // Arrange
        var (value, styles, timeProvider) = tc.Value;
        var paramName = StringPastDateOnlyRuleTestData.Evaluate.ParamName;
        var validator = new PastDateOnlyValidator(timeProvider, styles);
        var attribute = new PastDateOnlyStringAttribute { Styles = styles };
        var context = new ValidationContext(new object()) { MemberName = StringPastDateOnlyRuleTestData.Evaluate.MemberName };
        context.InitializeServiceProvider(serviceType => serviceType == typeof(TimeProvider) ? timeProvider : null);
        var guardResult = default(DateOnly);

        // Act
        var outcome = StringPastDateOnlyRule.Evaluate(value, styles, timeProvider, false, out var parsed);
        var outcomeAllowNull = StringPastDateOnlyRule.Evaluate(value, styles, timeProvider, true, out var parsedAllowNull);
        var mustResult = Must.Be.PastDateOnly(value, styles, timeProvider, paramName: paramName);
        var guardException = Record.Exception(() => guardResult = Guard.Against.FutureOrPresentDateOnly(value, styles, timeProvider, paramName: paramName));
        var fluentResult = validator.Validate(new Model { Value = value });
        var annotationResult = attribute.GetValidationResult(value, context);

        // Assert
        AssertRule(tc.Expected, tc.Expected.IsValid, outcome, parsed, tc.Value);
        AssertRule(tc.Expected, tc.Expected.IsValidAllowNull, outcomeAllowNull, parsedAllowNull, tc.Value);
        AssertMust(outcome, parsed, tc.Value, mustResult);
        AssertGuard(tc.Expected, outcome, parsed, guardException, guardResult);
        AssertFluentValidation(tc.Expected, outcomeAllowNull, fluentResult);
        AssertDataAnnotations(tc.Expected, outcomeAllowNull, annotationResult, attribute.Code);
    }

    private static void AssertRule(ConformanceExpected expected, bool isValid, StringPastDateOnlyOutcome outcome, DateOnly parsed, (string? value, DateTimeStyles styles, TimeProvider? timeProvider) inputs)
    {
        Assert.Equal(isValid, StringPastDateOnlyRule.IsPassed(outcome));
        Assert.Equal(expected.ProjectedCode, StringPastDateOnlyRule.Code(StringPastDateOnlyOutcome.Passed));
        Assert.Equal(expected.ParamName, StringPastDateOnlyRule.ParamName(outcome, StringPastDateOnlyRuleTestData.Evaluate.ParamName));
        Assert.Equal(expected.Value, StringPastDateOnlyRule.Value(outcome, inputs.value, inputs.styles, inputs.timeProvider));

        if (isValid)
        {
            Assert.Equal(expected.Result, parsed);
            return;
        }

        Assert.Equal(expected.Code, StringPastDateOnlyRule.Code(outcome));
        Assert.Equal(expected.MessageTemplate, StringPastDateOnlyRule.MessageTemplate(outcome));
    }

    private static void AssertMust(StringPastDateOnlyOutcome outcome, DateOnly parsed, (string? value, DateTimeStyles styles, TimeProvider? timeProvider) inputs, MustResult<DateOnly> result)
    {
        var paramName = StringPastDateOnlyRule.ParamName(outcome, StringPastDateOnlyRuleTestData.Evaluate.ParamName);
        var value = StringPastDateOnlyRule.Value(outcome, inputs.value, inputs.styles, inputs.timeProvider);

        Assert.Equal(StringPastDateOnlyRule.IsPassed(outcome), result.Success);
        Assert.Equal(paramName, result.ParamName);
        Assert.Equal(value, result.Value);
        if (value is not DateTimeStyles)
            Assert.Same(value, result.Value);

        if (StringPastDateOnlyRule.IsPassed(outcome))
        {
            Assert.Equal(parsed, result.Result);
            Assert.Empty(result.Code);
            Assert.Empty(result.MessageTemplate);
            Assert.Empty(result.Message);
            return;
        }

        Assert.Equal(default, result.Result);
        Assert.Equal(StringPastDateOnlyRule.Code(outcome), result.Code);
        Assert.Equal(StringPastDateOnlyRule.MessageTemplate(outcome), result.MessageTemplate);
        Assert.Equal(Render(StringPastDateOnlyRule.MessageTemplate(outcome), paramName!), result.Message);
    }

    private static void AssertGuard(ConformanceExpected expected, StringPastDateOnlyOutcome outcome, DateOnly parsed, Exception? exception, DateOnly result)
    {
        if (StringPastDateOnlyRule.IsPassed(outcome))
        {
            Assert.Null(exception);
            Assert.Equal(parsed, result);
            return;
        }

        var paramName = StringPastDateOnlyRule.ParamName(outcome, StringPastDateOnlyRuleTestData.Evaluate.ParamName);

        Assert.NotNull(exception);
        Assert.IsType(expected.ExceptionType!, exception);
        Assert.Equal(paramName, ((ArgumentException)exception).ParamName);
        Assert.Contains(Render(StringPastDateOnlyRule.MessageTemplate(outcome), paramName!), exception.Message, StringComparison.Ordinal);
        Assert.Equal(StringPastDateOnlyRule.Code(outcome), exception.Data[GuardFailure.CodeDataKey]);
        Assert.Equal(paramName, exception.Data[GuardFailure.PropertyPathDataKey]);
    }

    private static void AssertFluentValidation(ConformanceExpected expected, StringPastDateOnlyOutcome outcomeAllowNull, FluentValidationResult result)
    {
        Assert.Equal(StringPastDateOnlyRule.IsPassed(outcomeAllowNull), result.IsValid);

        if (StringPastDateOnlyRule.IsPassed(outcomeAllowNull))
        {
            Assert.Empty(result.Errors);
            return;
        }

        var memberName = StringPastDateOnlyRuleTestData.Evaluate.MemberName;
        var failure = Assert.Single(result.Errors);

        Assert.Equal(Render(StringPastDateOnlyRule.MessageTemplate(outcomeAllowNull), StringPastDateOnlyRule.ParamName(outcomeAllowNull, null) ?? memberName), failure.ErrorMessage);
        Assert.Equal(memberName, failure.PropertyName);
        Assert.Equal(expected.ProjectedCode, failure.ErrorCode);
        Assert.Equal(StringPastDateOnlyRule.Code(StringPastDateOnlyOutcome.Passed), failure.ErrorCode);
    }

    private static void AssertDataAnnotations(ConformanceExpected expected, StringPastDateOnlyOutcome outcomeAllowNull, ValidationResult? result, string attributeCode)
    {
        Assert.Equal(expected.ProjectedCode, attributeCode);
        Assert.Equal(StringPastDateOnlyRule.Code(StringPastDateOnlyOutcome.Passed), attributeCode);
        Assert.Equal(StringPastDateOnlyRule.IsPassed(outcomeAllowNull), result == ValidationResult.Success);

        if (StringPastDateOnlyRule.IsPassed(outcomeAllowNull))
            return;

        var memberName = StringPastDateOnlyRuleTestData.Evaluate.MemberName;

        Assert.Equal(Render(StringPastDateOnlyRule.MessageTemplate(outcomeAllowNull), StringPastDateOnlyRule.ParamName(outcomeAllowNull, null) ?? memberName), result!.ErrorMessage);
        Assert.Equal([memberName], result.MemberNames);
    }

    private static string Render(string template, string name) =>
        template.Replace(StringPastDateOnlyRuleTestData.ParamNameToken, name, StringComparison.Ordinal);
}
