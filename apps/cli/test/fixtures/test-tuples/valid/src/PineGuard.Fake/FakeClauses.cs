namespace PineGuard.Fake;

/// <summary>
/// Fake Must-shaped clause the paired TestData in this fixture resolves
/// against — proves the tuple-element-to-parameter-name check when the
/// source method IS resolvable.
/// </summary>
public static class FakeClauses
{
    public static bool IsBetween(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
