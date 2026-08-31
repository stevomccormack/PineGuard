using Microsoft.AspNetCore.Http;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MustClauses;

namespace PineGuard.AspNetCore;

/// <summary>
/// Runs every registered validator over the arguments an entry point bound, and combines what they found
/// into one result.
/// </summary>
/// <remarks>
/// The Minimal API filter and the MVC filter differ only in where their arguments come from and what they
/// do with a failure, so the part between — resolve validators for each argument's runtime type, await
/// them, aggregate — lives here and is written once.
/// </remarks>
/// <seealso cref="MustValidationEndpointFilter"/>
/// <seealso cref="MustValidationActionFilter"/>
internal static class MustValidationRunner
{
    /// <summary>
    /// Validates each of <paramref name="arguments"/> with every validator registered for its runtime type.
    /// </summary>
    /// <param name="arguments">The bound arguments to validate. A <see langword="null"/> argument has no runtime type to resolve a validator for and is skipped.</param>
    /// <param name="httpContext">The request supplying the validators, and the token they observe.</param>
    /// <param name="mode">Whether validators aggregate every failure or stop at the first one.</param>
    /// <returns>The combined result — successful when no argument had a validator, or none of them failed.</returns>
    /// <remarks>
    /// An argument is validated whatever its binding source, so an <c>[AsParameters]</c> query object is
    /// treated exactly like a body. Under <see cref="MustValidationMode.StopOnFirstFailure"/> the run stops
    /// at the first validator that fails, so the promise the mode makes holds across arguments and not only
    /// within one validator.
    /// </remarks>
    internal static async ValueTask<MustValidationResult> ValidateAsync(IEnumerable<object?> arguments, HttpContext httpContext, MustValidationMode mode)
    {
        var results = new List<MustValidationResult>();

        foreach (var argument in arguments)
        {
            if (argument is null)
                continue;

            foreach (var validator in httpContext.RequestServices.GetMustValidators(argument.GetType()))
            {
                var result = await validator.ValidateAsync(argument, mode, httpContext.RequestAborted).ConfigureAwait(false);
                results.Add(result);

                if (result.Failed && mode == MustValidationMode.StopOnFirstFailure)
                    return MustValidationResult.Combine(results);
            }
        }

        return MustValidationResult.Combine(results);
    }
}
