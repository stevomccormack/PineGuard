using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Common;

namespace PineGuard.AspNetCore;

/// <summary>
/// Turns PineGuard validation on for every MVC action.
/// </summary>
/// <remarks>
/// Assumes <c>services.AddMustValidation(...)</c> has already registered the options, the message resolver
/// and the validators; this extension only adds the filter that runs them.
/// </remarks>
/// <seealso cref="MustValidationActionFilter"/>
/// <seealso cref="MustValidationServiceCollectionExtension"/>
public static class MvcBuilderExtension
{
    /// <summary>
    /// Adds <see cref="MustValidationActionFilter"/> to the MVC filter pipeline.
    /// </summary>
    /// <param name="builder">The MVC builder to configure.</param>
    /// <returns><paramref name="builder"/>, for further chaining.</returns>
    /// <remarks>
    /// The filter is global: every action argument with a registered validator is validated, and an action
    /// with no validated argument is unaffected.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services.AddControllers().AddMustValidation();
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IMvcBuilder AddMustValidation(this IMvcBuilder builder)
    {
        ThrowHelper.ThrowIfNull(builder);

        builder.Services.Configure<MvcOptions>(options => options.Filters.Add<MustValidationActionFilter>());

        return builder;
    }
}
