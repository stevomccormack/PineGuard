#if NET10_0_OR_GREATER
// Microsoft.Extensions.Validation's resolver seam ships [Experimental("ASP0029")] in .NET 10. PineGuard
// takes that on here, in one internal type, so no consumer ever sees the diagnostic: the only public
// surface is ValidationOptionsExtension.AddMustValidatorResolver(), whose signature names nothing
// experimental. .NET 11 stabilised the seam, so there the pragma is a no-op.
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
/// Plugs PineGuard validators into the built-in validation that ships from .NET 10 onwards, so a Minimal
/// API endpoint validated by <c>Microsoft.Extensions.Validation</c> runs <c>IMustValidator&lt;T&gt;</c>
/// alongside the data annotations it already ran.
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
#if NET11_0_OR_GREATER
    /// <summary>
    /// Claims <paramref name="type"/>, so that any PineGuard validator registered for it is run.
    /// </summary>
    /// <param name="type">The type about to be validated.</param>
    /// <param name="validatableTypeInfo">Always the info that runs PineGuard's validators and then the rest of the chain.</param>
    /// <returns>Always <see langword="true"/>.</returns>
    /// <remarks>
    /// Answering unconditionally costs a claimed type nothing when no validator is registered for it — the
    /// info resolves an empty validator list and hands straight on to the next resolver.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <see langword="null"/>.</exception>
    public bool TryGetValidatableTypeInfo(Type type, [NotNullWhen(true)] out IValidatableTypeInfo? validatableTypeInfo)
    {
        ThrowHelper.ThrowIfNull(type);

        validatableTypeInfo = new MustValidatableInfo(type);

        return true;
    }

    /// <summary>
    /// Declines every parameter: PineGuard validates a parameter through its type, which
    /// <see cref="TryGetValidatableTypeInfo"/> already claims.
    /// </summary>
    /// <param name="parameterInfo">The parameter about to be validated.</param>
    /// <param name="validatableParameterInfo">Always <see langword="null"/>.</param>
    /// <returns>Always <see langword="false"/>.</returns>
    public bool TryGetValidatableParameterInfo(ParameterInfo parameterInfo, [NotNullWhen(true)] out IValidatableParameterInfo? validatableParameterInfo)
    {
        validatableParameterInfo = null;

        return false;
    }
#else
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
#endif

    /// <summary>
    /// Runs every PineGuard validator registered for one claimed type, then whatever the rest of the
    /// resolver chain would have run for it.
    /// </summary>
    /// <param name="type">The claimed type, used to resolve validators and to ask the remaining resolvers.</param>
#if NET11_0_OR_GREATER
    private sealed class MustValidatableInfo(Type type) : IValidatableTypeInfo
#else
    private sealed class MustValidatableInfo(Type type) : IValidatableInfo
#endif
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

#if NET11_0_OR_GREATER
        /// <summary>
        /// Validates <paramref name="value"/> on the synchronous path .NET 11 added, by waiting on
        /// <see cref="ValidateAsync"/>.
        /// </summary>
        /// <param name="value">The value to validate; <see langword="null"/> has nothing to validate.</param>
        /// <param name="context">The validation in progress, supplying the services, the current path and the error dictionary.</param>
        /// <remarks>
        /// <see cref="IMustValidator"/> speaks only async, so a genuinely synchronous run
        /// does not exist to offer. Waiting here cannot deadlock: ASP.NET Core runs without a
        /// <see cref="SynchronizationContext"/>, and every await on the path confirms that with
        /// <c>ConfigureAwait(false)</c>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
        public void Validate(object? value, ValidateContext context) =>
            ValidateAsync(value, context, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Hands the property lookup to the first resolver after PineGuard's that claims the type.
        /// </summary>
        /// <param name="propertyName">The property being looked up.</param>
        /// <param name="validationOptions">The options whose resolver chain answers the lookup.</param>
        /// <param name="validatablePropertyInfo">Whatever the rest of the chain found, or <see langword="null"/>.</param>
        /// <returns>Whether the rest of the chain found the property.</returns>
        /// <remarks>
        /// PineGuard validates whole values, never single properties, so this info has no property
        /// knowledge of its own — but it sits in front of infos that do, and masking theirs would make
        /// claiming a type cost more than nothing.
        /// </remarks>
        public bool TryFindProperty(string propertyName, ValidationOptions validationOptions, [NotNullWhen(true)] out IValidatablePropertyInfo? validatablePropertyInfo)
        {
            validatablePropertyInfo = null;

            return NextClaimingInfo(validationOptions) is { } info
                && info.TryFindProperty(propertyName, validationOptions, out validatablePropertyInfo);
        }
#endif

        private async Task ValidateWithMustValidatorsAsync(object value, ValidateContext context, CancellationToken cancellationToken)
        {
#if NET11_0_OR_GREATER
            // .NET 11 carries the request's services directly; a context built without any has no
            // container to resolve validators from, so there is nothing of PineGuard's to run.
            if (context.ServiceProvider is not { } services)
                return;
#else
            var services = context.ValidationContext;
#endif

            foreach (var validator in services.GetMustValidators(type))
            {
                var result = await validator.ValidateAsync(value, cancellationToken).ConfigureAwait(false);

                foreach (var failure in result.Failures)
#if NET11_0_OR_GREATER
                    AddError(context, value, failure);
#else
                    AddError(context, failure);
#endif
            }
        }

        /// <summary>
        /// Hands the value on to the first resolver after PineGuard's that claims the type, so this resolver
        /// only ever adds validation and never replaces it.
        /// </summary>
        private async Task ValidateWithRemainingResolversAsync(object? value, ValidateContext context, CancellationToken cancellationToken)
        {
#if NET11_0_OR_GREATER
            if (NextClaimingInfo(context.ValidationOptions) is { } info)
                await info.ValidateAsync(value, context, cancellationToken).ConfigureAwait(false);
#else
            if (context.ValidationOptions is not { } options)
                return;

            if (NextClaimingInfo(options) is { } info)
                await info.ValidateAsync(value, context, cancellationToken).ConfigureAwait(false);
#endif
        }

#if NET11_0_OR_GREATER
        private IValidatableTypeInfo? NextClaimingInfo(ValidationOptions options)
#else
        private IValidatableInfo? NextClaimingInfo(ValidationOptions options)
#endif
        {
            foreach (var resolver in options.Resolvers)
            {
                if (resolver is not MustValidatableInfoResolver && resolver.TryGetValidatableTypeInfo(type, out var info))
                    return info;
            }

            return null;
        }

#if NET11_0_OR_GREATER
        /// <summary>
        /// Records <paramref name="failure"/>'s message under the path the built-in pipeline is currently at.
        /// </summary>
        /// <remarks>
        /// <see cref="ValidateContext.ValidationErrors"/> is only created once something fails, so the first
        /// failure of a request creates it. <see cref="ValidationError.Name"/> carries the leaf the failure
        /// sits on — the member name, the way the built-in infos spell it — while
        /// <see cref="ValidationError.Path"/> is the full key; a root failure has no leaf, and names the
        /// empty string exactly as the built-in type-level errors do.
        /// </remarks>
        private static void AddError(ValidateContext context, object container, MustFailure failure)
        {
            var key = failure.PropertyPath.Length == 0
                ? context.CurrentValidationPath
                : PropertyPathUtility.Combine(context.CurrentValidationPath, failure.PropertyPath);

            var separator = failure.PropertyPath.LastIndexOf(PropertyPathUtility.PropertySeparator);

            context.AddValidationError(new ValidationError
            {
                Name = separator < 0 ? failure.PropertyPath : failure.PropertyPath[(separator + 1)..],
                Path = key,
                ErrorMessage = failure.Message,
                Container = container
            });
        }
#else
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
#endif
    }
}
#pragma warning restore ASP0029
#endif
