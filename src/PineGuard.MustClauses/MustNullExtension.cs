using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustNullExtension
{
    public static MustResult NotNull<T>(
        this IMustClause _,
        T value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must not be null.";
        var ok = value is not null;
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }

    public static MustResult Null<T>(
        this IMustClause _,
        T value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be null.";
        var ok = value is null;
        return MustResult.FromBool(ok, messageTemplate, paramName, value);
    }
}
