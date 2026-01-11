namespace PineGuard.GuardClauses;

public static class Guard
{
    private static readonly GuardClause GuardClause = new();

    public static IGuardClause Against => GuardClause;
}
