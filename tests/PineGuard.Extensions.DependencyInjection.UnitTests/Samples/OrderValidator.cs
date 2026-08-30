using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection.UnitTests.Samples;

public sealed class OrderValidator : IMustValidator<Order>
{
    public MustValidationResult Validate(Order value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(Order value, CancellationToken cancellationToken = default) => new(Validate(value));
}
