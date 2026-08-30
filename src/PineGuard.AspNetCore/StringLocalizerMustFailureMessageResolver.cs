using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Publishes each failure's message from an <see cref="IStringLocalizer"/> resource keyed by the
/// failure's stable <see cref="MustFailure.Code"/>, falling back to the English template the rule
/// rendered.
/// </summary>
/// <param name="options">The validation options naming the resource type to look codes up in.</param>
/// <param name="localizerFactory">
/// The localizer factory. Optional: an application that registers this resolver without calling
/// <c>AddLocalization()</c> gets the English templates rather than an unresolvable-service failure at the
/// first bad request.
/// </param>
/// <remarks>
/// Looking a message up by <b>code</b> rather than by English text is what makes the seam usable: the
/// resource key is the frozen <c>domain.aspect.condition</c> address, so re-wording an English template
/// never invalidates a translation. The resolved value has its <c>{paramName}</c> placeholder rendered
/// with the failure's property path, exactly as the rule would have rendered it.
/// </remarks>
/// <example>
/// <code>
/// builder.Services.AddLocalization();
/// builder.Services.AddSingleton&lt;IMustFailureMessageResolver, StringLocalizerMustFailureMessageResolver&gt;();
/// </code>
/// </example>
/// <seealso cref="DefaultMustFailureMessageResolver"/>
public sealed class StringLocalizerMustFailureMessageResolver(
    IOptions<MustValidationOptions> options,
    IStringLocalizerFactory? localizerFactory = null)
    : IMustFailureMessageResolver
{
    private const string ParamNameToken = "{paramName}";

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public string Resolve(MustFailure failure, HttpContext httpContext)
    {
        ThrowHelper.ThrowIfNull(failure);

        if (localizerFactory is null)
            return failure.Message;

        var localizer = localizerFactory.Create(options.Value.LocalizationResourceType ?? typeof(MustValidationOptions));
        var localized = localizer[failure.Code];

        return localized.ResourceNotFound
            ? failure.Message
            : localized.Value.Replace(ParamNameToken, failure.PropertyPath, StringComparison.Ordinal);
    }
}
