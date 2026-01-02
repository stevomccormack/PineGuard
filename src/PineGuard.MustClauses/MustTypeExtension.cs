using System.Globalization;
using System.Runtime.CompilerServices;

namespace PineGuard.MustClauses;

public static partial class MustTypeExtension
{
    public static MustResult<Type> OfType(
        this IMustClause _,
        string value,
        Type type,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        bool ok = type switch
        {
            var t when t == typeof(string)
                => !string.IsNullOrWhiteSpace(value),

            var t when t == typeof(int)
                => int.TryParse(value, out int _),

            var t when t == typeof(long)
                => long.TryParse(value, out long _),

            var t when t == typeof(double)
                => double.TryParse(value, out double _),

            var t when t == typeof(decimal)
                => decimal.TryParse(value, out decimal _),

            var t when t == typeof(bool)
                => bool.TryParse(value, out bool _),

            var t when t == typeof(DateTime)
                => DateTime.TryParse(value, out DateTime _),

            var t when t == typeof(Guid)
                => Guid.TryParse(value, out Guid _),

            var t when t == typeof(System.DayOfWeek)
                => int.TryParse(value, out int dayVal) && dayVal >= 0 && dayVal <= 6,

            var t when t.IsEnum
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

    static bool TryChangeType(string s, Type t)
    {
        try
        {
            Convert.ChangeType(s, t, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
