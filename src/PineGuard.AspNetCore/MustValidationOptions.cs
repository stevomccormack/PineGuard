using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PineGuard.Codes;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// The per-application settings every PineGuard request-validation component reads: the filters, the
/// exception handler and the <see cref="ValidationProblemDetails"/> builder.
/// </summary>
/// <remarks>
/// Registered by <c>services.AddMustValidation(configure, assemblies)</c> and consumed as
/// <c>IOptions&lt;MustValidationOptions&gt;</c>, whose <c>Value</c> each component reads once. Every
/// default is the conservative one: failures aggregate, codes are published, and only
/// <see cref="MustValidationException"/> becomes a 400.
/// </remarks>
/// <seealso cref="ProblemDetailsExtension"/>
public sealed class MustValidationOptions
{
    /// <summary>
    /// Gets or sets the naming policy applied to every error key and property path, overriding the
    /// application's JSON naming policy.
    /// </summary>
    /// <value><see langword="null"/> by default, meaning the application's own policy decides.</value>
    /// <remarks>
    /// Set this only when the error keys must differ from the way the application serialises its models —
    /// the default of following the app's JSON policy is what keeps <c>errors</c> keys matching the field
    /// names the client actually sent.
    /// </remarks>
    public JsonNamingPolicy? PropertyNamingPolicy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application's JSON naming policy is used when
    /// <see cref="PropertyNamingPolicy"/> is <see langword="null"/>.
    /// </summary>
    /// <value><see langword="true"/> by default.</value>
    /// <remarks>
    /// Set to <see langword="false"/> to publish property paths exactly as the validators declare them
    /// (<c>Lines[1].Sku</c>), regardless of how the application serialises models.
    /// </remarks>
    public bool UseJsonNamingPolicy { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the response body carries the <c>failures</c> extension —
    /// one entry per failure, each with its stable <see cref="MustFailure.Code"/>.
    /// </summary>
    /// <value><see langword="true"/> by default.</value>
    /// <remarks>
    /// The <c>errors</c> dictionary of RFC 9457 has nowhere to put a code, so a client that means to branch
    /// on the *rule* rather than the message reads <c>failures</c>. Turn this off to emit a plain
    /// <see cref="ValidationProblemDetails"/>.
    /// </remarks>
    public bool IncludeCodes { get; set; } = true;

    /// <summary>
    /// Gets or sets whether validators collect every failure or stop at the first rule that fails.
    /// </summary>
    /// <value><see cref="MustValidationMode.Aggregate"/> by default.</value>
    public MustValidationMode Mode { get; set; } = MustValidationMode.Aggregate;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ArgumentException"/> family reaching the
    /// exception handler is turned into a 400.
    /// </summary>
    /// <value><see langword="false"/> by default.</value>
    /// <remarks>
    /// <b>Warning.</b> Setting this to <see langword="true"/> maps every <see cref="ArgumentException"/>,
    /// <see cref="ArgumentNullException"/> and <see cref="ArgumentOutOfRangeException"/> reaching the
    /// exception handler to a 400 — including one thrown by a programmer error deep inside your own code or
    /// a dependency. That hides bugs behind a client-error status. A guard three layers down is a bug, not a
    /// bad request; the 400 spelling at a boundary is
    /// <c>MustValidationResult.From(...).ThrowIfFailed()</c>.
    /// </remarks>
    public bool HandleGuardExceptions { get; set; }

    /// <summary>
    /// Gets or sets the code reported for an argument exception PineGuard did not itself throw, and so
    /// carries no stamped code.
    /// </summary>
    /// <value><see cref="MustCodes.Value.Argument.Invalid"/> by default — the reserved catalogue constant no clause emits.</value>
    /// <remarks>Only ever read when <see cref="HandleGuardExceptions"/> is <see langword="true"/>.</remarks>
    public string UnknownGuardCode { get; set; } = MustCodes.Value.Argument.Invalid;

    /// <summary>
    /// Gets or sets the <see cref="ProblemDetails.Title"/> of every validation response.
    /// </summary>
    /// <value><c>"One or more validation errors occurred."</c> by default — the wording ASP.NET Core itself uses.</value>
    public string Title { get; set; } = "One or more validation errors occurred.";

    /// <summary>
    /// Gets or sets the resource type <see cref="StringLocalizerMustFailureMessageResolver"/> looks failure
    /// codes up in.
    /// </summary>
    /// <value><see langword="null"/> by default, meaning <see cref="MustValidationOptions"/> itself names the resource.</value>
    public Type? LocalizationResourceType { get; set; }
}
