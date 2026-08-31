using Microsoft.AspNetCore.Http;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Publishes each failure's message exactly as the rule rendered it — the resolver a monolingual
/// application never has to think about.
/// </summary>
/// <remarks>
/// Registered with <c>TryAddSingleton</c>, so an application that registers its own
/// <see cref="IMustFailureMessageResolver"/> before calling <c>AddMustValidation</c> keeps it.
/// </remarks>
/// <seealso cref="StringLocalizerMustFailureMessageResolver"/>
public sealed class DefaultMustFailureMessageResolver : IMustFailureMessageResolver
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public string Resolve(MustFailure failure, HttpContext httpContext)
    {
        ThrowHelper.ThrowIfNull(failure);

        return failure.Message;
    }
}
