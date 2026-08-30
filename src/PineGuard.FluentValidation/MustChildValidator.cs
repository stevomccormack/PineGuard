using FluentValidation;
using FluentValidation.Results;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Presents an <see cref="IMustValidator{T}"/> to FluentValidation as an <see cref="IValidator{T}"/>, so
/// <see cref="RuleBuilderExtension.SetMustValidator{T, TProperty}"/> can hand it to FluentValidation's own
/// <c>SetValidator</c> and inherit the child-validator behaviour that comes with it.
/// </summary>
/// <typeparam name="T">The type the wrapped validator validates.</typeparam>
/// <remarks>
/// <para>
/// Going through <c>SetValidator</c> rather than a hand-rolled rule is what buys the behaviour a consumer
/// expects for free: a <see langword="null"/> property is skipped, collection elements are indexed, and —
/// the reason it matters in this release — FluentValidation dispatches to <see cref="Validate(IValidationContext)"/>
/// when the parent validator runs synchronously and to
/// <see cref="ValidateAsync(IValidationContext, CancellationToken)"/> when it runs asynchronously. A Must
/// validator carrying asynchronous rules therefore works under <c>ValidateAsync</c>, which a synchronous-only
/// adaptation could not offer.
/// </para>
/// <para>
/// FluentValidation collects a child validator's failures from the context it supplies, not from the returned
/// <see cref="ValidationResult"/>, so the context-taking overloads add each failure to the context. The
/// property path is re-rooted under <c>context.PropertyChain</c> first, which is how a failure the Must
/// validator reported at <c>City</c> arrives at <c>ShipTo.City</c>, and one it reported at its own root
/// arrives at <c>ShipTo</c>.
/// </para>
/// <para>
/// This type is deliberately not public. The supported spelling is
/// <see cref="RuleBuilderExtension.SetMustValidator{T, TProperty}"/>; a public "Must validator as a
/// FluentValidation validator" surface is a separate, additive decision.
/// </para>
/// </remarks>
/// <seealso cref="RuleBuilderExtension"/>
/// <seealso cref="FluentMustValidator{T}"/>
internal sealed class MustChildValidator<T> : IValidator<T?>
    where T : notnull
{
    private readonly IMustValidator<T> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MustChildValidator{T}"/> class.
    /// </summary>
    /// <param name="validator">The PineGuard validator to present as a FluentValidation validator.</param>
    internal MustChildValidator(IMustValidator<T> validator) => _validator = validator;

    /// <summary>
    /// Validates the context's instance and adds every failure to the context, re-rooted under its property path.
    /// </summary>
    /// <param name="context">The FluentValidation context supplied by the parent rule.</param>
    /// <returns>The failures this validator contributed, in the order the Must validator reported them.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public ValidationResult Validate(IValidationContext context)
    {
        ThrowHelper.ThrowIfNull(context);

        var typedContext = ValidationContext<T>.GetFromNonGenericContext(context);
        return AddFailures(typedContext, _validator.Validate(typedContext.InstanceToValidate));
    }

    /// <summary>
    /// Asynchronously validates the context's instance and adds every failure to the context, re-rooted under
    /// its property path.
    /// </summary>
    /// <param name="context">The FluentValidation context supplied by the parent rule.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The failures this validator contributed, in the order the Must validator reported them.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context"/> is <see langword="null"/>. Thrown eagerly rather than surfaced on
    /// the returned task, so a caller that never awaits still sees the mistake.
    /// </exception>
    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellationToken = default)
    {
        ThrowHelper.ThrowIfNull(context);

        return ValidateContextAsync(ValidationContext<T>.GetFromNonGenericContext(context), cancellationToken);
    }

    /// <summary>
    /// Validates <paramref name="instance"/> on its own, outside any parent rule.
    /// </summary>
    /// <param name="instance">The instance to validate, or <see langword="null"/>.</param>
    /// <returns>
    /// A valid result when <paramref name="instance"/> is <see langword="null"/> — presence is a separate rule,
    /// spelled <c>.NotNull()</c> — otherwise the Must validator's result.
    /// </returns>
    public ValidationResult Validate(T? instance) =>
        instance is null ? new ValidationResult() : _validator.Validate(instance).ToValidationResult();

    /// <summary>
    /// Asynchronously validates <paramref name="instance"/> on its own, outside any parent rule.
    /// </summary>
    /// <param name="instance">The instance to validate, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A valid result when <paramref name="instance"/> is <see langword="null"/>; otherwise the Must validator's result.
    /// </returns>
    public async Task<ValidationResult> ValidateAsync(T? instance, CancellationToken cancellationToken = default)
    {
        if (instance is null)
            return new ValidationResult();

        var result = await _validator.ValidateAsync(instance, cancellationToken).ConfigureAwait(false);
        return result.ToValidationResult();
    }

    /// <summary>
    /// Describes the FluentValidation rules this validator exposes — none, because a Must validator's rules are
    /// not FluentValidation rules.
    /// </summary>
    /// <returns>An empty descriptor.</returns>
    /// <remarks>
    /// An empty descriptor is the honest answer and degrades gracefully: a consumer that reads descriptors to
    /// generate metadata (MVC's client-side validation, for one) simply finds nothing to generate for this
    /// property, rather than failing.
    /// </remarks>
    public IValidatorDescriptor CreateDescriptor() => new ValidatorDescriptor<T?>([]);

    /// <inheritdoc/>
    public bool CanValidateInstancesOfType(Type type) => typeof(T).IsAssignableFrom(type);

    private async Task<ValidationResult> ValidateContextAsync(ValidationContext<T> context, CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(context.InstanceToValidate, cancellationToken).ConfigureAwait(false);
        return AddFailures(context, result);
    }

    private static ValidationResult AddFailures(ValidationContext<T> context, MustValidationResult result)
    {
        var validationResult = result.WithPropertyPathPrefix(context.PropertyChain.ToString()).ToValidationResult();

        foreach (var failure in validationResult.Errors)
            context.AddFailure(failure);

        return validationResult;
    }
}
