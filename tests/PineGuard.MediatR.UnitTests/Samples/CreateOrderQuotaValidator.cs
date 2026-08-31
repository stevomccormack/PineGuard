using PineGuard.Codes;
using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

public sealed class CreateOrderQuotaValidator : RecordingValidator
{
    public override MustValidationResult Validate(CreateOrder value) =>
        value?.Quantity > 0
            ? MustValidationResult.Ok()
            : MustValidationResult.Fail(new MustFailure(nameof(CreateOrder.Quantity), MustCodes.Number.Sign.Positive, "Quantity must be positive.", value?.Quantity));
}
