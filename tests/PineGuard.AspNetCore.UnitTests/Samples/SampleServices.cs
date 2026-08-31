using Microsoft.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Builds the request services a filter, a handler or the ProblemDetails builder expects to find.
/// </summary>
public static class SampleServices
{
    /// <summary>
    /// Builds a provider carrying logging, default <see cref="MustValidationOptions"/> and the default
    /// message resolver, plus whatever <paramref name="configure"/> adds.
    /// </summary>
    /// <param name="configure">Registers the validators — and any option override — the case needs.</param>
    public static ServiceProvider Build(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.Configure<MustValidationOptions>(_ => { });
        services.AddSingleton<IMustFailureMessageResolver, DefaultMustFailureMessageResolver>();

        configure(services);

        return services.BuildServiceProvider();
    }
}
