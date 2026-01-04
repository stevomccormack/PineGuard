namespace PineGuard.MustClauses;

public static class Must
{
    private static readonly MustClause _mustClause = new();

    public static IMustClause Be => _mustClause;
}
