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

    /// <summary>
    /// A UTC+1 zone that observes daylight saving on European rules: clocks go forward at 02:00 on the
    /// last Sunday of March and back at 03:00 on the last Sunday of October. In 2026 that is 29 March
    /// (02:00–02:59 do not exist) and 25 October (02:00–02:59 occur twice).
    /// </summary>
    public static readonly TimeZoneInfo DaylightSaving = TimeZoneInfo.CreateCustomTimeZone(
        "PineGuard.Test.DaylightSaving", TimeSpan.FromHours(1), "PineGuard Test DST", "PineGuard Test Standard", "PineGuard Test Daylight",
        [
            TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
                DateTime.MinValue.Date, DateTime.MaxValue.Date, TimeSpan.FromHours(1),
                TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 3, 5, DayOfWeek.Sunday),
                TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0), 10, 5, DayOfWeek.Sunday))
        ]);

    /// <summary>An invalid local time in <see cref="DaylightSaving"/>: 2026-03-29 02:30 falls in the spring-forward gap.</summary>
    public static readonly DateTime InvalidLocalTime = new(2026, 3, 29, 2, 30, 0, DateTimeKind.Unspecified);

    /// <summary>An ambiguous local time in <see cref="DaylightSaving"/>: 2026-10-25 02:30 occurs twice.</summary>
    public static readonly DateTime AmbiguousLocalTime = new(2026, 10, 25, 2, 30, 0, DateTimeKind.Unspecified);

    /// <summary>A local time in <see cref="DaylightSaving"/> that is neither invalid nor ambiguous.</summary>
    public static readonly DateTime OrdinaryLocalTime = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Unspecified);

    /// <summary>
    /// A UTC+10:30 zone that observes daylight saving on a half-hour transition: clocks go forward by
    /// 30 minutes at 02:00 on the first Sunday of October and back at 02:00 on the first Sunday of
    /// April. In 2026 that is 4 October (02:00–02:29 do not exist) and 5 April (01:30–01:59 occur twice).
    /// </summary>
    public static readonly TimeZoneInfo HalfHourDaylightSaving = TimeZoneInfo.CreateCustomTimeZone(
        "PineGuard.Test.HalfHourDaylightSaving", new TimeSpan(10, 30, 0), "PineGuard Test Half-Hour DST", "PineGuard Test Half-Hour Standard", "PineGuard Test Half-Hour Daylight",
        [
            TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
                DateTime.MinValue.Date, DateTime.MaxValue.Date, new TimeSpan(0, 30, 0),
                TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 10, 1, DayOfWeek.Sunday),
                TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 4, 1, DayOfWeek.Sunday))
        ]);

    /// <summary>An invalid local time in <see cref="HalfHourDaylightSaving"/>: 2026-10-04 02:15 falls in the half-hour spring-forward gap.</summary>
    public static readonly DateTime HalfHourInvalidLocalTime = new(2026, 10, 4, 2, 15, 0, DateTimeKind.Unspecified);

    /// <summary>An ambiguous local time in <see cref="HalfHourDaylightSaving"/>: 2026-04-05 01:45 occurs twice.</summary>
    public static readonly DateTime HalfHourAmbiguousLocalTime = new(2026, 4, 5, 1, 45, 0, DateTimeKind.Unspecified);

    /// <summary>A local time in <see cref="HalfHourDaylightSaving"/> that is neither invalid nor ambiguous.</summary>
    public static readonly DateTime HalfHourOrdinaryLocalTime = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Unspecified);

    /// <summary>An identifier that resolves on this host: a Windows id on Windows, an IANA id elsewhere (US Pacific). Used only by the tests that must resolve a real zone.</summary>
    public static readonly string PlatformSampleId = GetPlatformSampleId(OperatingSystem.IsWindows());

    /// <summary>Selects the sample-zone identifier for a platform: the Windows registry id when <paramref name="isWindows"/> is true, otherwise the IANA id. Pure; reads no host state.</summary>
    internal static string GetPlatformSampleId(bool isWindows) => isWindows ? "Pacific Standard Time" : "America/Los_Angeles";

    /// <summary>Invalid in <see cref="PlatformSampleId"/>: 2026-03-08 02:30 falls in the US spring-forward gap (second Sunday of March).</summary>
    public static readonly DateTime PlatformInvalidLocalTime = new(2026, 3, 8, 2, 30, 0, DateTimeKind.Unspecified);

    /// <summary>Ambiguous in <see cref="PlatformSampleId"/>: 2026-11-01 01:30 occurs twice (first Sunday of November).</summary>
    public static readonly DateTime PlatformAmbiguousLocalTime = new(2026, 11, 1, 1, 30, 0, DateTimeKind.Unspecified);

    /// <summary>A daylight-saving instant for the supported platform sample zone.</summary>
    public static readonly DateTimeOffset PlatformDaylightSavingInstant = new(2026, 7, 1, 12, 0, 0, TimeSpan.FromHours(-7));

    /// <summary>A standard-time instant for the supported platform sample zone.</summary>
    public static readonly DateTimeOffset PlatformStandardTimeInstant = new(2026, 1, 15, 12, 0, 0, TimeSpan.FromHours(-8));

    /// <summary>An identifier no host resolves.</summary>
    public const string UnknownId = "PineGuard/Nowhere";
}
