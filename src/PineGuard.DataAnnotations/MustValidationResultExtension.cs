using System.ComponentModel.DataAnnotations;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Turns a <see cref="MustValidationResult"/> into the <see cref="ValidationResult"/> sequence
/// <see cref="IValidatableObject.Validate"/> returns, so a validator written once runs inside every
/// framework that speaks DataAnnotations.
/// </summary>
/// <remarks>
/// <para>
/// The bridge is three lines at the call site:
/// </para>
/// <code>
/// public IEnumerable&lt;ValidationResult&gt; Validate(ValidationContext validationContext) =&gt;
///     new OrderMustValidator().Validate(this).ToValidationResults();
/// </code>
/// <para>
/// <see cref="MustFailure.Code"/> does not survive the crossing: <see cref="ValidationResult"/> has no slot
/// for it, which is why codes are described as design-time only on the DataAnnotations surface. Keep the
/// <see cref="MustValidationResult"/> itself when a caller needs to key on the code.
/// </para>
/// <para>
/// <see cref="MustFailure.Value"/> is never carried across, because it may hold a secret and no PineGuard
/// adapter puts it into an object it hands to another framework.
/// </para>
/// </remarks>
/// <seealso cref="MustValidationResult"/>
/// <seealso cref="MustFailure"/>
public static class MustValidationResultExtension
{
    /// <summary>
    /// Converts a <see cref="MustValidationResult"/> into the <see cref="ValidationResult"/> sequence
    /// <see cref="IValidatableObject.Validate"/> expects.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    /// <returns>
    /// One <see cref="ValidationResult"/> per <see cref="MustValidationResult.Failures"/> entry, in the order
    /// the validator reported them; an empty sequence when <paramref name="result"/> succeeded, which is how
    /// <see cref="IValidatableObject"/> spells "no complaints".
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// public sealed class Order : IValidatableObject
    /// {
    ///     public IEnumerable&lt;ValidationResult&gt; Validate(ValidationContext validationContext) =&gt;
    ///         new OrderMustValidator().Validate(this).ToValidationResults();
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<ValidationResult> ToValidationResults(this MustValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);

        return result.Failures.Select(ToValidationResult);
    }

    /// <summary>
    /// Converts a <see cref="MustFailure"/> into a <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="failure">The failure to convert.</param>
    /// <returns>
    /// A <see cref="ValidationResult"/> whose <see cref="ValidationResult.ErrorMessage"/> is
    /// <see cref="MustFailure.Message"/> and whose <see cref="ValidationResult.MemberNames"/> is the single
    /// <see cref="MustFailure.PropertyPath"/> — or no member names at all when the failure is about the object
    /// itself rather than one of its members, which is how DataAnnotations spells a model-level error.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failure"/> is <see langword="null"/>.</exception>
    public static ValidationResult ToValidationResult(this MustFailure failure)
    {
        ThrowHelper.ThrowIfNull(failure);

        var memberNames = string.IsNullOrEmpty(failure.PropertyPath) ? [] : new[] { failure.PropertyPath };
        return new ValidationResult(failure.Message, memberNames);
    }
}
