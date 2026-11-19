using System.Globalization;
using BenchmarkDotNet.Attributes;
using PineGuard.Benchmarks.Common;
using PineGuard.GuardClauses;

namespace PineGuard.Benchmarks.Projections;

/// <summary>
/// The Guard clause <see cref="GuardStringDateOnlyClauses.FutureOrPresentDateOnly"/>, which maps the Must result and
/// throws on failure. Measured on the passing input only: every failing input throws, and the cost of building and
/// throwing an exception would swamp the mapping being compared. The failing paths are measured, up to the throw, by
/// <see cref="MustStringDateOnlyClausesBenchmarks"/>, and including the exception by
/// <see cref="Failures.GuardStringDateOnlyClausesFailureBenchmarks"/>.
/// </summary>
[MemoryDiagnoser]
public class GuardStringDateOnlyClausesBenchmarks
{
    private string? _value;
    private DateTimeStyles _styles;

    [Params(PastDateOnlyInput.PastDate)]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _value = PastDateOnlyInputs.Value(Input);
        _styles = PastDateOnlyInputs.Styles(Input);
    }

    [Benchmark]
    public DateOnly FutureOrPresentDateOnly() =>
        Guard.Against.FutureOrPresentDateOnly(_value, _styles, FixedTimeProvider.Default, paramName: "value");
}
