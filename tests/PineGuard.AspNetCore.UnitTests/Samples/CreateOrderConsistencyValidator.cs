using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// A second validator for <see cref="CreateOrder"/> — the whole-order rule — reporting a failure with no
/// property path at all.
/// </summary>
/// <remarks>
/// Two validators for one type is supported by design, so a case that registers this one alongside
/// <see cref="CreateOrderValidator"/> proves both that every registered validator runs and that a
/// root-level failure is published under the path the caller is currently at.
/// </remarks>
public sealed class CreateOrderConsistencyValidator : IMustValidator<CreateOrder>
{
    public MustValidationResult Validate(CreateOrder value) => MustValidationResult.Fail(SampleFailures.Root);

    public ValueTask<MustValidationResult> ValidateAsync(CreateOrder value, CancellationToken cancellationToken = default) => new(Validate(value));
}
