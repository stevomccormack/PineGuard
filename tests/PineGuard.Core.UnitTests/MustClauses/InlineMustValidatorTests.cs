using PineGuard.Core.UnitTests.MustClauses.Samples;
using PineGuard.MustClauses;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.MustClauses;

public sealed class InlineMustValidatorTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForSingle.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForSingle))]
    public void RuleFor_SingleArgument_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<OrderLine>();
        validator.RuleFor(x => x.Sku, sku => string.IsNullOrWhiteSpace(sku)
            ? MustResult<string>.Fail("sample.sku.blank", "{paramName} must not be blank.", "sku", sku)
            : MustResult<string>.Ok(sku));

        // Act
        var result = validator.Validate(new OrderLine(null, 1));

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Sku", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForCrossProperty.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForCrossProperty))]
    public void RuleFor_CrossProperty_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<OrderLine>();
        validator.RuleFor(x => x.Quantity, (line, quantity) => quantity > 0 && line.Sku is not null
            ? MustResult<int>.Ok(quantity)
            : MustResult<int>.Fail("sample.quantity.invalid", "{paramName} must be positive when Sku is set.", "quantity", quantity));

        // Act
        var result = validator.Validate(new OrderLine("SKU-1", 0));

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Quantity", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForValidator.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForValidator))]
    public void RuleFor_NestedValidator_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<InlineMustValidatorTestData.OrderWithSingleLine>();
        validator.RuleFor(x => x.Line, new OrderLineValidator());

        // Act
        var missingSkuResult = validator.Validate(new InlineMustValidatorTestData.OrderWithSingleLine(new OrderLine(null, 1)));
        var nullLineResult = validator.Validate(new InlineMustValidatorTestData.OrderWithSingleLine(null));

        // Assert
        Assert.False(missingSkuResult.Success);
        Assert.Equal("Line.Sku", missingSkuResult.Failures[0].PropertyPath);
        Assert.True(nullLineResult.Success);
    }

    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForEachSingle.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForEachSingle))]
    public void RuleForEach_SingleArgument_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (OrderLine line) => line.Quantity > 0
            ? MustResult<int>.Ok(line.Quantity)
            : MustResult<int>.Fail("sample.quantity.not-positive", "{paramName} must be positive.", "quantity", line.Quantity));

        // Act
        var result = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 0)]));

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Lines[0]", result.Failures[0].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForEachCrossProperty.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForEachCrossProperty))]
    public void RuleForEach_CrossProperty_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, (order, line) => line.Quantity >= order.Lines!.Count
            ? MustResult<int>.Ok(line.Quantity)
            : MustResult<int>.Fail("sample.quantity.below-minimum", "{paramName} must be at least the line count.", "quantity", line.Quantity));

        // Act
        var result = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine("SKU-1", 1), new OrderLine("SKU-2", 1)]));

        // Assert
        Assert.False(result.Success);
        Assert.Equal(2, result.Failures.Count);
        Assert.Equal("Lines[0]", result.Failures[0].PropertyPath);
        Assert.Equal("Lines[1]", result.Failures[1].PropertyPath);
    }

    [Theory]
    [MemberData(nameof(InlineMustValidatorTestData.RuleForEachValidator.Cases), MemberType = typeof(InlineMustValidatorTestData.RuleForEachValidator))]
    public void RuleForEach_NestedValidator_Forwarder_Works(bool _)
    {
        // Arrange
        var validator = new InlineMustValidator<CreateOrder>();
        validator.RuleForEach(x => x.Lines, new OrderLineValidator());

        // Act
        var result = validator.Validate(new CreateOrder(null, DateTime.MinValue, DateTime.MinValue, false, 0m, [new OrderLine(null, 1)]));

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Lines[0].Sku", result.Failures[0].PropertyPath);
    }
}
