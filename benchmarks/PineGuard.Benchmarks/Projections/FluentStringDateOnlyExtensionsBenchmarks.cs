using System.Globalization;
using BenchmarkDotNet.Attributes;
using FluentValidation;
using PineGuard.Benchmarks.Common;
using PineGuard.FluentValidation;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace PineGuard.Benchmarks.Projections;

/// <summary>
/// The FluentValidation extension <see cref="FluentStringDateOnlyExtensions.PastDateOnly{TModel}"/>, measured as one
/// <see cref="AbstractValidator{T}.Validate(T)"/> call on a validator built once in the global setup. The figures
/// include FluentValidation's own per-call cost, which is the same on both trees.
/// </summary>
[MemoryDiagnoser]
public class FluentStringDateOnlyExtensionsBenchmarks
{
    private PastDateOnlyValidator _validator = null!;
    private Model _model = null!;

    [ParamsAllValues]
    public PastDateOnlyInput Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _validator = new PastDateOnlyValidator(FixedTimeProvider.Default, PastDateOnlyInputs.Styles(Input));
        _model = new Model { Value = PastDateOnlyInputs.Value(Input) };
    }

    [Benchmark]
    public FluentValidationResult PastDateOnly() => _validator.Validate(_model);

    private sealed record Model { public string? Value { get; init; } }

    private sealed class PastDateOnlyValidator : AbstractValidator<Model>
    {
        public PastDateOnlyValidator(TimeProvider? timeProvider, DateTimeStyles styles) =>
            RuleFor(x => x.Value).PastDateOnly(timeProvider, styles);
    }
}
