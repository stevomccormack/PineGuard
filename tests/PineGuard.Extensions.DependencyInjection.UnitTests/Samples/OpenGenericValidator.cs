using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection.UnitTests.Samples;

public sealed class OpenGenericValidator<TValue> : IMustValidator<TValue>
    where TValue : notnull
{
    public MustValidationResult Validate(TValue value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(TValue value, CancellationToken cancellationToken = default) => new(Validate(value));
}
