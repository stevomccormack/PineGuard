namespace PineGuard.Utils;

public static partial class IsoUtility
{
    public static class Dates
    {
        public static bool TryParseDateOnly(string? value, out DateOnly date) =>
            Iso.IsoDateUtility.TryParseDateOnly(value, out date);

        public static bool TryParseDateTime(string? value, out DateTime dateTime) =>
            Iso.IsoDateUtility.TryParseDateTime(value, out dateTime);

        public static bool TryParseDateTimeOffset(string? value, out DateTimeOffset dateTimeOffset) =>
            Iso.IsoDateUtility.TryParseDateTimeOffset(value, out dateTimeOffset);
    }
}
