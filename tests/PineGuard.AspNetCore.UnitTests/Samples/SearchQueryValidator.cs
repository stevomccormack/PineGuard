using PineGuard.MustClauses;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Fails a query whose term is not <see cref="SearchQuery.ValidTerm"/>, with one failure — so a test can
/// tell which argument a failure came from.
/// </summary>
public sealed class SearchQueryValidator : IMustValidator<SearchQuery>
{
    public MustValidationResult Validate(SearchQuery value) =>
        value.Term == SearchQuery.ValidTerm
            ? MustValidationResult.Ok()
            : MustValidationResult.Fail(SampleFailures.Term);

    public ValueTask<MustValidationResult> ValidateAsync(SearchQuery value, CancellationToken cancellationToken = default) => new(Validate(value));
}
