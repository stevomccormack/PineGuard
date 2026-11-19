using System.Globalization;
using BenchmarkDotNet.Attributes;
using PineGuard.Benchmarks.Common;
using PineGuard.MustClauses;

namespace PineGuard.Benchmarks.Projections;

/// <summary>
/// The Must clause <see cref="MustStringDateOnlyClauses.PastDateOnly"/>, which returns a <see cref="MustResult{T}"/>.
/// On <c>main</c> it carries the rule inline; on the pilot branch it maps the rule object's outcome. The Guard,
/// FluentValidation and DataAnnotations adapters reach the rule through it on both trees.
/// </summary>
[MemoryDiagnoser]
public class MustStringDateOnlyClausesBenchmarks
{
    private string? _value;
    private DateTimeStyles _styles;

    [ParamsAllValues]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _value = PastDateOnlyInputs.Value(Input);
        _styles = PastDateOnlyInputs.Styles(Input);
    }

    [Benchmark]
    public MustResult<DateOnly> PastDateOnly() => Must.Be.PastDateOnly(_value, _styles, FixedTimeProvider.Default, paramName: "value");
}
