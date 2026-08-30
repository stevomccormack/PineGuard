using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection.UnitTests.Samples;

public sealed class ContactValidator : IMustValidator<Customer>, IMustValidator<Supplier>
{
    public MustValidationResult Validate(Customer value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(Customer value, CancellationToken cancellationToken = default) => new(Validate(value));

    public MustValidationResult Validate(Supplier value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(Supplier value, CancellationToken cancellationToken = default) => new(Validate(value));

    Type IMustValidator.ValidatedType => typeof(Customer);

    MustValidationResult IMustValidator.Validate(object? value) => MustValidationResult.Ok();

    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, CancellationToken cancellationToken) => new(MustValidationResult.Ok());

    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, MustValidationMode mode, CancellationToken cancellationToken) => new(MustValidationResult.Ok());
}
