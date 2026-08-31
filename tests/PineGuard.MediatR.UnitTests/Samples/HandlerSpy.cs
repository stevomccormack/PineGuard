namespace PineGuard.MediatR.UnitTests.Samples;

/// <summary>
/// Stands in for the rest of the pipeline: its <see cref="HandleAsync"/> converts to
/// <c>RequestHandlerDelegate&lt;Guid&gt;</c> and records whether — and with which token — it was reached.
/// </summary>
public sealed class HandlerSpy(Guid response)
{
    public int InvocationCount { get; private set; }

    public CancellationToken ReceivedCancellationToken { get; private set; }

    public Task<Guid> HandleAsync(CancellationToken cancellationToken)
    {
        InvocationCount++;
        ReceivedCancellationToken = cancellationToken;
        return Task.FromResult(response);
    }
}
