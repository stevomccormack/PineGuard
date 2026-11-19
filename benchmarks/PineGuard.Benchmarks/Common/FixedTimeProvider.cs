namespace PineGuard.Benchmarks.Common;

/// <summary>
/// A <see cref="TimeProvider"/> that always reports the same instant in UTC, so the clock-reading workloads give the
/// same outcome on every run and on every machine.
/// </summary>
/// <param name="utcNow">The instant this provider reports, normalized to UTC regardless of the supplied offset.</param>
internal sealed class FixedTimeProvider(DateTimeOffset utcNow)
    : TimeProvider
{
    /// <summary>
    /// A shared provider pinned to 2026-06-15T12:00:00+00:00, the instant the repository's temporal test scenarios are
    /// written against.
    /// </summary>
    public static readonly FixedTimeProvider Default = new(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero));

    private readonly DateTimeOffset fixedUtcNow = utcNow.ToUniversalTime();

    /// <summary>
    /// Returns the fixed instant supplied to the constructor. Repeated calls never advance.
    /// </summary>
    /// <returns>The instant this provider was constructed with, normalized to UTC.</returns>
    public override DateTimeOffset GetUtcNow() => fixedUtcNow;

    /// <summary>
    /// Always <see cref="TimeZoneInfo.Utc"/>. Never consults the host's local time zone.
    /// </summary>
    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
}
