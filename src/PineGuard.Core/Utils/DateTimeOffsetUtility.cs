namespace PineGuard.Utils;

/// <summary>
/// Provides internal projection helpers for <see cref="DateTimeOffset"/> instants. Not part of the
/// public surface: consumed only by <see cref="DateTimeUtility"/> and its friend assemblies.
/// </summary>
internal static class DateTimeOffsetUtility
{
    private static readonly TimeSpan MinOffset = TimeSpan.FromHours(-14);
    private static readonly TimeSpan MaxOffset = TimeSpan.FromHours(14);

    /// <summary>
    /// Attempts to project the specified instant into the wall-clock calendar date it represents at
    /// the given UTC offset. Reads no clock.
    /// </summary>
    /// <param name="instant">The instant to project.</param>
    /// <param name="offset">
    /// The UTC offset to project by. Must be a whole number of minutes within &#177;14 hours; otherwise,
    /// this method returns <see langword="false"/>.
    /// </param>
    /// <param name="date">
    /// When this method returns <see langword="true"/>, contains the midnight of the projected
    /// wall-clock date, with <see cref="DateTimeKind.Unspecified"/> kind. Otherwise, <see langword="default"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="offset"/> is a valid whole-minute UTC offset and the
    /// projected instant is representable as a <see cref="DateTime"/>; otherwise, <see langword="false"/>.
    /// </returns>
    internal static bool TryGetDateAtOffset(DateTimeOffset instant, TimeSpan offset, out DateTime date)
    {
        if (offset.Ticks % TimeSpan.TicksPerMinute != 0 || offset < MinOffset || offset > MaxOffset)
        {
            date = default;
            return false;
        }

        var projectedTicks = instant.UtcTicks + offset.Ticks;
        if (projectedTicks < DateTime.MinValue.Ticks || projectedTicks > DateTime.MaxValue.Ticks)
        {
            date = default;
            return false;
        }

        date = new DateTime(projectedTicks, DateTimeKind.Unspecified).Date;
        return true;
    }
}
