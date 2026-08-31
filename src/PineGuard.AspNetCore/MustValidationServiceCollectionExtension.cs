using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PineGuard.Common;
using PineGuard.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore;

/// <summary>
/// Registers everything PineGuard's request validation needs in one call: the options, the validators, the
/// message resolver and the boundary exception handler.
/// </summary>
/// <remarks>
/// This is the registration both entry points assume has run —
/// <see cref="EndpointConventionBuilderExtension.AddMustValidation{TBuilder}"/> and
/// <see cref="MvcBuilderExtension.AddMustValidation"/> only decide *where* validation happens, never what is
/// available to run.
/// </remarks>
/// <seealso cref="MustValidationOptions"/>
/// <seealso cref="MustValidationExceptionHandler"/>
public static class MustValidationServiceCollectionExtension
{
    private const string ScanningRequiresUnreferencedCode =
        "Assembly scanning enumerates types reflectively and cannot be statically analysed. Register validators explicitly with AddMustValidator<TValidator>() in a trimmed or AOT-published application.";

    /// <summary>
    /// Registers PineGuard's request validation with every option at its default, scanning
    /// <paramref name="validatorAssemblies"/> for validators.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="validatorAssemblies">The assemblies to scan for <c>IMustValidator&lt;T&gt;</c> implementations.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <example>
    /// <code>
    /// builder.Services.AddMustValidation(typeof(Program).Assembly);
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="validatorAssemblies"/> is <see langword="null"/>.</exception>
    [RequiresUnreferencedCode(ScanningRequiresUnreferencedCode)]
    public static IServiceCollection AddMustValidation(this IServiceCollection services, params Assembly[] validatorAssemblies) =>
        services.AddMustValidation(static _ => { }, validatorAssemblies);

    /// <summary>
    /// Registers PineGuard's request validation, configured by <paramref name="configure"/>, scanning
    /// <paramref name="validatorAssemblies"/> for validators.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="configure">Configures the application's <see cref="MustValidationOptions"/>.</param>
    /// <param name="validatorAssemblies">The assemblies to scan for <c>IMustValidator&lt;T&gt;</c> implementations.</param>
    /// <returns><paramref name="services"/>, for further chaining.</returns>
    /// <remarks>
    /// Validators are registered as singletons. A validator that consumes a scoped dependency — an async
    /// rule backed by a database, typically — is registered separately with
    /// <c>services.AddMustValidatorsFromAssemblyContaining&lt;Program&gt;(ServiceLifetime.Scoped)</c>.
    /// <para>
    /// The message resolver is added with <c>TryAdd</c>, so registering
    /// <see cref="StringLocalizerMustFailureMessageResolver"/> — or any other — before this call keeps it.
    /// The exception handler is registered here but only runs once the application calls
    /// <c>app.UseExceptionHandler()</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddMustValidation(options =>
    /// {
    ///     options.IncludeCodes = true;             // default
    ///     options.HandleGuardExceptions = false;   // default
    /// }, typeof(Program).Assembly);
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/>, <paramref name="configure"/> or <paramref name="validatorAssemblies"/> is <see langword="null"/>.</exception>
    [RequiresUnreferencedCode(ScanningRequiresUnreferencedCode)]
    public static IServiceCollection AddMustValidation(this IServiceCollection services, Action<MustValidationOptions> configure, params Assembly[] validatorAssemblies)
    {
        ThrowHelper.ThrowIfNull(services);
        ThrowHelper.ThrowIfNull(configure);
        ThrowHelper.ThrowIfNull(validatorAssemblies);

        services.Configure(configure);

        foreach (var assembly in validatorAssemblies)
            services.AddMustValidatorsFromAssembly(assembly);

        services.TryAddSingleton<IMustFailureMessageResolver, DefaultMustFailureMessageResolver>();
        services.AddExceptionHandler<MustValidationExceptionHandler>();

        return services;
    }
}
