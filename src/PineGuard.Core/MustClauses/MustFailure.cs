using System.Text;
using PineGuard.Common;

namespace PineGuard.MustClauses;

/// <summary>
/// One failure inside a <see cref="MustValidationResult"/>: where in the validated object it
/// occurred, the stable <see cref="Code"/> of the rule that failed, and the rendered message.
/// </summary>
/// <param name="PropertyPath">
/// Where in the validated object the failure is (e.g. <c>Email</c>, <c>Address.City</c>,
/// <c>Lines[2].Sku</c>), or <see cref="string.Empty"/> for the root.
/// </param>
/// <param name="Code">The stable, machine-readable identity of the rule that failed.</param>
/// <param name="Message">The rendered, human-readable failure message.</param>
/// <param name="Value">
/// The attempted value. Never serialized by any adapter — a value that may hold a secret must
/// not reach a response body, a log line, or a localisation table through this property.
/// </param>
public sealed record MustFailure(string PropertyPath, string Code, string Message, object? Value)
{
    /// <summary>
    /// Builds a <see cref="MustFailure"/> from a failed <see cref="IMustResult"/>.
    /// </summary>
    /// <param name="result">The failed result to convert.</param>
    /// <param name="propertyPath">
    /// When <see langword="null"/>, <see cref="PropertyPath"/> becomes <c>result.ParamName ?? ""</c> and
    /// <see cref="Message"/> is <c>result.Message</c> as-is. When given, <see cref="PropertyPath"/> becomes
    /// <paramref name="propertyPath"/> and <see cref="Message"/> is <c>result.MessageTemplate</c> re-rendered
    /// against it — so a validator's rule attribution never leaks the lambda parameter name into the message.
    /// </param>
    /// <returns>A <see cref="MustFailure"/> describing <paramref name="result"/>'s failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="result"/> represents a success.</exception>
    public static MustFailure From(IMustResult result, string? propertyPath = null)
    {
        ThrowHelper.ThrowIfNull(result);

        if (result.Success)
            throw new ArgumentException("The result must represent a failure.", nameof(result));

        return propertyPath is null
            ? new MustFailure(result.ParamName ?? string.Empty, result.Code, result.Message, result.Value)
            : new MustFailure(propertyPath, result.Code, MustMessage.Format(result.MessageTemplate, propertyPath), result.Value);
    }

    /// <summary>
    /// Overrides the compiler-generated record printer to omit <see cref="Value"/> — the PII guard
    /// documented on that property would otherwise be defeated by <see cref="ToString"/> and string
    /// interpolation (e.g. <c>$"{failure}"</c> in a log line).
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(nameof(PropertyPath)).Append(" = ").Append(PropertyPath);
        builder.Append(", ").Append(nameof(Code)).Append(" = ").Append(Code);
        builder.Append(", ").Append(nameof(Message)).Append(" = ").Append(Message);
        return true;
    }
}
