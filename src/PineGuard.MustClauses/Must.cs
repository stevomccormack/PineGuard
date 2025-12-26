namespace PineGuard.MustClauses;

public static class Must
{
    public static IMustClause Be { get; } = new MustClause();
}
