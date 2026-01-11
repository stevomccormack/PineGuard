namespace PineGuard.MustClauses;

public static class Must
{
    private static readonly MustClause MustClause = new();

    public static IMustClause Be => MustClause;
}
