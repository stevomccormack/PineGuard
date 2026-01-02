using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustNullExtension
{
    public static MustResult<T> NotNull<T>(
        this IMustClause _,
        T value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must not be null.";
        var ok = value is not null;
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    public static MustResult<T> Null<T>(
        this IMustClause _,
        T value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var messageTemplate = "{paramName} must be null.";
        var ok = value is null;
        return MustResult<T>.FromBool(ok, messageTemplate, paramName, value, result: default);
    }
}
