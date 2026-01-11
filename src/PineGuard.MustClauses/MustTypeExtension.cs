using System.Globalization;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static class MustTypeExtension
{
    public static MustResult<Type> OfType(
        this IMustClause _,
        string value,
        Type type,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        var ok = type switch
        {
            _ when type == typeof(string)
                => !string.IsNullOrWhiteSpace(value),

            _ when type == typeof(int)
                => int.TryParse(value, out var _),

            _ when type == typeof(long)
                => long.TryParse(value, out var _),

            _ when type == typeof(double)
                => double.TryParse(value, out var _),

            _ when type == typeof(decimal)
                => decimal.TryParse(value, out var _),

            _ when type == typeof(bool)
                => bool.TryParse(value, out var _),

            _ when type == typeof(DateTime)
                => DateTime.TryParse(value, out var _),

            _ when type == typeof(Guid)
                => Guid.TryParse(value, out var _),

            _ when type == typeof(DayOfWeek)
                => int.TryParse(value, out var dayVal) && dayVal >= 0 && dayVal <= 6,

            _ when type.IsEnum
                => Enum.TryParse(type, value, true, out var enumVal) && Enum.IsDefined(type, enumVal),

            _ => TryChangeType(value, type)
        };

        return MustResult<Type>.FromBool(
            ok,
            "{paramName} must be a valid type.",
            paramName,
            value,
            type);
    }

    private static bool TryChangeType(string s, Type t)
    {
        try
        {
            _ = Convert.ChangeType(s, t, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
