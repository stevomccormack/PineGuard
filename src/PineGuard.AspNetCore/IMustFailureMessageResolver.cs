using Microsoft.AspNetCore.Http;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Produces the message published for a <see cref="MustFailure"/> — the seam a localised application
/// replaces to answer in the caller's language.
/// </summary>
/// <remarks>
/// Registered as a singleton by <c>services.AddMustValidation(...)</c> with
/// <see cref="DefaultMustFailureMessageResolver"/> as the implementation, unless the application has
/// already registered one of its own (<see cref="StringLocalizerMustFailureMessageResolver"/>, say).
/// <para>
/// An implementation never reads <see cref="MustFailure.Value"/>: the attempted value may be a secret,
/// and a resource table is no better a place for it than a response body.
/// </para>
/// </remarks>
/// <seealso cref="DefaultMustFailureMessageResolver"/>
/// <seealso cref="StringLocalizerMustFailureMessageResolver"/>
public interface IMustFailureMessageResolver
{
    /// <summary>
    /// Resolves the message published for <paramref name="failure"/>.
    /// </summary>
    /// <param name="failure">The failure to render a message for.</param>
    /// <param name="httpContext">The request the failure was found on, for implementations that vary by request (culture, tenant, header).</param>
    /// <returns>The message to publish.</returns>
    string Resolve(MustFailure failure, HttpContext httpContext);
}
