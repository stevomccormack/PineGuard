using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection.UnitTests.Samples;

public abstract class AbstractOrderValidator : IMustValidator<Order>
{
    public MustValidationResult Validate(Order value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(Order value, CancellationToken cancellationToken = default) => new(Validate(value));
}
