using PineGuard.Common;

namespace PineGuard.MustClauses;

/// <summary>
/// The marker exception for "validation failed at a boundary". Thrown by
/// <see cref="MustValidationResult.ThrowIfFailed"/>.
/// </summary>
/// <remarks>
/// Deliberately does not derive from <see cref="ArgumentException"/>: single-value results throw
/// argument exceptions (<see cref="MustResult{T}.ThrowIfFailed()"/>, guards); validation results
/// throw <see cref="MustValidationException"/>. Not sealed — a consumer catching at a coarser or
/// finer granularity (e.g. <c>OrderValidationException : MustValidationException</c>) is a
/// legitimate use case.
/// </remarks>
/// <seealso cref="MustValidationResult"/>
public class MustValidationException : Exception
{
    /// <summary>
    /// Gets the validation result that produced this exception.
    /// </summary>
    public MustValidationResult Result { get; }

    /// <summary>
    /// Initializes a new instance whose <see cref="Exception.Message"/> is <c>result.Message</c>.
    /// </summary>
    /// <param name="result">The validation result that failed.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public MustValidationException(MustValidationResult result)
        : this(result, RequireResult(result).Message, innerException: null)
    {
    }

    /// <summary>
    /// Initializes a new instance with a custom message.
    /// </summary>
    /// <param name="result">The validation result that failed.</param>
    /// <param name="message">The exception message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public MustValidationException(MustValidationResult result, string message)
        : this(result, message, innerException: null)
    {
    }

    /// <summary>
    /// Initializes a new instance with a custom message and inner exception.
    /// </summary>
    /// <param name="result">The validation result that failed.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused this exception, if any.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public MustValidationException(MustValidationResult result, string message, Exception? innerException)
        : base(message, innerException)
    {
        Result = RequireResult(result);
    }

    private static MustValidationResult RequireResult(MustValidationResult result)
    {
        ThrowHelper.ThrowIfNull(result);
        return result;
    }
}
