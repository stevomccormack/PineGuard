using PineGuard.Codes;
using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

public sealed class CreateOrderValidator : RecordingValidator
{
    public override MustValidationResult Validate(CreateOrder value) =>
        string.IsNullOrWhiteSpace(value?.Sku)
            ? MustValidationResult.Fail(new MustFailure(nameof(CreateOrder.Sku), MustCodes.Text.Content.Blank, "Sku must not be null or whitespace.", value?.Sku))
            : MustValidationResult.Ok();
}
