using System.Text.Json.Serialization;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// One entry of the <c>failures</c> extension on a validation response: the same failure the
/// <c>errors</c> dictionary carries, plus the stable <see cref="MustFailure.Code"/> that dictionary has
/// nowhere to put.
/// </summary>
/// <param name="PropertyPath">
/// Where in the request the failure is, already transformed by the application's naming policy
/// (<c>lines[1].sku</c>). Serialised as <c>property</c>.
/// </param>
/// <param name="Code">The stable, machine-readable identity of the rule that failed.</param>
/// <param name="Message">The rendered, human-readable failure message — the same text the <c>errors</c> dictionary carries.</param>
/// <remarks>
/// Deliberately does not carry <see cref="MustFailure.Value"/>: the attempted value may be a password or
/// a token, and nothing in this package puts it on the wire.
/// </remarks>
/// <seealso cref="ProblemDetailsExtension"/>
/// <seealso cref="MustFailure"/>
public sealed record MustFailureDetail(
    [property: JsonPropertyName("property")] string PropertyPath,
    string Code,
    string Message);
