using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

/// <summary>
/// A validator that records the <see cref="CancellationToken"/> it was handed, so a test can assert the
/// token reached every validator the behaviour ran.
/// </summary>
public abstract class RecordingValidator : IMustValidator<CreateOrder>
{
    public CancellationToken ReceivedCancellationToken { get; private set; }

    public abstract MustValidationResult Validate(CreateOrder value);

    public ValueTask<MustValidationResult> ValidateAsync(CreateOrder value, CancellationToken cancellationToken = default)
    {
        ReceivedCancellationToken = cancellationToken;
        return new ValueTask<MustValidationResult>(Validate(value));
    }
}
