using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Collects the conventions an extension adds, and replays them onto a real <see cref="EndpointBuilder"/>
/// so a test can read the resulting filter factories.
/// </summary>
public sealed class SampleEndpointConventionBuilder : IEndpointConventionBuilder
{
    private readonly List<Action<EndpointBuilder>> _conventions = [];

    public void Add(Action<EndpointBuilder> convention) => _conventions.Add(convention);

    /// <summary>
    /// Applies every collected convention to a fresh endpoint builder and returns it.
    /// </summary>
    public EndpointBuilder Build()
    {
        var builder = new RouteEndpointBuilder(static _ => Task.CompletedTask, RoutePatternFactory.Parse("/"), order: 0);

        foreach (var convention in _conventions)
            convention(builder);

        return builder;
    }
}
