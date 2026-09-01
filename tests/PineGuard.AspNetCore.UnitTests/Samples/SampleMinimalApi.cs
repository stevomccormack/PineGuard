using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The Minimal API application of Plan 03's stories 2 and 8 — the one a consumer writes after reading the
/// README, mapped exactly as the README maps it.
/// </summary>
/// <remarks>
/// Validators are registered one by one rather than by scanning this assembly, because the assembly also
/// holds validators built to fail every value (<see cref="CreateOrderConsistencyValidator"/>) for the sake
/// of other cases; an end-to-end application registers only what its stories need.
/// </remarks>
/// <seealso cref="MustValidationEndpointFilter"/>
public static class SampleMinimalApi
{
    /// <summary>
    /// Registers request validation and the two validators the mapped endpoints run.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddMustValidation();
        services.AddMustValidator<CreateOrderValidator>();
        services.AddMustValidator<SearchQueryValidator>();
    }

    /// <summary>
    /// Maps one endpoint per shape the filter has to get right: a validated body, a validated
    /// <c>[AsParameters]</c> query, an endpoint with nothing to validate, and a whole group turned on at
    /// once.
    /// </summary>
    /// <param name="app">The application to map the endpoints on.</param>
    public static void Map(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapPost("/orders", (CreateOrder order) => TypedResults.Ok(order.Email)).AddMustValidation();
        app.MapPost("/customers", (Customer customer) => TypedResults.Ok(customer.Name)).AddMustValidation();
        app.MapGet("/search", ([AsParameters] SearchQuery query) => TypedResults.Ok(query.Term)).AddMustValidation();

        app.MapGroup("/api").AddMustValidation().MapPost("/orders", (CreateOrder order) => TypedResults.Ok(order.Email));
    }
}
