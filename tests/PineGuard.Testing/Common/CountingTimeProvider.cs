namespace PineGuard.Testing.Common;

/// <summary>
/// A <see cref="TimeProvider"/> that reports a fixed instant like <see cref="FixedTimeProvider"/>, but
/// also counts every <see cref="GetUtcNow"/> call so a test can assert exactly how many times a
/// predicate under test read the clock.
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
public sealed class CountingTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)
    : TimeProvider
{
    private readonly DateTimeOffset fixedUtcNow = utcNow.ToUniversalTime();

    /// <summary>The number of times <see cref="GetUtcNow"/> has been called.</summary>
    public int UtcNowCallCount { get; private set; }

    /// <summary>
    /// Increments <see cref="UtcNowCallCount"/> and returns the fixed instant supplied to the
    /// constructor. Repeated calls never advance.
    /// </summary>
    /// <returns>The instant this provider was constructed with, normalized to UTC.</returns>
    public override DateTimeOffset GetUtcNow()
    {
        UtcNowCallCount++;
        return fixedUtcNow;
    }

    /// <summary>
    /// The zone supplied to the constructor, or <see cref="TimeZoneInfo.Utc"/> when none was supplied.
    /// Never consults the host's local time zone.
    /// </summary>
    public override TimeZoneInfo LocalTimeZone => localTimeZone ?? TimeZoneInfo.Utc;
}
