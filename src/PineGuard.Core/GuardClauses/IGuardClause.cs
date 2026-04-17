namespace PineGuard.GuardClauses;

/// <summary>
/// Marker interface that serves as the entry point for the fluent <c>Guard.Against.*</c> API.
/// </summary>
/// <remarks>
/// Obtain an instance via <see cref="Guard.Against"/>. Extension methods on this interface provide
/// the full guard surface — e.g., <c>Guard.Against.Null(value)</c>.
/// </remarks>
/// <seealso cref="Guard"/>
/// <seealso cref="GuardClause"/>
/// <seealso href="https://pineguard.ai/docs/guard">Guard Clauses documentation</seealso>
public interface IGuardClause;
