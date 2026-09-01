using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Starts a real ASP.NET Core application on an in-memory transport, so a test can send an actual request
/// through the actual pipeline and assert on the bytes that come back.
/// </summary>
/// <remarks>
/// <see cref="TestServer"/> replaces the transport and nothing else: routing, model binding, the filter
/// pipeline, the exception-handler middleware and JSON serialisation are the application's own. That is
/// what an end-to-end case proves and a <c>DefaultHttpContext</c> case cannot — a filter that builds a
/// correct <c>ValidationProblemDetails</c> can still be attached to the wrong endpoints, or have its body
/// re-serialised into a different shape on the way out.
/// </remarks>
public static class SampleHost
{
    /// <summary>
    /// Builds, configures and starts an application served by <see cref="TestServer"/>.
    /// </summary>
    /// <param name="configureServices">Registers what the application under test needs.</param>
    /// <param name="configureApplication">Builds the middleware pipeline and maps the endpoints the case exercises.</param>
    /// <returns>The started application; the caller disposes it.</returns>
    /// <remarks>
    /// The application name is pinned to this assembly because the entry assembly of a test run is the test
    /// host, and MVC discovers its controllers through that name. The environment is pinned to Production so
    /// the exception-handler middleware never adds developer detail to a body a test is asserting on.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configureServices"/> or <paramref name="configureApplication"/> is <see langword="null"/>.</exception>
    public static async Task<WebApplication> StartAsync(Action<IServiceCollection> configureServices, Action<WebApplication> configureApplication)
    {
        ArgumentNullException.ThrowIfNull(configureServices);
        ArgumentNullException.ThrowIfNull(configureApplication);

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(SampleHost).Assembly.GetName().Name,
            ContentRootPath = AppContext.BaseDirectory,
            EnvironmentName = Environments.Production
        });

        builder.Logging.ClearProviders();
        builder.WebHost.UseTestServer();

        configureServices(builder.Services);

        var app = builder.Build();

        configureApplication(app);

        await app.StartAsync();

        return app;
    }

    /// <summary>
    /// Builds a request that accepts both JSON shapes an answer can arrive in.
    /// </summary>
    /// <param name="method">The request's method.</param>
    /// <param name="requestUri">The request's path and query.</param>
    /// <param name="json">The request body, or <see langword="null"/> for a request that has none.</param>
    /// <returns>The request to send.</returns>
    /// <remarks>
    /// The problem-details writer the exception-handler middleware falls back to only answers a client that
    /// accepts <c>application/json</c> or <c>application/problem+json</c>, so both are asked for: an
    /// unacceptable request would otherwise surface as a rethrown exception rather than the status a test
    /// means to assert.
    /// </remarks>
    public static HttpRequestMessage Request(HttpMethod method, string requestUri, string? json)
    {
        var request = new HttpRequestMessage(method, requestUri);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/problem+json"));

        if (json is not null)
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        return request;
    }
}
