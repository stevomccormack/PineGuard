using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.Extensions.DependencyInjection;

/// <summary>
/// Resolves <see cref="IMustValidator"/> registrations from an <see cref="IServiceProvider"/> by
/// <see cref="Type"/>, for callers that only learn the validated type at run time.
/// </summary>
/// <remarks>
/// A request filter, a pipeline behaviour or any other dispatcher holds an <see cref="object"/> and its
/// runtime <see cref="Type"/>, not a compile-time <c>T</c>, so it cannot ask for
/// <c>IMustValidator&lt;T&gt;</c> directly. These methods close the generic interface over the given type
/// and hand back the non-generic <see cref="IMustValidator"/> view, which validates <see cref="object"/>.
/// </remarks>
/// <seealso cref="MustValidatorServiceCollectionExtension"/>
/// <seealso cref="IMustValidator"/>
public static class ServiceProviderExtension
{
    /// <summary>
    /// Gets the validator registered for <paramref name="validatedType"/>, if there is one.
    /// </summary>
    /// <param name="provider">The service provider to resolve from.</param>
    /// <param name="validatedType">The type the validator validates.</param>
    /// <param name="validator">When this method returns <see langword="true"/>, the resolved validator; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a validator is registered for <paramref name="validatedType"/>; otherwise <see langword="false"/>.</returns>
    /// <remarks>
    /// When several validators are registered for the same type this returns the last one registered — the
    /// behaviour of every other single-service resolution in <c>Microsoft.Extensions.DependencyInjection</c>.
    /// Use <see cref="GetMustValidators"/> to run all of them.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> or <paramref name="validatedType"/> is <see langword="null"/>.</exception>
    public static bool TryGetMustValidator(this IServiceProvider provider, Type validatedType, [NotNullWhen(true)] out IMustValidator? validator)
    {
        ThrowHelper.ThrowIfNull(provider);
        ThrowHelper.ThrowIfNull(validatedType);

        validator = provider.GetService(MakeValidatorType(validatedType)) as IMustValidator;

        return validator is not null;
    }

    /// <summary>
    /// Gets every validator registered for <paramref name="validatedType"/>, in registration order.
    /// </summary>
    /// <param name="provider">The service provider to resolve from.</param>
    /// <param name="validatedType">The type the validators validate.</param>
    /// <returns>The registered validators; empty when none is registered.</returns>
    /// <remarks>
    /// Registering several validators for one type is supported by design
    /// (<see cref="MustValidatorServiceCollectionExtension"/> uses <c>Add</c>, not <c>TryAdd</c>), so a
    /// dispatcher that means to honour all of them uses this rather than
    /// <see cref="TryGetMustValidator"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> or <paramref name="validatedType"/> is <see langword="null"/>.</exception>
    public static IReadOnlyList<IMustValidator> GetMustValidators(this IServiceProvider provider, Type validatedType)
    {
        ThrowHelper.ThrowIfNull(provider);
        ThrowHelper.ThrowIfNull(validatedType);

        var enumerableType = typeof(IEnumerable<>).MakeGenericType(MakeValidatorType(validatedType));

        return ((IEnumerable)provider.GetRequiredService(enumerableType)).Cast<IMustValidator>().ToList();
    }

    private static Type MakeValidatorType(Type validatedType) => typeof(IMustValidator<>).MakeGenericType(validatedType);
}
