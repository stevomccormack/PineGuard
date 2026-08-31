using Microsoft.Extensions.DependencyInjection;
using PineGuard.Common;

namespace PineGuard.MediatR;

/// <summary>
/// Adds PineGuard validation to a MediatR pipeline from inside <c>AddMediatR(cfg =&gt; …)</c>.
/// </summary>
/// <remarks>
/// The verb matches the ASP.NET Core integration's <c>AddMustValidation()</c>: one word for "run my Must
/// validators here", whichever seam is being configured.
/// </remarks>
/// <seealso cref="MustValidationBehavior{TRequest, TResponse}"/>
public static class MediatRServiceConfigurationExtension
{
    /// <summary>
    /// Registers <see cref="MustValidationBehavior{TRequest, TResponse}"/> as an open pipeline behaviour, so
    /// every request with a registered <c>IMustValidator&lt;TRequest&gt;</c> is validated before its handler.
    /// </summary>
    /// <param name="configuration">The MediatR configuration being built.</param>
    /// <returns>The same <paramref name="configuration"/>, so calls chain.</returns>
    /// <remarks>
    /// Register the validators themselves separately — <c>AddMustValidatorsFromAssemblyContaining&lt;T&gt;()</c>
    /// in <c>PineGuard.Extensions.DependencyInjection</c> is the usual companion call. A request whose type
    /// has no registered validator is unaffected.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configuration"/> is <see langword="null"/>.</exception>
    public static MediatRServiceConfiguration AddMustValidation(this MediatRServiceConfiguration configuration)
    {
        ThrowHelper.ThrowIfNull(configuration);

        return configuration.AddOpenBehavior(typeof(MustValidationBehavior<,>));
    }
}
