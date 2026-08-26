using PineGuard.Core.UnitTests.MustClauses.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class MustPropertyRuleTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustPropertyRuleTestData.PropertyPath.Cases), MemberType = typeof(MustPropertyRuleTestData.PropertyPath))]
    public void PropertyPath_ReflectsExpressionDerivedPath(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<OrderLine>();

        // Act
        var rule = validator.RuleFor(x => x.Sku, sku => MustResult<string>.Ok(sku));

        // Assert
        Assert.Equal("Sku", rule.PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustPropertyRuleTestData.FluentBuilders.Cases), MemberType = typeof(MustPropertyRuleTestData.FluentBuilders))]
    public void Builders_ReturnSameInstance_ForChaining(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<OrderLine>();
        var rule = validator.RuleFor(x => x.Sku, sku => MustResult<string>.Ok(sku));

        // Act
        var afterWhen = rule.When(_ => true);
        var afterUnless = rule.Unless(_ => false);
        var afterCode = rule.WithCode("sample.sku.custom");
        var afterMessage = rule.WithMessage("{paramName} custom.");
        var afterPath = rule.WithPropertyPath("Custom.Path");

        // Assert
        Assert.Same(rule, afterWhen);
        Assert.Same(rule, afterUnless);
        Assert.Same(rule, afterCode);
        Assert.Same(rule, afterMessage);
        Assert.Same(rule, afterPath);
    }

    [Theory]
    [MemberData(nameof(MustPropertyRuleTestData.Overrides.Cases), MemberType = typeof(MustPropertyRuleTestData.Overrides))]
    public void WithCode_WithMessage_WithPropertyPath_OverrideFailure(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<OrderLine>();
        validator.RuleFor(x => x.Sku, sku => MustResult<string>.Fail("sample.sku.blank", "{paramName} must not be blank.", "sku", sku))
            .WithCode("custom.code")
            .WithMessage("{paramName} was overridden.")
            .WithPropertyPath("CustomPath");

        // Act
        var result = validator.Validate(new OrderLine(null, 1));

        // Assert
        var failure = Assert.Single(result.Failures);
        Assert.Equal("custom.code", failure.Code);
        Assert.Equal("CustomPath was overridden.", failure.Message);
        Assert.Equal("CustomPath", failure.PropertyPath);
    }

    [Theory]
    [MemberData(nameof(MustPropertyRuleTestData.NullArgumentGuards.Cases), MemberType = typeof(MustPropertyRuleTestData.NullArgumentGuards))]
    public void NullArguments_ThrowExpectedException(MustPropertyRuleTestData.NullArgumentGuards.Case testCase)
    {
        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, testCase.Value);

        // Assert
        ThrowsCaseAssert.Expected(ex, testCase);
    }
}
