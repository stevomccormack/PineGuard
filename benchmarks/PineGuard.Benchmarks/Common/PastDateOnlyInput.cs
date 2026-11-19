namespace PineGuard.Benchmarks.Common;

/// <summary>
/// The inputs every "string is a <see cref="DateOnly"/> in the past" workload is measured on. Each one takes a
/// different path through the rule; <see cref="PastDateOnlyInputs"/> supplies the value and parsing styles for each.
/// </summary>
public enum PastDateOnlyInput
{
    /// <summary><c>"2000-01-01"</c>: parses, and is before today. Passes.</summary>
    PastDate,

    /// <summary>Today's date on <see cref="FixedTimeProvider.Default"/>: parses, but is not before today. Fails.</summary>
    Today,

    /// <summary><c>"not-a-date"</c>: does not parse. Fails before the clock is read.</summary>
    NotADate,

    /// <summary><see langword="null"/>: fails where the layer rejects null and passes where it skips null.</summary>
    NullValue,

    /// <summary>
    /// <c>"2000-01-01"</c> with <see cref="System.Globalization.DateTimeStyles.AssumeUniversal"/>: rejected before the value is read.
    /// </summary>
    InvalidStyles
}
