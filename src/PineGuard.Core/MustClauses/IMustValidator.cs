namespace PineGuard.MustClauses;

/// <summary>
/// Non-generic entry point for validating a whole object and getting back a structured
/// <see cref="MustValidationResult"/>. Use <see cref="IMustValidator{T}"/> at compile time when
/// the validated type is known; this interface exists for runtime dispatch by <c>Type</c>.
/// </summary>
/// <seealso cref="IMustValidator{T}"/>
/// <seealso cref="MustValidator{T}"/>
public interface IMustValidator
{
    /// <summary>
    /// Gets the type this validator validates.
    /// </summary>
    Type ValidatedType { get; }

    /// <summary>
    /// Validates <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to validate. Must be assignable to <see cref="ValidatedType"/>, or <see langword="null"/>.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not <see langword="null"/> and not assignable to <see cref="ValidatedType"/>.</exception>
    MustValidationResult Validate(object? value);

    /// <summary>
    /// Validates <paramref name="value"/> asynchronously.
    /// </summary>
    /// <param name="value">The value to validate. Must be assignable to <see cref="ValidatedType"/>, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not <see langword="null"/> and not assignable to <see cref="ValidatedType"/>.</exception>
    ValueTask<MustValidationResult> ValidateAsync(object? value, CancellationToken cancellationToken = default);
}
