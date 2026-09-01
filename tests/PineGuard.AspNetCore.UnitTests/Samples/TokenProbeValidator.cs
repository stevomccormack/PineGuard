using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Records the token it is given on the probe it validates, and always succeeds.
/// </summary>
public sealed class TokenProbeValidator : IMustValidator<TokenProbe>
{
    public MustValidationResult Validate(TokenProbe value) => MustValidationResult.Ok();

    public ValueTask<MustValidationResult> ValidateAsync(TokenProbe value, CancellationToken cancellationToken = default)
    {
        value.ObservedToken = cancellationToken;

        return new ValueTask<MustValidationResult>(MustValidationResult.Ok());
    }
}
