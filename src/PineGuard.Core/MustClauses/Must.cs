namespace PineGuard.MustClauses;

/// <summary>
/// Entry point for the fluent <c>Must.Be.*</c> validation API.
/// </summary>
/// <remarks>
/// Access <see cref="Be"/> to start a validation chain. All validation methods
/// are extension methods on <see cref="IMustClause"/>.
/// </remarks>
/// <example>
/// <code>
/// var result = Must.Be.True(isActive);
/// if (result.Failed)
///     Console.WriteLine(result.Message);
/// </code>
/// </example>
/// <seealso cref="IMustClause"/>
/// <seealso href="https://pineguard.ai/docs/must">Must Clauses documentation</seealso>
public static class Must
{
    private static readonly MustClause MustClause = new();

    /// <summary>
    /// Gets the <see cref="IMustClause"/> entry point used to invoke fluent validation methods.
    /// </summary>
    public static IMustClause Be => MustClause;
}
