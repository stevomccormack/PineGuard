namespace PineGuard.MustClauses;

/// <summary>
/// Something that validates a whole <typeparamref name="T"/> and returns a structured <see cref="MustValidationResult"/>.
/// </summary>
/// <typeparam name="T">The type this validator validates.</typeparam>
/// <remarks>
/// The non-generic <see cref="IMustValidator"/> members are default interface implementations, so a
/// hand-rolled type implementing exactly one closed <see cref="IMustValidator{T}"/> only needs to write
/// <see cref="Validate(T)"/> and <see cref="ValidateAsync(T, CancellationToken)"/>. A type implementing two
/// closed <see cref="IMustValidator{T}"/>s inherits two candidate defaults and must implement
/// <see cref="IMustValidator"/> explicitly itself. <see cref="ValidateAsync(T, CancellationToken)"/> is on the
/// interface now (synchronous by default) so a later phase can add async rules without an interface change;
/// every future addition to this interface must be a default interface member, never an abstract one.
/// </remarks>
/// <seealso cref="IMustValidator"/>
/// <seealso cref="MustValidator{T}"/>
public interface IMustValidator<in T> : IMustValidator
    where T : notnull
{
    /// <summary>
    /// Validates <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    MustValidationResult Validate(T value);

    /// <summary>
    /// Validates <paramref name="value"/> asynchronously.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    ValueTask<MustValidationResult> ValidateAsync(T value, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    Type IMustValidator.ValidatedType => typeof(T);

    /// <inheritdoc/>
    MustValidationResult IMustValidator.Validate(object? value) => Validate(MustValidatorCast.To<T>(value));

    /// <inheritdoc/>
    ValueTask<MustValidationResult> IMustValidator.ValidateAsync(object? value, CancellationToken cancellationToken) =>
        ValidateAsync(MustValidatorCast.To<T>(value), cancellationToken);
}
