namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The <c>[AsParameters]</c> argument Plan 03's story 8 binds from the query string — validated exactly
/// like a body.
/// </summary>
public sealed class SearchQuery
{
    /// <summary>
    /// The only term <see cref="SearchQueryValidator"/> accepts.
    /// </summary>
    public const string ValidTerm = "pine";

    public string? Term { get; init; }

    public static SearchQuery Valid => new() { Term = ValidTerm };

    public static SearchQuery Invalid => new() { Term = "  " };
}
