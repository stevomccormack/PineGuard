using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for cron expressions.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/cron">Guard Cron Clauses documentation</seealso>
public static class GuardCronClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a cron expression in the given format.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard as a cron expression.</param>
    /// <param name="format">The field layout to guard against. Defaults to <see cref="CronFormat.Standard"/>.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustCronClauses.CronExpression"/>.
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
    /// Thrown when <paramref name="value"/> is not a valid cron expression and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustCronClauses.CronExpression"/>:
    /// <c>Guard.Against.NotCronExpression</c> passes when the expression has the field count
    /// <paramref name="format"/> requires and every field falls inside its own range. The
    /// <c>@yearly</c>-style macros and the Quartz-only characters are not part of the supported
    /// dialect and therefore throw.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotCronExpression(schedule);
    /// </code>
    /// </example>
    /// <seealso cref="MustCronClauses.CronExpression"/>
    public static string NotCronExpression(
        this IGuardClause _,
        string? value,
        CronFormat format = CronFormat.Standard,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.CronExpression(value, format, paramName); // Guard.Against.NotCronExpression => Must.Be.CronExpression (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
