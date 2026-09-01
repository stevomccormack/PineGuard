#if NET10_0_OR_GREATER
// ValidationOptions.Resolvers ships [Experimental("ASP0029")] in .NET 10; the diagnostic is taken here so
// that AddMustValidatorResolver() — whose signature names only the stable ValidationOptions — is clean at
// every call site.
#pragma warning disable ASP0029
using Microsoft.Extensions.Validation;
using PineGuard.Common;

namespace PineGuard.AspNetCore;

/// <summary>
/// Turns PineGuard validators on inside .NET 10's built-in validation pipeline.
/// </summary>
/// <remarks>
/// Assumes <c>services.AddMustValidation(...)</c> has already registered the validators; this extension
/// only decides that the built-in pipeline is where they run.
/// </remarks>
/// <seealso cref="MustValidationServiceCollectionExtension"/>
/// <seealso cref="MustValidationEndpointFilter"/>
public static class ValidationOptionsExtension
{
    /// <summary>
    /// Adds the resolver that runs PineGuard validators to <paramref name="options"/>.
    /// </summary>
    /// <param name="options">The built-in validation options to add the resolver to.</param>
    /// <returns><paramref name="options"/>, for further chaining.</returns>
    /// <remarks>
    /// The resolver goes to the head of <see cref="ValidationOptions.Resolvers"/>, which are consulted in
    /// order, so PineGuard's failures are found before the rest of the chain's; the resolver then hands the
    /// value on, so data annotations and <c>[ValidatableType]</c> still run.
    /// <para>
    /// Named for what it adds — one resolver — so it never reads as a second spelling of the DI package's
    /// <c>AddMustValidatorsFromAssembly</c>, which adds validators.
    /// </para>
    /// <para>
    /// Failure codes are not carried on this path: the built-in error shape has nowhere to put them. Use
    /// <see cref="EndpointConventionBuilderExtension.AddMustValidation{TBuilder}"/> when the response must
    /// publish codes.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddMustValidation(typeof(Program).Assembly);
    /// builder.Services.AddValidation(options => options.AddMustValidatorResolver());
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <see langword="null"/>.</exception>
    public static ValidationOptions AddMustValidatorResolver(this ValidationOptions options)
    {
        ThrowHelper.ThrowIfNull(options);

        options.Resolvers.Insert(0, new MustValidatableInfoResolver());

        return options;
    }
}
#pragma warning restore ASP0029
#endif
