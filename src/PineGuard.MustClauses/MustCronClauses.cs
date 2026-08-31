using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate cron expressions,
/// delegating to <see cref="CronRules"/> for core validation logic.
/// </summary>
/// <seealso cref="CronRules"/>
/// <seealso href="https://pineguard.ai/docs/must/cron">Cron Must Clauses documentation</seealso>
public static class MustCronClauses
{
    /// <summary>
    /// Validates that the specified string is a cron expression in the given format.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a cron expression.</param>
    /// <param name="format">The field layout to validate against. Defaults to <see cref="CronFormat.Standard"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a cron expression in <paramref name="format"/>, or
    /// <see langword="false"/> with a descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="CronRules.IsCronExpression"/>, so every field is checked against its own
    /// range and the <c>@yearly</c>-style macros and the Quartz-only characters are rejected. The result
    /// carries the expression as written rather than its fields, because a caller that wants the firing
    /// times needs a scheduler rather than a splitter. The failure message follows the pattern
    /// <c>"{paramName} must be a valid cron expression."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.CronExpression(schedule);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="CronRules.IsCronExpression"/>
    /// <seealso href="https://pineguard.ai/docs/must/cron">Cron Must Clauses documentation</seealso>
    public static MustResult<string> CronExpression(this IMustClause _,
        string? value,
        CronFormat format = CronFormat.Standard,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Cron.Expression.Invalid, "{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a valid cron expression.";

        var ok = CronRules.IsCronExpression(value, format);
        return MustResult<string>.FromBool(ok, MustCodes.Cron.Expression.Invalid, messageTemplate, paramName, value, value);
    }
}
