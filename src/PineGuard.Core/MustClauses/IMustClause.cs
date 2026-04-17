namespace PineGuard.MustClauses;

/// <summary>
/// Marker interface that serves as the entry point for the fluent <c>Must.Be.*</c> validation API.
/// </summary>
/// <remarks>
/// Obtain an instance via <see cref="Must.Be"/>. Extension methods on this interface provide
/// the full validation surface — e.g., <c>Must.Be.True(value)</c>.
/// </remarks>
/// <seealso cref="Must"/>
/// <seealso cref="MustClause"/>
/// <seealso href="https://pineguard.ai/docs/must">Must Clauses documentation</seealso>
public interface IMustClause;
