using PineGuard.MustClauses;

namespace PineGuard.Core.UnitTests.MustClauses.Samples;

public sealed class OrderLineValidator : MustValidator<OrderLine>
{
    public OrderLineValidator()
    {
        RuleFor(x => x.Sku, sku => string.IsNullOrWhiteSpace(sku)
            ? MustResult<string>.Fail("sample.sku.blank", "{paramName} must not be null or whitespace.", nameof(sku), sku)
            : MustResult<string>.Ok(sku, sku, nameof(sku)));

        RuleFor(x => x.Quantity, quantity => quantity > 0
            ? MustResult<int>.Ok(quantity, quantity, nameof(quantity))
            : MustResult<int>.Fail("sample.quantity.not-positive", "{paramName} must be positive.", nameof(quantity), quantity));
    }
}
