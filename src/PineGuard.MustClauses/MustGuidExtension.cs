using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustGuidExtension
{
    public static MustResult Guid(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be a valid GUID.";

        if (string.IsNullOrEmpty(value)
            || string.IsNullOrWhiteSpace(value)
            || value != value.Trim())
        {
            return MustResult.Fail(messageTemplate, paramName, value);
        }

        var ok = System.Guid.TryParse(value, out Guid _);
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }
}
