namespace PineGuard.Testing.Common;

/// <summary>
/// A <see cref="TimeProvider"/> that always reports the same instant, so validations that read the
/// current time can be asserted without depending on the machine clock.
/// </summary>
/// <param name="utcNow">The instant this provider reports. Supply it as UTC (a zero offset).</param>
public sealed class FixedTimeProvider(DateTimeOffset utcNow)
    : TimeProvider
{
    /// <summary>
    /// A shared provider pinned to 2026-06-15T12:00:00+00:00 — the instant temporal test scenarios
    /// are written against so that "past" and "future" mean the same thing in every test project.
    /// </summary>
    public static readonly FixedTimeProvider Default = new(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero));

    /// <summary>
    /// Returns the fixed instant supplied to the constructor. Repeated calls never advance.
    /// </summary>
    /// <returns>The instant this provider was constructed with.</returns>
    public override DateTimeOffset GetUtcNow() => utcNow;
}
