namespace PineGuard.Testing.Common;

/// <summary>
/// Deterministic <see cref="TimeZoneInfo"/> instances for temporal tests, so zone-aware behavior can
/// be asserted without depending on the host's installed time zone database.
/// </summary>
public static class TestTimeZones
{
    /// <summary>A fixed UTC+10 zone with no daylight saving.</summary>
    public static readonly TimeZoneInfo PlusTen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.PlusTen", TimeSpan.FromHours(10), "PineGuard Test +10", "PineGuard Test +10");

    /// <summary>A fixed UTC−8 zone with no daylight saving.</summary>
    public static readonly TimeZoneInfo MinusEight = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.MinusEight", TimeSpan.FromHours(-8), "PineGuard Test -8", "PineGuard Test -8");
}
