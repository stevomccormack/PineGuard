using FluentValidation.Results;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Bridges FluentValidation's <see cref="ValidationResult"/> and PineGuard's <see cref="MustValidationResult"/>
/// in both directions, so a validator written in either style can be consumed by a seam that expects the other.
/// </summary>
/// <remarks>
/// <para>
/// The mapping is one pair of slots each way: <see cref="ValidationFailure.PropertyName"/> ↔
/// <see cref="MustFailure.PropertyPath"/> and <see cref="ValidationFailure.ErrorCode"/> ↔
/// <see cref="MustFailure.Code"/>. Failure order is preserved, so a consumer that relies on rule
/// registration order keeps it across the bridge.
/// </para>
/// <para>
/// <see cref="MustFailure.Value"/> travels inbound only. A FluentValidation failure's
/// <see cref="ValidationFailure.AttemptedValue"/> is carried into it, because that is the property's
/// documented purpose; the outbound direction deliberately leaves <c>AttemptedValue</c> unset, because
/// <see cref="MustFailure.Value"/> may hold a secret and no PineGuard adapter puts it into an object it
/// hands to another framework — the same rule the ErrorOr, FluentResults and OneOf bridges follow.
/// </para>
/// </remarks>
/// <seealso cref="MustValidationResult"/>
/// <seealso cref="MustFailure"/>
/// <seealso cref="FluentMustValidator{T}"/>
public static class ValidationResultExtension
{
    /// <summary>
    /// Converts a FluentValidation <see cref="ValidationResult"/> into a <see cref="MustValidationResult"/>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// <see cref="MustValidationResult.Ok()"/> when <paramref name="result"/> is valid; otherwise a failed
    /// result carrying one <see cref="MustFailure"/> per error, in FluentValidation's order.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var mustResult = new OrderValidator().Validate(order).ToMustValidationResult();
    /// </code>
    /// </example>
    public static MustValidationResult ToMustValidationResult(this ValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.IsValid
            ? MustValidationResult.Ok()
            : MustValidationResult.Fail(result.Errors.Select(ToMustFailure));
    }

    /// <summary>
    /// Converts a FluentValidation <see cref="ValidationFailure"/> into a <see cref="MustFailure"/>.
    /// </summary>
    /// <param name="failure">The failure to convert.</param>
    /// <returns>
    /// A <see cref="MustFailure"/> whose <see cref="MustFailure.PropertyPath"/> is
    /// <see cref="ValidationFailure.PropertyName"/>, <see cref="MustFailure.Code"/> is
    /// <see cref="ValidationFailure.ErrorCode"/>, <see cref="MustFailure.Message"/> is
    /// <see cref="ValidationFailure.ErrorMessage"/> and <see cref="MustFailure.Value"/> is
    /// <see cref="ValidationFailure.AttemptedValue"/>. Each string slot falls back to
    /// <see cref="string.Empty"/>, because FluentValidation leaves all three nullable.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public static MustFailure ToMustFailure(this ValidationFailure failure)
    {
        ThrowHelper.ThrowIfNull(failure);

        return new MustFailure(
            failure.PropertyName ?? string.Empty,
            failure.ErrorCode ?? string.Empty,
            failure.ErrorMessage ?? string.Empty,
            failure.AttemptedValue);
    }

    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into a FluentValidation <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> carrying one <see cref="ValidationFailure"/> per
    /// <see cref="MustValidationResult.Failures"/> entry, in the order the validator reported them; an empty,
    /// and therefore valid, result when <paramref name="result"/> succeeded.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// public override ValidationResult Validate(ValidationContext&lt;Order&gt; context) =&gt;
    ///     _mustValidator.Validate(context.InstanceToValidate).ToValidationResult();
    /// </code>
    /// </example>
    public static ValidationResult ToValidationResult(this MustValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);

        return new ValidationResult(result.Failures.Select(ToValidationFailure));
    }

    /// <summary>
    /// Converts a <see cref="MustFailure"/> into a FluentValidation <see cref="ValidationFailure"/>.
    /// </summary>
    /// <param name="failure">The failure to convert.</param>
    /// <returns>
    /// A <see cref="ValidationFailure"/> whose <see cref="ValidationFailure.PropertyName"/> is
    /// <see cref="MustFailure.PropertyPath"/>, <see cref="ValidationFailure.ErrorMessage"/> is
    /// <see cref="MustFailure.Message"/> and <see cref="ValidationFailure.ErrorCode"/> is
    /// <see cref="MustFailure.Code"/>. <see cref="ValidationFailure.AttemptedValue"/> is left unset — see the
    /// class remarks.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public static ValidationFailure ToValidationFailure(this MustFailure failure)
    {
        ThrowHelper.ThrowIfNull(failure);

        return new ValidationFailure(failure.PropertyPath, failure.Message) { ErrorCode = failure.Code };
    }
}
