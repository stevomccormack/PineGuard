namespace PineGuard.Utils;

public static class DateTimeUtility
{
    public static DateTime ToUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Utc => value,
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };

    public static TimeSpan Diff(DateTime value, DateTime other) =>
        ToUtc(value) - ToUtc(other);
}
