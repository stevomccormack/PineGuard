using System.Globalization;
using BenchmarkDotNet.Attributes;
using PineGuard.Benchmarks.Common;
using PineGuard.Common;
using PineGuard.Rules;

namespace PineGuard.Benchmarks.Rules;

/// <summary>
/// The Core predicate behind the pilot rule, <see cref="StringRules.DateOnly.IsInPast"/>: parse the string, derive
/// today from the clock and compare, returning a <see cref="bool"/>. The pilot leaves it unchanged, so it is the
/// control of the comparison between <c>main</c> and the pilot branch.
/// </summary>
[MemoryDiagnoser]
public class StringRulesDateOnlyBenchmarks
{
    private string? _value;
    private DateTimeStyles _styles;

    [Params(PastDateOnlyInput.PastDate, PastDateOnlyInput.Today, PastDateOnlyInput.NotADate, PastDateOnlyInput.NullValue)]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _value = PastDateOnlyInputs.Value(Input);
        _styles = PastDateOnlyInputs.Styles(Input);
    }

    [Benchmark]
    public bool IsInPast() => StringRules.DateOnly.IsInPast(_value, Inclusion.Exclusive, _styles, FixedTimeProvider.Default);
}
