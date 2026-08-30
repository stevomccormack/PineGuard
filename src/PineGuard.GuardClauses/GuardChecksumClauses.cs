using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for check digits.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/checksum">Guard Checksum Clauses documentation</seealso>
public static class GuardChecksumClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> does not satisfy the Luhn checksum.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard. Spaces and hyphens are stripped before verification.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustChecksumClauses.Luhn"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> does not satisfy the Luhn checksum and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// A passing guard proves only that the digits are internally consistent — never that the
    /// sequence identifies a real account, device or person.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotLuhn(cardNumber);
    /// </code>
    /// </example>
    /// <seealso cref="MustChecksumClauses.Luhn"/>
    public static string NotLuhn(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Luhn(value, paramName); // Guard.Against.NotLuhn => Must.Be.Luhn (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
