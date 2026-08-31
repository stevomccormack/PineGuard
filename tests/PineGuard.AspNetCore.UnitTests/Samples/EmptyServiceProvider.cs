namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// A container that answers nothing — the stand-in for a third-party provider that does not supply
/// <c>IServiceProviderIsService</c>.
/// </summary>
public sealed class EmptyServiceProvider : IServiceProvider
{
    public object? GetService(Type serviceType) => null;
}
