#if NET10_0_OR_GREATER
// Microsoft.Extensions.Validation's resolver seam ships [Experimental("ASP0029")] in .NET 10. PineGuard
// takes that on here, in one internal type, so no consumer ever sees the diagnostic: the only public
// surface is ValidationOptionsExtension.AddMustValidatorResolver(), whose signature names nothing
// experimental.
#pragma warning disable ASP0029
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Validation;
using PineGuard.Common;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Utils;

namespace PineGuard.AspNetCore;

/// <summary>
/// Plugs PineGuard validators into .NET 10's built-in validation, so a Minimal API endpoint validated by
/// <c>Microsoft.Extensions.Validation</c> runs <c>IMustValidator&lt;T&gt;</c> alongside the data annotations
/// it already ran.
/// </summary>
/// <remarks>
/// Registered by <see cref="ValidationOptionsExtension.AddMustValidatorResolver"/>, which inserts it at the
/// head of <see cref="ValidationOptions.Resolvers"/>. Internal because the interface it implements is
/// experimental in .NET 10 — an application composes it through the extension method, never by name.
/// <para>
/// The resolver is deliberately additive: it claims every type, then delegates to the rest of the resolver
/// chain, so <c>[ValidatableType]</c>, the source generator and the data-annotation walk all keep working
/// exactly as they did. Claiming a type is the only way to have PineGuard validators considered at all —
/// which validators exist is a question only the request's container can answer, and a resolver is asked
/// before any container exists.
/// </para>
/// <para>
/// Codes are not carried on this path: the built-in error shape is a dictionary of messages with nowhere to
/// put a <see cref="MustFailure.Code"/>. An application that needs codes uses the endpoint filter instead.
/// </para>
/// </remarks>
/// <seealso cref="ValidationOptionsExtension"/>
/// <seealso cref="MustValidationEndpointFilter"/>
internal sealed class MustValidatableInfoResolver : IValidatableInfoResolver
{
    /// <summary>
    /// Claims <paramref name="type"/>, so that any PineGuard validator registered for it is run.
    /// </summary>
    /// <param name="type">The type about to be validated.</param>
    /// <param name="validatableInfo">Always the info that runs PineGuard's validators and then the rest of the chain.</param>
    /// <returns>Always <see langword="true"/>.</returns>
    /// <remarks>
    /// Answering unconditionally costs a claimed type nothing when no validator is registered for it — the
    /// info resolves an empty validator list and hands straight on to the next resolver.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <see langword="null"/>.</exception>
    public bool TryGetValidatableTypeInfo(Type type, [NotNullWhen(true)] out IValidatableInfo? validatableInfo)
    {
        ThrowHelper.ThrowIfNull(type);

        validatableInfo = new MustValidatableInfo(type);

        return true;
    }

    /// <summary>
    /// Declines every parameter: PineGuard validates a parameter through its type, which
    /// <see cref="TryGetValidatableTypeInfo"/> already claims.
    /// </summary>
    /// <param name="parameterInfo">The parameter about to be validated.</param>
    /// <param name="validatableInfo">Always <see langword="null"/>.</param>
    /// <returns>Always <see langword="false"/>.</returns>
    public bool TryGetValidatableParameterInfo(ParameterInfo parameterInfo, [NotNullWhen(true)] out IValidatableInfo? validatableInfo)
    {
        validatableInfo = null;

        return false;
    }

    /// <summary>
    /// Runs every PineGuard validator registered for one claimed type, then whatever the rest of the
    /// resolver chain would have run for it.
    /// </summary>
    /// <param name="type">The claimed type, used to resolve validators and to ask the remaining resolvers.</param>
    private sealed class MustValidatableInfo(Type type) : IValidatableInfo
    {
        /// <summary>
        /// Validates <paramref name="value"/> and records what PineGuard found in
        /// <see cref="ValidateContext.ValidationErrors"/>.
        /// </summary>
        /// <param name="value">The value to validate; <see langword="null"/> has nothing to validate.</param>
        /// <param name="context">The validation in progress, supplying the services, the current path and the error dictionary.</param>
        /// <param name="cancellationToken">Observed by every validator.</param>
        /// <returns>A task that completes once this type's validators and the rest of the chain have run.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
        public async Task ValidateAsync(object? value, ValidateContext context, CancellationToken cancellationToken)
        {
            ThrowHelper.ThrowIfNull(context);

            if (value is not null)
                await ValidateWithMustValidatorsAsync(value, context, cancellationToken).ConfigureAwait(false);

            await ValidateWithRemainingResolversAsync(value, context, cancellationToken).ConfigureAwait(false);
        }

        private async Task ValidateWithMustValidatorsAsync(object value, ValidateContext context, CancellationToken cancellationToken)
        {
            foreach (var validator in context.ValidationContext.GetMustValidators(type))
            {
                var result = await validator.ValidateAsync(value, cancellationToken).ConfigureAwait(false);

                foreach (var failure in result.Failures)
                    AddError(context, failure);
            }
        }

        /// <summary>
        /// Hands the value on to the first resolver after PineGuard's that claims the type, so this resolver
        /// only ever adds validation and never replaces it.
        /// </summary>
        private async Task ValidateWithRemainingResolversAsync(object? value, ValidateContext context, CancellationToken cancellationToken)
        {
            if (context.ValidationOptions is not { } options)
                return;

            foreach (var resolver in options.Resolvers)
            {
                if (resolver is MustValidatableInfoResolver || !resolver.TryGetValidatableTypeInfo(type, out var info))
                    continue;

                await info.ValidateAsync(value, context, cancellationToken).ConfigureAwait(false);
                return;
            }
        }

        /// <summary>
        /// Appends <paramref name="failure"/>'s message under the path the built-in pipeline is currently at.
        /// </summary>
        /// <remarks>
        /// <see cref="ValidateContext.ValidationErrors"/> is only created once something fails, so the first
        /// failure of a request creates it.
        /// </remarks>
        private static void AddError(ValidateContext context, MustFailure failure)
        {
            var key = failure.PropertyPath.Length == 0
                ? context.CurrentValidationPath
                : PropertyPathUtility.Combine(context.CurrentValidationPath, failure.PropertyPath);

            var errors = context.ValidationErrors ??= [];

            errors[key] = errors.TryGetValue(key, out var existing) ? [.. existing, failure.Message] : [failure.Message];
        }
    }
}
#pragma warning restore ASP0029
#endif
