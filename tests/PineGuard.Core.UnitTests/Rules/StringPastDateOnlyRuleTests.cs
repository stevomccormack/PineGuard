using PineGuard.Rules;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class StringPastDateOnlyRuleTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Evaluate.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.Evaluate))]
    public void Evaluate_BehavesAsExpected(StringPastDateOnlyRuleTestData.Evaluate.Case tc)
    {
        // Arrange
        var (value, styles, timeProvider, allowNull) = tc.Value;
        var clock = new CountingTimeProvider(timeProvider.GetUtcNow(), timeProvider.LocalTimeZone);

        // Act
        var outcome = StringPastDateOnlyRule.Evaluate(value, styles, clock, allowNull, out var parsed);

        // Assert
        Assert.Equal(tc.Expected, outcome.ToString());
        Assert.Equal(tc.ExpectedOutValue, parsed);
        Assert.Equal(tc.ExpectedClockReads, clock.UtcNowCallCount);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.EvaluateSystemClock.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.EvaluateSystemClock))]
    public void EvaluateSystemClock_BehavesAsExpected(StringPastDateOnlyRuleTestData.EvaluateSystemClock.Case tc)
    {
        // Arrange
        var (value, styles, timeProvider, allowNull) = tc.Value;

        // Act
        var outcome = StringPastDateOnlyRule.Evaluate(value, styles, timeProvider, allowNull, out var parsed);

        // Assert
        Assert.Equal(tc.Expected, outcome.ToString());
        Assert.Equal(tc.ExpectedOutValue, parsed);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Code.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.Code))]
    public void Code_BehavesAsExpected(StringPastDateOnlyRuleTestData.Code.Case tc)
    {
        // Act
        var result = StringPastDateOnlyRule.Code((StringPastDateOnlyOutcome)tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Code.InvalidCases), MemberType = typeof(StringPastDateOnlyRuleTestData.Code))]
    public void Code_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (StringPastDateOnlyRuleTestData.Code.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => StringPastDateOnlyRule.Code((StringPastDateOnlyOutcome)t.Value));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.MessageTemplate.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.MessageTemplate))]
    public void MessageTemplate_BehavesAsExpected(StringPastDateOnlyRuleTestData.MessageTemplate.Case tc)
    {
        // Act
        var result = StringPastDateOnlyRule.MessageTemplate((StringPastDateOnlyOutcome)tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.MessageTemplate.InvalidCases), MemberType = typeof(StringPastDateOnlyRuleTestData.MessageTemplate))]
    public void MessageTemplate_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (StringPastDateOnlyRuleTestData.MessageTemplate.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => StringPastDateOnlyRule.MessageTemplate((StringPastDateOnlyOutcome)t.Value));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.ParamName.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.ParamName))]
    public void ParamName_BehavesAsExpected(StringPastDateOnlyRuleTestData.ParamName.Case tc)
    {
        // Arrange
        var (outcome, paramName) = tc.Value;

        // Act
        var result = StringPastDateOnlyRule.ParamName((StringPastDateOnlyOutcome)outcome, paramName);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.ParamName.InvalidCases), MemberType = typeof(StringPastDateOnlyRuleTestData.ParamName))]
    public void ParamName_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var (outcome, paramName) = ((StringPastDateOnlyRuleTestData.ParamName.InvalidCase)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => StringPastDateOnlyRule.ParamName((StringPastDateOnlyOutcome)outcome, paramName));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Value.Cases), MemberType = typeof(StringPastDateOnlyRuleTestData.Value))]
    public void Value_BehavesAsExpected(StringPastDateOnlyRuleTestData.Value.Case tc)
    {
        // Arrange
        var (outcome, value, styles, timeProvider) = tc.Value;

        // Act
        var result = StringPastDateOnlyRule.Value((StringPastDateOnlyOutcome)outcome, value, styles, timeProvider);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringPastDateOnlyRuleTestData.Value.InvalidCases), MemberType = typeof(StringPastDateOnlyRuleTestData.Value))]
    public void Value_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var (outcome, value, styles, timeProvider) = ((StringPastDateOnlyRuleTestData.Value.InvalidCase)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => StringPastDateOnlyRule.Value((StringPastDateOnlyOutcome)outcome, value, styles, timeProvider));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
