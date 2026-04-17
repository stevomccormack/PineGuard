namespace PineGuard.Common;

/// <summary>
/// Specifies whether range and comparison boundaries are inclusive or exclusive.
/// </summary>
public enum Inclusion
{
    /// <summary>
    /// The boundary values are included in the comparison.
    /// </summary>
    Inclusive,

    /// <summary>
    /// The boundary values are excluded from the comparison.
    /// </summary>
    Exclusive
}
