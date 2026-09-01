namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// An argument that records the cancellation token its validator was handed, so a test can prove the
/// request's own token is the one observed.
/// </summary>
/// <remarks>
/// The record lives on the argument rather than on the validator so that concurrently running theories
/// never share it.
/// </remarks>
public sealed class TokenProbe
{
    public CancellationToken ObservedToken { get; set; }
}
