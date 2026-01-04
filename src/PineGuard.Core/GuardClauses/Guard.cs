namespace PineGuard.GuardClauses;

public static class Guard
{
    private static readonly GuardClause _guardClause = new();

    public static IGuardClause Against => _guardClause;
}
