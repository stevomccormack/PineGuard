namespace PineGuard.Fake;

public static class FakeClauses
{
    public static bool IsBetween(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
