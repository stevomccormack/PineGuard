using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Turns PineGuard validation on for a Minimal API endpoint, or for every endpoint in a group.
/// </summary>
/// <remarks>
/// Assumes <c>services.AddMustValidation(...)</c> has already registered the options, the message resolver
/// and the validators; this extension only decides which endpoints run them.
/// </remarks>
/// <seealso cref="MustValidationEndpointFilter"/>
/// <seealso cref="MustValidationServiceCollectionExtension"/>
public static class EndpointConventionBuilderExtension
{
    /// <summary>
    /// Validates every bound argument of the endpoints <paramref name="builder"/> describes before their
    /// handlers run.
    /// </summary>
    /// <typeparam name="TBuilder">The endpoint builder being configured — a single endpoint, or a whole <c>MapGroup</c>.</typeparam>
    /// <param name="builder">The builder to add the filter factory to.</param>
    /// <returns><paramref name="builder"/>, for further chaining.</returns>
    /// <remarks>
    /// The filter is attached at build time only to endpoints declaring at least one parameter that has a
    /// registered validator, so applying this to an entire group costs the group's unvalidated endpoints
    /// nothing at run time.
    /// </remarks>
    /// <example>
    /// <code>
    /// app.MapPost("/orders", (CreateOrder order) => TypedResults.Created($"/orders/{order.Id}"))
    ///    .AddMustValidation();
    ///
    /// app.MapGroup("/api").AddMustValidation();
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    public static TBuilder AddMustValidation<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        ThrowHelper.ThrowIfNull(builder);

        return builder.AddEndpointFilterFactory(CreateFilter);
    }

    /// <summary>
    /// Builds the filter for one endpoint, or hands <paramref name="next"/> back untouched when the endpoint
    /// has nothing to validate.
    /// </summary>
    /// <param name="context">The endpoint being built, whose <see cref="EndpointFilterFactoryContext.MethodInfo"/> declares the parameters.</param>
    /// <param name="next">The pipeline the filter would wrap.</param>
    /// <returns><paramref name="next"/> itself when no parameter type has a validator; otherwise a delegate running <see cref="MustValidationEndpointFilter"/> first.</returns>
    internal static EndpointFilterDelegate CreateFilter(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        if (!HasValidatedParameter(context))
            return next;

        var filter = new MustValidationEndpointFilter();

        return invocationContext => filter.InvokeAsync(invocationContext, next);
    }

    /// <summary>
    /// Returns <see langword="true"/> when at least one of the endpoint's parameters has a registered
    /// validator.
    /// </summary>
    /// <param name="context">The endpoint being built.</param>
    /// <remarks>
    /// The question is answered with <see cref="IServiceProviderIsService"/>, which reports a registration
    /// without constructing it — a scoped validator must not be instantiated from the application's root
    /// provider merely to find out that it exists. A container that cannot answer the question is treated as
    /// though every parameter were validated, so the filter runs and decides at request time.
    /// </remarks>
    private static bool HasValidatedParameter(EndpointFilterFactoryContext context)
    {
        var isService = context.ApplicationServices.GetService<IServiceProviderIsService>();

        if (isService is null)
            return true;

        return Array.Exists(context.MethodInfo.GetParameters(), parameter => isService.IsService(typeof(IMustValidator<>).MakeGenericType(parameter.ParameterType)));
    }
}
