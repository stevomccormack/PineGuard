using PineGuard.Utils;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static class MustGuidExtension
{
    public static MustResult<Guid> Guid(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be a valid GUID.";

        if (string.IsNullOrEmpty(value)
            || string.IsNullOrWhiteSpace(value)
            || value != value.Trim())
        {
            return MustResult<Guid>.Fail(messageTemplate, paramName, value);
        }

        var ok = GuidUtility.TryParse(value, out var parsed);
        return MustResult<Guid>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }
}
