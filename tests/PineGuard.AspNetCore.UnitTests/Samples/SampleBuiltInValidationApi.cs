#if NET10_0_OR_GREATER
#pragma warning disable ASP0029
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The application of Plan 03's story 4 — PineGuard validators running inside .NET 10's own
/// <c>Microsoft.Extensions.Validation</c> pipeline rather than inside a PineGuard filter.
/// </summary>
/// <remarks>
/// No endpoint here calls <c>AddMustValidation()</c>: validation is attached by the platform once
/// <c>AddValidation</c> is registered, which is the whole point of the story — the resolver participates in
/// Microsoft's pipeline instead of running beside it.
/// <para>
/// The body this path publishes is the built-in one, so it differs from the filters' in two ways a case
/// here pins down: keys are the declared property paths rather than the application's JSON spelling,
/// because the naming policy is applied by the filters and no filter is in this pipeline; and the body is a
/// bare <c>HttpValidationProblemDetails</c> — a title and a dictionary of messages, with no status, no type
/// and no <c>failures</c> extension, because a dictionary of messages has nowhere to put a code.
/// </para>
/// </remarks>
/// <seealso cref="ValidationOptionsExtension"/>
public static class SampleBuiltInValidationApi
{
    /// <summary>
    /// Registers the validators and hands PineGuard's resolver to the built-in pipeline.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddMustValidation();
        services.AddMustValidator<CreateOrderValidator>();
        services.AddValidation(options => options.AddMustValidatorResolver());
    }

    /// <summary>
    /// Maps a validated endpoint and one whose argument type no validator is registered for.
    /// </summary>
    /// <param name="app">The application to map the endpoints on.</param>
    public static void Map(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapPost("/validated/orders", (CreateOrder order) => TypedResults.Ok(order.Email));
        app.MapPost("/validated/customers", (Customer customer) => TypedResults.Ok(customer.Name));
    }
}
#pragma warning restore ASP0029
#endif
