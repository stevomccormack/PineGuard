using FluentValidation;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Extends FluentValidation's <see cref="IRuleBuilder{T, TProperty}"/> with the composition step
/// FluentValidation itself spells <c>SetValidator</c>: hand a nested property to a validator that
/// already knows how to validate it — here, a PineGuard <see cref="IMustValidator{T}"/>.
/// </summary>
/// <remarks>
/// <para>
/// This is the half of the FluentValidation bridge that travels inwards. <see cref="FluentMustValidator{T}"/>
/// lets a PineGuard seam consume a FluentValidation validator; <see cref="SetMustValidator{T, TProperty}"/>
/// lets a FluentValidation validator consume a PineGuard one, so a team migrating in either direction
/// never has to rewrite the validator it already trusts.
/// </para>
/// </remarks>
/// <seealso cref="FluentMustValidator{T}"/>
/// <seealso cref="ValidationResultExtension"/>
public static class RuleBuilderExtension
{
    /// <summary>
    /// Validates the property with <paramref name="validator"/>, re-rooting every failure it reports
    /// under the property's own path — the <see cref="IMustValidator{T}"/> counterpart of
    /// FluentValidation's <c>SetValidator</c>.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated. The property itself may be nullable; <paramref name="validator"/> is typed for its non-null form.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="validator">The PineGuard validator to run against the property value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{T, TProperty}"/> for further rule chaining, exactly as <c>SetValidator</c> returns.</returns>
    /// <remarks>
    /// <para>
    /// A <see langword="null"/> property is skipped, matching both FluentValidation's <c>SetValidator</c>
    /// and the nested-validator rule on <c>MustValidator&lt;T&gt;</c>: presence is a separate rule, spelled
    /// <c>.NotNull()</c>.
    /// </para>
    /// <para>
    /// Each failure crosses as a <c>ValidationFailure</c> carrying the rule's <c>Code</c> as its
    /// <c>ErrorCode</c> and its already-rendered message, at <c>"{property path}.{failure path}"</c> —
    /// so a <c>City</c> failure inside <c>ShipTo</c> is reported at <c>ShipTo.City</c>, and a failure the
    /// nested validator reported at its own root lands on <c>ShipTo</c> itself. Under
    /// <c>RuleForEach</c> the element index comes with it: <c>Lines[1].Sku</c>.
    /// </para>
    /// <para>
    /// The rule follows the parent validator: <c>Validate</c> runs the nested validator synchronously and
    /// <c>ValidateAsync</c> runs it asynchronously, so a validator carrying asynchronous rules is usable here
    /// as long as the parent is validated asynchronously — and surfaces its usual
    /// <see cref="InvalidOperationException"/> if it is not.
    /// </para>
    /// <para>
    /// <typeparamref name="TProperty"/> is constrained to <c>notnull</c>, following
    /// <see cref="IMustValidator{T}"/>. A nullable value-type property (<c>int?</c>) therefore needs a Must
    /// clause rather than a nested validator, which is the natural spelling for one anyway.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ruleBuilder"/> or <paramref name="validator"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// RuleFor(x =&gt; x.ShipTo).SetMustValidator(new AddressMustValidator());
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, TProperty?> SetMustValidator<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        IMustValidator<TProperty> validator)
        where TProperty : notnull
    {
        ThrowHelper.ThrowIfNull(ruleBuilder);
        ThrowHelper.ThrowIfNull(validator);

        return ruleBuilder.SetValidator(new MustChildValidator<TProperty>(validator));
    }
}
