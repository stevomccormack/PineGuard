namespace PineGuard.Testing.Common;

/// <summary>
/// A <see cref="TimeProvider"/> that returns each instant from a fixed sequence once, in order, and
/// throws once the sequence is exhausted — so a test can assert that a predicate under test samples
/// the clock exactly as many times as the sequence provides for, and fail loudly on resampling.
/// </summary>
/// <remarks>
/// Only <see cref="TimeProvider.GetUtcNow"/> and <see cref="TimeProvider.GetLocalNow"/> are fixed. The
/// inherited <see cref="TimeProvider.GetTimestamp"/>, <see cref="TimeProvider.TimestampFrequency"/> and
/// <see cref="TimeProvider.CreateTimer"/> are real elapsed-time/timer behavior — this type does not
/// virtualize them. Tests that need advancing or timer behavior should use
/// <c>Microsoft.Extensions.TimeProvider.Testing.FakeTimeProvider</c>.
/// </remarks>
/// <param name="sequence">The instants this provider reports, in order. Each is normalized to UTC regardless of its supplied offset.</param>
/// <param name="localTimeZone">The zone <see cref="TimeProvider.LocalTimeZone"/> reports. If <see langword="null"/>, <see cref="TimeZoneInfo.Utc"/>.</param>
public sealed class AdvancingTimeProvider(IReadOnlyList<DateTimeOffset> sequence, TimeZoneInfo? localTimeZone = null)
    : TimeProvider
{
    private readonly DateTimeOffset[] utcSequence = [.. sequence.Select(instant => instant.ToUniversalTime())];

    private int index;

    /// <summary>The number of times <see cref="GetUtcNow"/> has been called.</summary>
    public int UtcNowCallCount { get; private set; }

    /// <summary>
    /// Increments <see cref="UtcNowCallCount"/> and returns the next UTC-normalized instant in the
    /// sequence supplied to the constructor.
    /// </summary>
    /// <returns>The next instant in the sequence, normalized to UTC.</returns>
    /// <exception cref="InvalidOperationException">The sequence is exhausted.</exception>
    public override DateTimeOffset GetUtcNow()
    {
        UtcNowCallCount++;

        if (index >= utcSequence.Length)
            throw new InvalidOperationException($"{nameof(AdvancingTimeProvider)} sequence exhausted after {utcSequence.Length} read(s).");

        return utcSequence[index++];
    }

    /// <summary>
    /// The zone supplied to the constructor, or <see cref="TimeZoneInfo.Utc"/> when none was supplied.
    /// Never consults the host's local time zone.
    /// </summary>
    public override TimeZoneInfo LocalTimeZone => localTimeZone ?? TimeZoneInfo.Utc;
}
