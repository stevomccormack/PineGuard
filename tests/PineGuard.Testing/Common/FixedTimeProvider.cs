namespace PineGuard.Testing.Common;

/// <summary>
/// A <see cref="TimeProvider"/> that always reports the same instant, so validations that read the
/// current time can be asserted without depending on the machine clock.
/// </summary>
/// <remarks>
/// Only <see cref="TimeProvider.GetUtcNow"/> and <see cref="TimeProvider.GetLocalNow"/> are fixed. The
/// inherited <see cref="TimeProvider.GetTimestamp"/>, <see cref="TimeProvider.TimestampFrequency"/> and
/// <see cref="TimeProvider.CreateTimer"/> are real elapsed-time/timer behavior — this type does not
/// virtualize them. Tests that need advancing or timer behavior should use
/// <c>Microsoft.Extensions.TimeProvider.Testing.FakeTimeProvider</c>.
/// </remarks>
/// <param name="utcNow">The instant this provider reports, normalized to UTC regardless of the supplied offset.</param>
/// <param name="localTimeZone">The zone <see cref="TimeProvider.LocalTimeZone"/> reports. If <see langword="null"/>, <see cref="TimeZoneInfo.Utc"/>.</param>
public sealed class FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)
    : TimeProvider
{
    /// <summary>
    /// A shared provider pinned to 2026-06-15T12:00:00+00:00 — the instant temporal test scenarios
    /// are written against so that "past" and "future" mean the same thing in every test project.
    /// </summary>
    public static readonly FixedTimeProvider Default = new(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero));

    /// <summary>2026-06-15T20:00:00Z in a UTC+10 zone: the local calendar date is 2026-06-16, one day ahead of the UTC date.</summary>
    public static readonly FixedTimeProvider DefaultAheadOfUtc = new(new DateTimeOffset(2026, 06, 15, 20, 0, 0, TimeSpan.Zero), TestTimeZones.PlusTen);

    /// <summary>2026-06-15T04:00:00Z in a UTC−8 zone: the local calendar date is 2026-06-14, one day behind the UTC date.</summary>
    public static readonly FixedTimeProvider DefaultBehindUtc = new(new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.Zero), TestTimeZones.MinusEight);

    private readonly DateTimeOffset fixedUtcNow = utcNow.ToUniversalTime();

    /// <summary>
    /// Returns the fixed instant supplied to the constructor. Repeated calls never advance.
    /// </summary>
    /// <returns>The instant this provider was constructed with, normalized to UTC.</returns>
    public override DateTimeOffset GetUtcNow() => fixedUtcNow;

    /// <summary>
    /// The zone supplied to the constructor, or <see cref="TimeZoneInfo.Utc"/> when none was supplied.
    /// Never consults the host's local time zone.
    /// </summary>
    public override TimeZoneInfo LocalTimeZone => localTimeZone ?? TimeZoneInfo.Utc;
}
