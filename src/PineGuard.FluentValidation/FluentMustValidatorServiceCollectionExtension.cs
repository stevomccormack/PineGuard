using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Registers a <see cref="FluentMustValidator{T}"/> for every FluentValidation validator already in the
/// container, so an application that owns <see cref="IValidator{T}"/> registrations gets the matching
/// <see cref="IMustValidator{T}"/> registrations without writing either of them twice.
/// </summary>
/// <remarks>
/// <para>
/// This is the wiring half of the migration story <see cref="FluentMustValidator{T}"/> tells: register
/// FluentValidation's validators the way the team already does, add one line, and every PineGuard seam that
/// resolves <see cref="IMustValidator{T}"/> — options binding, request filters, mediator pipelines — sees
/// them.
/// </para>
/// </remarks>
/// <seealso cref="FluentMustValidator{T}"/>
/// <seealso cref="IMustValidator{T}"/>
public static class FluentMustValidatorServiceCollectionExtension
{
    /// <summary>
    /// Adapts every registered FluentValidation validator into an <see cref="IMustValidator{T}"/> registration.
    /// </summary>
    /// <param name="services">The service collection to scan and add to.</param>
    /// <returns><paramref name="services"/>, so calls can be chained.</returns>
    /// <remarks>
    /// <para>
    /// Only closed <see cref="IValidator{T}"/> service registrations are adapted. A validator registered
    /// solely as its own concrete type is invisible here — FluentValidation's own
    /// <c>AddValidatorsFromAssembly</c> registers both, so the common path is covered — and an open-generic
    /// <c>IValidator&lt;&gt;</c> registration is skipped because there is no single type to close
    /// <see cref="FluentMustValidator{T}"/> over.
    /// </para>
    /// <para>
    /// Each adapter is registered with the lifetime of the FluentValidation registration it wraps, and takes
    /// its <see cref="IValidator{T}"/> by constructor injection — so a validator that depends on a scoped
    /// service keeps working, and the container's usual last-registration-wins rule decides which validator
    /// an adapter wraps when several are registered for the same type.
    /// </para>
    /// <para>
    /// Registration is additive but not duplicating: a hand-written <see cref="IMustValidator{T}"/> for the
    /// same type is left in place and both run, while calling this method twice adds nothing the second time.
    /// </para>
    /// <para>
    /// Call this after the FluentValidation registrations, because it reads the collection as it stands.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// services.AddValidatorsFromAssemblyContaining&lt;Program&gt;();
    /// services.AddMustValidatorsFromFluentValidators();
    /// </code>
    /// </example>
    public static IServiceCollection AddMustValidatorsFromFluentValidators(this IServiceCollection services)
    {
        ThrowHelper.ThrowIfNull(services);

        foreach (var descriptor in services.ToArray())
        {
            var serviceType = descriptor.ServiceType;
            if (!serviceType.IsConstructedGenericType || serviceType.GetGenericTypeDefinition() != typeof(IValidator<>))
                continue;

            var validatedType = serviceType.GenericTypeArguments[0];
            var mustValidatorType = typeof(IMustValidator<>).MakeGenericType(validatedType);
            var adapterType = typeof(FluentMustValidator<>).MakeGenericType(validatedType);

            services.TryAddEnumerable(ServiceDescriptor.Describe(mustValidatorType, adapterType, descriptor.Lifetime));
        }

        return services;
    }
}
