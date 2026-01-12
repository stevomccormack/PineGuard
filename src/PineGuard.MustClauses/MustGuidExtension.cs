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
        const string messageTemplate = "{paramName} must be a valid GUID.";

        var ok = GuidUtility.TryParse(value, out var parsed);
        return MustResult<Guid>.FromBool(ok, messageTemplate, paramName, value, parsed);
    }
}
