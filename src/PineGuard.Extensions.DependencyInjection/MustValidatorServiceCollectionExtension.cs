#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection;

/// <summary>
/// Registers <see cref="IMustValidator"/> implementations in an <see cref="IServiceCollection"/> — one
/// validator named at compile time, or every validator found by scanning an assembly.
/// </summary>
/// <remarks>
/// Every overload registers a validator under three shapes: its own concrete type, each closed
/// <see cref="IMustValidator{T}"/> it implements (a single class may validate several types), and the
/// non-generic <see cref="IMustValidator"/> used for runtime dispatch by <see cref="Type"/>. All of them
/// share one instance per lifetime scope, because the interface registrations forward to the concrete one.
/// <para>
/// Registration uses <c>Add</c>, never <c>TryAdd</c>: two validators for the same <c>T</c> is a supported
/// arrangement, and <see cref="ServiceProviderExtension.GetMustValidators"/> returns all of them. The
/// consequence is that registering the same validator twice produces two sets of descriptors rather than
/// being silently ignored.
/// </para>
/// </remarks>
/// <seealso cref="ServiceProviderExtension"/>
/// <seealso cref="IMustValidator{T}"/>
public static class MustValidatorServiceCollectionExtension
{
#if NET8_0_OR_GREATER
    private const string ScanningRequiresUnreferencedCode =
        "Assembly scanning enumerates types reflectively and cannot be statically analysed. Register validators explicitly with AddMustValidator<TValidator>() in a trimmed or AOT-published application.";
#endif

    /// <summary>
    /// Registers <typeparamref name="TValidator"/> as itself, as every closed <see cref="IMustValidator{T}"/>
    /// it implements, and as <see cref="IMustValidator"/>.
    /// </summary>
    /// <typeparam name="TValidator">The concrete validator type to register.</typeparam>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="lifetime">The lifetime to register with. Use <see cref="ServiceLifetime.Scoped"/> when the validator consumes a scoped dependency.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <remarks>
    /// This is the trim-safe registration: <typeparamref name="TValidator"/> is named at compile time, so
    /// nothing about it is discovered reflectively.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddMustValidator<TValidator>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TValidator : class, IMustValidator
    {
        ThrowHelper.ThrowIfNull(services);

        return AddValidatorType(services, typeof(TValidator), lifetime);
    }

    /// <summary>
    /// Registers every validator declared in <paramref name="assembly"/>.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="lifetime">The lifetime to register each validator with.</param>
    /// <param name="filter">An optional predicate narrowing which discovered validator types are registered.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <remarks>
    /// A type qualifies when it is a non-abstract, non-open-generic class implementing at least one closed
    /// <see cref="IMustValidator{T}"/>. Abstract bases and open-generic validators are skipped because
    /// neither is a registerable service implementation.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="assembly"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode(ScanningRequiresUnreferencedCode)]
#endif
    public static IServiceCollection AddMustValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null)
    {
        ThrowHelper.ThrowIfNull(services);
        ThrowHelper.ThrowIfNull(assembly);

        foreach (var type in assembly.GetTypes())
        {
            if (!IsMustValidatorType(type))
                continue;

            if (filter is not null && !filter(type))
                continue;

            AddValidatorType(services, type, lifetime);
        }

        return services;
    }

    /// <summary>
    /// Registers every validator declared in each of <paramref name="assemblies"/>.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="assemblies">The assemblies to scan, in order.</param>
    /// <param name="lifetime">The lifetime to register each validator with.</param>
    /// <param name="filter">An optional predicate narrowing which discovered validator types are registered.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <remarks>
    /// Discovery rules are <see cref="AddMustValidatorsFromAssembly"/>'s. An assembly contributing no
    /// validators contributes no registrations; an empty sequence is not an error.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="assemblies"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode(ScanningRequiresUnreferencedCode)]
#endif
    public static IServiceCollection AddMustValidatorsFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null)
    {
        ThrowHelper.ThrowIfNull(services);
        ThrowHelper.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
            services.AddMustValidatorsFromAssembly(assembly, lifetime, filter);

        return services;
    }

    /// <summary>
    /// Registers every validator declared in the assembly containing <typeparamref name="TMarker"/>.
    /// </summary>
    /// <typeparam name="TMarker">Any type in the assembly to scan — typically the application's entry point.</typeparam>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="lifetime">The lifetime to register each validator with.</param>
    /// <param name="filter">An optional predicate narrowing which discovered validator types are registered.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <remarks>
    /// Discovery rules are <see cref="AddMustValidatorsFromAssembly"/>'s; only the way the assembly is named
    /// differs.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is <see langword="null"/>.</exception>
#if NET8_0_OR_GREATER
    [RequiresUnreferencedCode(ScanningRequiresUnreferencedCode)]
#endif
    public static IServiceCollection AddMustValidatorsFromAssemblyContaining<TMarker>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton, Func<Type, bool>? filter = null) =>
        services.AddMustValidatorsFromAssembly(typeof(TMarker).Assembly, lifetime, filter);

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="type"/> is a registerable validator implementation.
    /// </summary>
    /// <param name="type">The candidate type.</param>
    internal static bool IsMustValidatorType(Type type) =>
        type.IsClass &&
        !type.IsAbstract &&
        !type.ContainsGenericParameters &&
        Array.Exists(type.GetInterfaces(), IsMustValidatorInterface);

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="type"/> is a closed <see cref="IMustValidator{T}"/>.
    /// </summary>
    /// <param name="type">The candidate interface type.</param>
    internal static bool IsMustValidatorInterface(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMustValidator<>);

    private static IServiceCollection AddValidatorType(IServiceCollection services, Type validatorType, ServiceLifetime lifetime)
    {
        services.Add(new ServiceDescriptor(validatorType, validatorType, lifetime));

        foreach (var interfaceType in validatorType.GetInterfaces())
        {
            if (!IsMustValidatorInterface(interfaceType))
                continue;

            services.Add(new ServiceDescriptor(interfaceType, provider => provider.GetRequiredService(validatorType), lifetime));
        }

        services.Add(new ServiceDescriptor(typeof(IMustValidator), provider => provider.GetRequiredService(validatorType), lifetime));

        return services;
    }
}
