using System.Globalization;
using BenchmarkDotNet.Attributes;
using PineGuard.Benchmarks.Common;
using PineGuard.GuardClauses;

namespace PineGuard.Benchmarks.Failures;

/// <summary>
/// The failing paths of the Guard clause <see cref="GuardStringDateOnlyClauses.FutureOrPresentDateOnly"/>, which maps
/// the failed Must result to an exception and throws it. Each call builds, throws and catches one exception and returns
/// it, so the figures include the exception. The passing path is measured by
/// <see cref="Projections.GuardStringDateOnlyClausesBenchmarks"/>.
/// </summary>
[MemoryDiagnoser]
public class GuardStringDateOnlyClausesFailureBenchmarks
{
    private string? _value;
    private DateTimeStyles _styles;

    [Params(PastDateOnlyInput.Today, PastDateOnlyInput.NotADate, PastDateOnlyInput.NullValue)]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _value = PastDateOnlyInputs.Value(Input);
        _styles = PastDateOnlyInputs.Styles(Input);
    }

    [Benchmark]
    public ArgumentException FutureOrPresentDateOnly()
    {
        try
        {
            _ = Guard.Against.FutureOrPresentDateOnly(_value, _styles, FixedTimeProvider.Default, paramName: "value");
        }
        catch (ArgumentException exception)
        {
            return exception;
        }

        throw new InvalidOperationException("The input passed the Guard clause; every input of this class must fail it.");
    }
}
