using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Fails an order with anything but <see cref="CreateOrder.ValidEmail"/>, reporting exactly the two failures
/// Plan 03's story-2 body publishes.
/// </summary>
public sealed class CreateOrderValidator : IMustValidator<CreateOrder>
{
    public MustValidationResult Validate(CreateOrder value) =>
        value.Email == CreateOrder.ValidEmail
            ? MustValidationResult.Ok()
            : MustValidationResult.Fail(SampleFailures.Email, SampleFailures.LineSku);

    public ValueTask<MustValidationResult> ValidateAsync(CreateOrder value, CancellationToken cancellationToken = default) => new(Validate(value));
}
