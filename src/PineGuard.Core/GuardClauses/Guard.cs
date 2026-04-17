namespace PineGuard.GuardClauses;

/// <summary>
/// Entry point for the fluent <c>Guard.Against.*</c> fail-fast validation API.
/// </summary>
/// <remarks>
/// Access <see cref="Against"/> to start a guard chain. All guard methods are
/// extension methods on <see cref="IGuardClause"/> and throw on invalid input.
/// </remarks>
/// <example>
/// <code>
/// Guard.Against.Null(value);
/// Guard.Against.Empty(items);
/// </code>
/// </example>
/// <seealso cref="IGuardClause"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public static class Guard
{
    private static readonly GuardClause GuardClause = new();

    /// <summary>
    /// Gets the <see cref="IGuardClause"/> entry point used to invoke fluent guard methods.
    /// </summary>
    public static IGuardClause Against => GuardClause;
}
