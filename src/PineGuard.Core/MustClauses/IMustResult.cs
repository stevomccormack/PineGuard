namespace PineGuard.MustClauses;

/// <summary>
/// Non-generic view over a <see cref="MustResult{T}"/>, letting results of different result
/// types be collected, inspected, or aggregated together without knowing <c>T</c>.
/// </summary>
/// <seealso cref="MustResult{T}"/>
/// <seealso cref="MustValidationResult"/>
public interface IMustResult
{
    /// <summary>
    /// Gets a value indicating whether the validation or parsing operation succeeded.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Gets a value indicating whether the validation or parsing operation failed.
    /// </summary>
    bool Failed { get; }

    /// <summary>
    /// Gets the stable, machine-readable identity of the rule that failed, or <see cref="string.Empty"/> on success.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets the human-readable failure message, or <see cref="string.Empty"/> on success.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Gets the raw message template with the <c>{paramName}</c> placeholder still present,
    /// or <see cref="string.Empty"/> on success.
    /// </summary>
    string MessageTemplate { get; }

    /// <summary>
    /// Gets the name of the parameter that failed validation, or <see langword="null"/> if unknown.
    /// </summary>
    string? ParamName { get; }

    /// <summary>
    /// The original value that was validated/parsed.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// The result produced by the operation when <see cref="Success"/> is <see langword="true"/>, boxed if necessary.
    /// </summary>
    object? Result { get; }
}
